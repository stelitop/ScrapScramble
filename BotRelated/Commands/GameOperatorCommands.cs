using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ScrapScramble.BotRelated.Attributes;
using ScrapScramble.Game;
using ScrapScramble.Game.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
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
                BotInfoHandler.inGame = true;
                BotInfoHandler.shopsSent = false;
                BotInfoHandler.gameHandler.StartNewGame();
                BotInfoHandler.gameHandler.currentRound = 1;
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);

                await ctx.Client.UpdateStatusAsync(new DiscordActivity
                {
                    Name = "Scrap Scramble | >help",
                    ActivityType = ActivityType.Playing
                });

                await SendShops(ctx);
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

                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Shops sent out successfully",
                    Description = "All players have received their shops",
                    Color = DiscordColor.Green
                };

                for (int i = 0; i < BotInfoHandler.participantsDiscordIds.Count(); i++)
                {
                    if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                    await BotInfoHandler.SendNewUI(ctx, i);
                }

                await msg.DeleteAsync().ConfigureAwait(false);
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
            BotInfoHandler.UIMessages.Clear();

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
            BotInfoHandler.gameHandler.currentRound++;
            BotInfoHandler.shopsSent = false;
            for (int i = 0; i < BotInfoHandler.UIMessages.Count(); i++)
            {
                BotInfoHandler.UIMessages[i] = null;
            }
            GameHandlerMethods.NextRound(ref BotInfoHandler.gameHandler);

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = "New Round Started",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);


            await BotInfoHandler.RefreshPlayerList(ctx);
            await SendShops(ctx);
        }

        [Command("pairslist")]
        [Description("Displays all pairs of players that will fight in the next round.")]
        [RequireIngame]
        public async Task GetPairsList(CommandContext ctx)
        {
            Console.WriteLine(1);
            string msg = string.Empty;
            Console.WriteLine(2);
            for (int i=0; i<BotInfoHandler.gameHandler.pairsHandler.opponents.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                Console.WriteLine($"{BotInfoHandler.gameHandler.pairsHandler.opponents.Count()}: {i}, {BotInfoHandler.gameHandler.pairsHandler.opponents[i]}");
                if (i < BotInfoHandler.gameHandler.pairsHandler.opponents[i]) msg += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} vs {BotInfoHandler.gameHandler.pairsHandler.opponents[i]+1}) {BotInfoHandler.gameHandler.players[BotInfoHandler.gameHandler.pairsHandler.opponents[i]].name}\n";                
            }
            Console.WriteLine(3);
            for (int i=0; i<BotInfoHandler.gameHandler.pairsHandler.opponents.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.players[i].lives <= 0) continue;
                if (i == BotInfoHandler.gameHandler.pairsHandler.opponents[i]) msg += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} gets a bye\n";
            }
            Console.WriteLine(4);

            if (msg.Equals(string.Empty)) msg = "No pairs have been assigned yet.";
            else msg.Trim();
            Console.WriteLine(5);
            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "List of Mech Pairs for Combat",
                Description = msg,
                Color = DiscordColor.Azure
            };
            Console.WriteLine(6);
            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            Console.WriteLine(7);
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
            if (pl1 < 1 || pl1 > BotInfoHandler.participantsDiscordIds.Count()) return;
            if (pl2 < 1 || pl2 > BotInfoHandler.participantsDiscordIds.Count()) return;
            //if (pl1 == pl2) return;
            pl1--;
            pl2--;
            if (BotInfoHandler.gameHandler.players[pl1].lives <= 0) return;
            if (BotInfoHandler.gameHandler.players[pl2].lives <= 0) return;

            GameHandlerMethods.StartBattle(ref BotInfoHandler.gameHandler, pl1, pl2);
            
            var fightMessage = new DiscordEmbedBuilder{
                Title = "Combat!",
                Color = DiscordColor.Gold
            };
            
            string msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader[i] + "\n";
            }            
            fightMessage.AddField("[The Fighters]", msg);
            
            msg = string.Empty;            
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.statsHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.statsHeader[i] + "\n";
            }            
            fightMessage.AddField("[Stats]", msg);

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

        [Command("interactiveplayerlist")]
        [RequireIngame]
        [RequireGuild]
        public async Task CreateInteractivePlayerlist(CommandContext ctx)
        {
            await BotInfoHandler.SendNewPlayerList(ctx);
        }

        [Command("setstartinglives")]
        public async Task SetStartingLives(CommandContext ctx, int num)
        {
            if (num < 1) num = 1;
            BotInfoHandler.gameHandler.startingLives = num;

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = "Startings Lives Changed Successfully",
                Description = $"Players will now start games with {num} lives.",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);
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
                    string name = arguments[2];
                    for (int i = 3; i < arguments.Count(); i++) name = name + $" {arguments[i]}";

                    switch (arguments[1])
                    {
                        case "+=":

                            for (int i = 0; i < BotInfoHandler.gameHandler.pool.mechs.Count(); i++)
                                if (BotInfoHandler.gameHandler.pool.mechs[i].name.Equals(name, StringComparison.OrdinalIgnoreCase))
                                {
                                    BotInfoHandler.gameHandler.players[index].shop.options.Add((Mech)BotInfoHandler.gameHandler.pool.mechs[i].DeepCopy());
                                    return true;
                                }
                            return false;

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
        public async Task NextOpponents(CommandContext ctx)
        {            
            BotInfoHandler.gameHandler.pairsHandler.NextRoundPairs(ref BotInfoHandler.gameHandler);

            await GetPairsList(ctx);
        }

        [Command("wait")]
        public async Task TestWait(CommandContext ctx, TimeSpan time)
        {
            await ctx.RespondAsync("Frog");

            Thread.Sleep(time);

            await ctx.RespondAsync("Champ");
        }
    }
}
