using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ScrapScramble.BotRelated.Attributes;
using ScrapScramble.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string msg = string.Empty;
            for (int i=0; i<BotInfoHandler.gameHandler.opponents.Count(); i++)
            {
                if (i < BotInfoHandler.gameHandler.opponents[i]) msg += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} vs {BotInfoHandler.gameHandler.opponents[i]+1}) {BotInfoHandler.gameHandler.players[BotInfoHandler.gameHandler.opponents[i]].name}\n";
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

            BotInfoHandler.gameHandler.opponents[BotInfoHandler.gameHandler.opponents[pl1]] = BotInfoHandler.gameHandler.opponents[pl1];
            BotInfoHandler.gameHandler.opponents[BotInfoHandler.gameHandler.opponents[pl2]] = BotInfoHandler.gameHandler.opponents[pl2];

            BotInfoHandler.gameHandler.opponents[pl1] = pl2;
            BotInfoHandler.gameHandler.opponents[pl2] = pl1;

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

        [Command("removedead")]
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
    }
}
