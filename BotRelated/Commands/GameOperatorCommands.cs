using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ScrapScramble.BotRelated.Attributes;
using ScrapScramble.Game;
using ScrapScramble.Game.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScrapScramble.BotRelated.Commands
{

    [RequireRoles(RoleCheckMode.All, "Game Operator")]
    public class GameOperatorCommands : BaseCommandModule
    {
        [Command("startgame")]
        [Description("Starts a new game.")]
        [RequireGuild]
        public async Task StartGame(CommandContext ctx)
        {
            const int minPlayers = 1;
            DiscordEmbedBuilder responseMessage;

            if (BotInfoHandler.gameHandler.players.Count() < minPlayers) //amount to be changed
            {
                //not enough players
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Not enough players to start a game",
                    Description = $"You need at least {minPlayers} players to start a game.",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            }
            else if (BotInfoHandler.inGame)
            {
                //already in game
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "A game is already in the process.",
                    Description = "Cancel the current game to start another.",
                    Color = DiscordColor.Red
                };

                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            }
            else
            {
                //successfully starting a game
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Game successfully started.",
                    Description = "More commands are now available.",
                    Color = DiscordColor.Green
                };

                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);

                MinionPool pool = new MinionPool();                

                if (BotInfoHandler.gameHandler.setsAmount <= 0)
                {
                    pool.FillGenericMinionPool();
                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Sets Included This Game",
                        Description = "All sets are included.",
                        Color = DiscordColor.Azure
                    };
                }
                else
                {
                    var setNames = pool.FillMinionPoolWithSets(BotInfoHandler.gameHandler.setsAmount, BotInfoHandler.gameHandler.setHandler);
                    string desc = setNames[0];
                    for (int i = 1; i < BotInfoHandler.gameHandler.setsAmount; i++) desc += $"\n{setNames[i]}";
                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Sets Included This Game",
                        Description = desc,
                        Color = DiscordColor.Azure
                    };
                }

                await Task.Delay(1000);
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);                

                BotInfoHandler.inGame = true;
                BotInfoHandler.shopsSent = false;
                BotInfoHandler.gameHandler.StartNewGame(pool);
                BotInfoHandler.gameHandler.currentRound = 1;
                BotInfoHandler.pairsReady = false;                

                ctx.Client.UpdateStatusAsync(new DiscordActivity
                {
                    Name = "Scrap Scramble | >help",
                    ActivityType = ActivityType.Playing
                });

                await Task.Delay(1000);
                await NextPairs(ctx);
                await Task.Delay(1000);
                await SendShops(ctx);
                await Task.Delay(1000);
                await CreateInteractivePlayerlist(ctx);

                BotInfoHandler.pairsReady = false;
            }

        }

        //[Command("sendshops")]
        [RequireGuild]
        [RequireIngame]
        public async Task SendShops(CommandContext ctx)
        {
            DiscordEmbedBuilder responseMessage;

            if (BotInfoHandler.shopsSent == true)
            {
                //shop already sent
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have already sent the shops to the players",
                    Description = "This is to prevent spam.",
                    Color = DiscordColor.Red
                };
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            }
            else
            {
                BotInfoHandler.shopsSent = true;
                //shops not sent yet
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Sending Shops to the Players",
                    Description = "This may take a while.",
                    Color = DiscordColor.Gray
                };

                var msg = await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);                

                for (int i = 0; i < BotInfoHandler.participantsDiscordIds.Count(); i++)
                {
                    if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                    await BotInfoHandler.SendNewUI(ctx, i);
                    await Task.Delay(1000);
                }

                await msg.DeleteAsync().ConfigureAwait(false);

                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Shops sent out successfully",
                    Description = "All players have received their shops",
                    Color = DiscordColor.Green
                };
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            }
        }

        [Command("cancelgame")]
        [Description("Cancels the currently running game.")]
        [RequireGuild]
        [RequireIngame]
        public async Task CancelGame(CommandContext ctx)
        {
            BotInfoHandler.gameHandler = new Game.GameHandler();
            BotInfoHandler.inGame = false;
            BotInfoHandler.gameHandler.currentRound = 1;
            BotInfoHandler.participantsDiscordIds.Clear();
            BotInfoHandler.shopsSent = false;
            BotInfoHandler.pairsReady = false;
            BotInfoHandler.UIMessages.Clear();
            BotInfoHandler.interactivePlayerList = null;
            BotInfoHandler.interactivePlayerListCaller = null;

            await ctx.Client.UpdateStatusAsync(new DiscordActivity
            {
                Name = $"({BotInfoHandler.participantsDiscordIds.Count()}) Waiting to >signup",
                ActivityType = ActivityType.Playing
            });

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                Title = "Game Cancelled Successfully",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);
        }        

        [Command("nextround")]
        [Description("Proceeds to the next round of the game. Also sends each player their shop.")]
        [RequireGuild]
        [RequireIngame]
        public async Task NextRound(CommandContext ctx)
        {
            if (BotInfoHandler.pairsReady == false)
            {
                await NextPairs(ctx);

                //await ctx.RespondAsync(embed: new DiscordEmbedBuilder {
                //    Title = "Pairs Not Assigned",
                //    Description = "Type >nextpairs to be able to proceed to the next round.",
                //    Color = DiscordColor.Red
                //}).ConfigureAwait(false);
                //return;
            }
            BotInfoHandler.gameHandler.currentRound++;
            BotInfoHandler.shopsSent = false;
            BotInfoHandler.pairsReady = false;

            for (int i = 0; i < BotInfoHandler.UIMessages.Count(); i++)
            {
                BotInfoHandler.UIMessages[i] = null;
            }
            GameHandlerMethods.NextRound(BotInfoHandler.gameHandler);

            await Task.Delay(3000);

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = "New Round Started",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);

            //await Task.Delay(1000);
            //await BotInfoHandler.RefreshPlayerList(ctx);
            await Task.Delay(1000);
            await SendShops(ctx);
            await Task.Delay(1000);
            await CreateInteractivePlayerlist(ctx);
        }

        [Aliases("pairlist")]
        [Command("pairslist")]
        [Description("Displays all pairs of players that will fight in the next round.")]
        [RequireIngame]
        public async Task GetPairsList(CommandContext ctx)
        {
            string msg = string.Empty;            
            for (int i=0; i<BotInfoHandler.gameHandler.pairsHandler.opponents.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                Console.WriteLine($"{BotInfoHandler.gameHandler.pairsHandler.opponents.Count()}: {i}, {BotInfoHandler.gameHandler.pairsHandler.opponents[i]}");
                if (i < BotInfoHandler.gameHandler.pairsHandler.opponents[i]) msg += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} vs {BotInfoHandler.gameHandler.pairsHandler.opponents[i]+1}) {BotInfoHandler.gameHandler.players[BotInfoHandler.gameHandler.pairsHandler.opponents[i]].name}\n";                
            }            
            for (int i=0; i<BotInfoHandler.gameHandler.pairsHandler.opponents.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                if (i == BotInfoHandler.gameHandler.pairsHandler.opponents[i]) msg += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} gets a bye\n";
            }            

            if (msg.Equals(string.Empty)) msg = "No pairs have been assigned yet.";
            else msg.Trim();
            
            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "List of Mech Pairs for Combat",
                Description = msg,
                Color = DiscordColor.Azure
            };
            
            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);            
        }

        [Command("pair")]
        [Description("Pairs two players together to fight each other in the next round.")]
        [RequireIngame]
        public async Task PairPlayers(CommandContext ctx, [Description("Player 1")]int pl1, [Description("Player 2")]int pl2)
        {
            if (pl1 < 1 || pl1 > BotInfoHandler.participantsDiscordIds.Count()) return;
            if (pl2 < 1 || pl2 > BotInfoHandler.participantsDiscordIds.Count()) return;
            pl1--;
            pl2--;
            if (BotInfoHandler.gameHandler.players[pl1].lives <= 0) return;
            if (BotInfoHandler.gameHandler.players[pl2].lives <= 0) return;

            BotInfoHandler.gameHandler.pairsHandler.SetPair(pl1, pl2);

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                Title = "Pair Assigned Successfully",
                Description = $"{BotInfoHandler.gameHandler.players[pl1].name} and {BotInfoHandler.gameHandler.players[pl2].name} have been assigned as opponents.",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);
        }

        [Command("fight")]
        [Description("Makes two players fight each other.")]
        [RequireIngame]
        [RequireGuild]
        public async Task Fight(CommandContext ctx, [Description("Player 1")]int pl1, [Description("Player 2")]int pl2)
        {
            //should make this send an embed

            if (pl1 < 1 || pl1 > BotInfoHandler.participantsDiscordIds.Count()) return;
            if (pl2 < 1 || pl2 > BotInfoHandler.participantsDiscordIds.Count()) return;
            //if (pl1 == pl2) return;
            pl1--;
            pl2--;
            if (BotInfoHandler.gameHandler.players[pl1].lives <= 0) return;
            if (BotInfoHandler.gameHandler.players[pl2].lives <= 0) return;

            GameHandlerMethods.StartBattle(BotInfoHandler.gameHandler, pl1, pl2);

            var fightMessage = new DiscordEmbedBuilder{
                Title = $"Combat! {BotInfoHandler.gameHandler.players[pl1].name} vs {BotInfoHandler.gameHandler.players[pl2].name}",
                Color = DiscordColor.Gold
            };

            string msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader1.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader1[i] + "\n";
            }            
            fightMessage.AddField($"{BotInfoHandler.gameHandler.players[pl1].name} upgraded with:", msg, true);

            msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader2.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader2[i] + "\n";
            }
            fightMessage.AddField($"{BotInfoHandler.gameHandler.players[pl2].name} upgraded with:", msg, true);

            //msg = string.Empty;            
            //for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.statsHeader.Count(); i++)
            //{
            //    msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.statsHeader[i] + "\n";
            //}            

            //fightMessage.AddField("[Stats]", msg);

            msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.preCombatHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.preCombatHeader[i] + "\n";
            }
            fightMessage.AddField("[Pre-Combat]", msg);

            msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.combatHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.combatHeader[i] + "\n";
            }
            fightMessage.AddField("[Combat]", msg);            
            
            await ctx.RespondAsync(embed: fightMessage).ConfigureAwait(false);
        }

        [Aliases("interactiveplayerslist")]
        [Command("interactiveplayerlist")]
        [RequireIngame]
        [RequireGuild]
        public async Task CreateInteractivePlayerlist(CommandContext ctx)
        {
            await BotInfoHandler.SendNewPlayerList(ctx);
        }

        //[Command("removedead")]
        [RequireIngame]
        public async Task RemoveDeadPlayers(CommandContext ctx)
        {
            string deadNames = string.Empty;
            for (int i=0; i<BotInfoHandler.gameHandler.players.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.players[i].lives <= 0)
                {
                    deadNames = $"{deadNames}{BotInfoHandler.gameHandler.players[i].name} ";
                    BotInfoHandler.RemovePlayer(i);
                    i--;
                }
            }

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                Title = $"{deadNames}Have Been Eliminated",
                Color = DiscordColor.Aquamarine
            }).ConfigureAwait(false);
        }

        private bool ProcessEditCommand(string msg, int index)
        {
            var arguments = msg.Split(' ');
            if (arguments.Count() == 0) return false;

            int val = 0;

            switch (arguments[0].ToLower())
            {
                case "attack":
                case "health":
                case "mana":
                case "lives":
                    if (arguments.Count() != 3) break;
                    if (!int.TryParse(arguments[2], out val)) return false;

                    ref int chosenVar = ref BotInfoHandler.gameHandler.players[index].curMana;

                    switch (arguments[0].ToLower())
                    {
                        case "attack": { chosenVar = ref BotInfoHandler.gameHandler.players[index].creatureData.attack; break; }
                        case "health": { chosenVar = ref BotInfoHandler.gameHandler.players[index].creatureData.health; break; }
                        case "mana": { chosenVar = ref BotInfoHandler.gameHandler.players[index].curMana; break; }
                        case "lives": { chosenVar = ref BotInfoHandler.gameHandler.players[index].lives; break; }
                        default: { return false; }
                    }

                    switch(arguments[1])
                    {
                        case "=":
                            chosenVar = val;
                            return true;

                        case "+=":
                            chosenVar += val;
                            return true;

                        case "-=":
                            chosenVar -= val;
                            return true;

                        default:
                            return false;
                    }
                case "shop":
                    if (arguments.Count() < 3) break;
                    string shopName = arguments[2];
                    for (int i = 3; i < arguments.Count(); i++) shopName += $" {arguments[i]}";

                    switch (arguments[1])
                    {
                        case "+=":

                            for (int i = 0; i < BotInfoHandler.gameHandler.players[index].pool.upgrades.Count(); i++)
                                if (BotInfoHandler.gameHandler.players[index].pool.upgrades[i].name.Equals(shopName, StringComparison.OrdinalIgnoreCase))
                                {
                                    BotInfoHandler.gameHandler.players[index].shop.AddUpgrade(BotInfoHandler.gameHandler.pool.upgrades[i]);
                                    return true;
                                }
                            return false;

                        default:
                            return false;
                    }

                case "hand":
                    if (arguments.Count() < 3) break;
                    string handName = arguments[2];
                    for (int i = 3; i < arguments.Count(); i++) handName += $" {arguments[i]}";

                    switch (arguments[1])
                    {
                        case "+=":

                            for (int i = 0; i < BotInfoHandler.gameHandler.pool.upgrades.Count(); i++)
                                if (BotInfoHandler.gameHandler.pool.upgrades[i].name.Equals(handName, StringComparison.OrdinalIgnoreCase))
                                {
                                    BotInfoHandler.gameHandler.players[index].hand.AddCard(BotInfoHandler.gameHandler.pool.upgrades[i]);
                                    return true;
                                }
                            return false;

                        default:
                            return false;
                    }

                case "name":
                    if (arguments.Count() < 3) break;
                    string playerName = arguments[2];
                    for (int i = 3; i < arguments.Count(); i++) playerName += $" {arguments[i]}";

                    switch (arguments[1])
                    {
                        case "=":
                            BotInfoHandler.gameHandler.players[index].name = playerName;                                
                            return true;

                        default:
                            return false;
                    }

                default:
                    return false;
            }
            return false;
        }

        [Command("editplayer")]
        [RequireIngame]
        public async Task EditPlayer(CommandContext ctx, int index)
        {
            if (index < 1 || index > BotInfoHandler.gameHandler.players.Count()) return;
            index--;
            
            string subCommand = string.Empty;

            while (true)
            {
                var interactivity = ctx.Client.GetInteractivity();

                var msgResult = await interactivity.WaitForMessageAsync(
                    x => x.Channel == ctx.Channel &&
                    x.Author == ctx.User).ConfigureAwait(false);                

                if (msgResult.TimedOut) break;

                if (msgResult.Result.Content.ToLower().Equals("end"))
                {
                    msgResult.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
                    BotInfoHandler.RefreshUI(ctx, index);

                    break;
                }

                bool result = ProcessEditCommand(msgResult.Result.Content, index);

                if (result)
                {
                    await msgResult.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);
                }
                else
                {
                    await msgResult.Result.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
                }
            }
        }

        [Command("nextpairs")]
        [RequireIngame]
        [Description("Generaters the opponents for the next round")]
        public async Task NextPairs(CommandContext ctx)
        {            
            BotInfoHandler.gameHandler.pairsHandler.NextRoundPairs(BotInfoHandler.gameHandler);

            await GetPairsList(ctx);
        }

        [Command("wait")]
        public async Task TestWait(CommandContext ctx, TimeSpan time)
        {
            await ctx.RespondAsync("Frog");

            Thread.Sleep(time);

            await ctx.RespondAsync("Champ");
        }

        [Command("playerinfo")]
        [RequireIngame]
        [Description("Shows the information about a specfic player")]
        public async Task PlayerInfo(CommandContext ctx, int index)
        {
            if (index < 1 || index > BotInfoHandler.gameHandler.players.Count()) return;
            index--;

            if (BotInfoHandler.gameHandler.players[index].lives <= 0)
            {
                //dead player
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder {
                    Title = "This Player is Dead",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
            }
            else
            {
                int dummyInt;
                string desc = string.Empty;


                desc += $"**{BotInfoHandler.gameHandler.players[index].name} upgraded with:**\n";
                desc += BotInfoHandler.gameHandler.players[index].GetUpgradesList(out dummyInt);
                desc += "\n\n";
                desc += BotInfoHandler.gameHandler.players[index].GetInfoForCombat(BotInfoHandler.gameHandler);
                

                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = $"{BotInfoHandler.gameHandler.players[index].name}'s Info",
                    Description = desc,
                    Color = DiscordColor.Gold
                }).ConfigureAwait(false);
            }
        }                        

        //[Command("gamewithpackages")]
        //public async Task GameWithPackages(CommandContext ctx, int packages = 4)
        //{
        //    if (packages < 1) packages = 1;
        //    List<string> packagesNames = BotInfoHandler.gameHandler.pool.FillMinionPoolWithSets(packages, BotInfoHandler.gameHandler.setHandler);

        //    string description = $"- {packagesNames[0]}";

        //    for (int i=1; i<packagesNames.Count(); i++)
        //    {
        //        description += $"\n- {packagesNames[i]}";
        //    }

        //    await ctx.RespondAsync(embed: new DiscordEmbedBuilder {
        //        Title = $"Number of Sets set to {packages}",
        //        Description = description,
        //        Color = DiscordColor.Green
        //    }).ConfigureAwait(false);
        //}        

        [Command("savegame")]
        public async Task ExportGame(CommandContext ctx)
        {
            if (BotInfoHandler.inGame)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("game.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, new ExportInfo(BotInfoHandler.gameHandler, BotInfoHandler.participantsDiscordIds, BotInfoHandler.shopsSent, BotInfoHandler.pairsReady));

                await ctx.RespondAsync(embed: new DiscordEmbedBuilder { 
                    Title = "Game Successfully Saved",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
            else
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder { 
                    Title = "Your Are Not in a Game",
                    Description = "You must first start a game to able to save it.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
            }
        }

        [Command("loadgame")]
        public async Task ImportGame(CommandContext ctx)
        { 
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("game.bin", FileMode.Open, FileAccess.Read);
            ExportInfo gameInfo = (ExportInfo)formatter.Deserialize(stream);

            BotInfoHandler.inGame = true;
            BotInfoHandler.gameHandler = gameInfo.gameHandler;
            BotInfoHandler.pairsReady = gameInfo.pairsReady;
            BotInfoHandler.participantsDiscordIds = gameInfo.participantsDiscordIds;

            BotInfoHandler.UIMessages = new List<DiscordMessage>();
            BotInfoHandler.interactivePlayerList = null;
            BotInfoHandler.interactivePlayerListCaller = null;

            await SendShops(ctx);
        }
    }    
}
