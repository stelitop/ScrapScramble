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
                BotInfoHandler.currentRound = 1;
                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);

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
        [RequireGuild]
        [RequireIngame]
        public async Task CancelGame(CommandContext ctx)
        {
            BotInfoHandler.gameHandler = new Game.GameHandler();
            BotInfoHandler.inGame = false;
            BotInfoHandler.currentRound = 1;
            BotInfoHandler.participantsDiscordIds.Clear();
            BotInfoHandler.shopsSent = false;
            BotInfoHandler.UIMessages.Clear();

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                Title = "Game Cancelled Successfully",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);
        }

        [Command("nextround")]
        [RequireGuild]
        [RequireIngame]
        public async Task NextRound(CommandContext ctx)
        {
            BotInfoHandler.currentRound++;
            BotInfoHandler.shopsSent = false;
            for (int i = 0; i < BotInfoHandler.UIMessages.Count(); i++)
            {
                BotInfoHandler.UIMessages[i] = null;
            }
            GameHandlerMethods.NextRound(ref BotInfoHandler.gameHandler);

            Console.WriteLine("ee");

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = "New Round Started",
                Color = DiscordColor.Green
            }).ConfigureAwait(false);

            await SendShops(ctx);
        }

        [Command("pairslist")]
        [RequireIngame]
        public async Task GetPairsList(CommandContext ctx)
        {            
            string msg = string.Empty;
            for (int i=0; i<BotInfoHandler.gameHandler.opponents.Count(); i++)
            {
                if (i < BotInfoHandler.gameHandler.opponents[i]) msg += $"{BotInfoHandler.gameHandler.players[i].name} vs {BotInfoHandler.gameHandler.players[BotInfoHandler.gameHandler.opponents[i]].name}\n";
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
        [RequireIngame]
        public async Task PairPlayers(CommandContext ctx, int pl1, int pl2)
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
        [RequireIngame]
        [RequireGuild]
        public async Task Fight(CommandContext ctx, int pl1, int pl2)
        {
            if (pl1 < 1 || pl1 > BotInfoHandler.participantsDiscordIds.Count()) return;
            if (pl2 < 1 || pl2 > BotInfoHandler.participantsDiscordIds.Count()) return;
            if (pl1 == pl2) return;
            pl1--;
            pl2--;

            Console.WriteLine(1);
            GameHandlerMethods.StartBattle(ref BotInfoHandler.gameHandler, pl1, pl2);
            Console.WriteLine(2);
            var fightMessage = new DiscordEmbedBuilder{
                Title = "Combat!",
                Color = DiscordColor.Gold
            };
            Console.WriteLine(3);
            string msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.introductionHeader[i] + "\n";
            }
            Console.WriteLine(4);
            fightMessage.AddField("Introduction Header", msg);
            Console.WriteLine(5);
            msg = string.Empty;
            Console.WriteLine(6);
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.preCombatHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.preCombatHeader[i] + "\n";
            }
            Console.WriteLine(7);
            fightMessage.AddField("Pre-Combat Header", msg);
            Console.WriteLine(8);
            msg = string.Empty;
            for (int i = 0; i < BotInfoHandler.gameHandler.combatOutputCollector.combatHeader.Count(); i++)
            {
                msg = msg + BotInfoHandler.gameHandler.combatOutputCollector.combatHeader[i] + "\n";
            }
            fightMessage.AddField("Combat Header", msg);            
            Console.WriteLine(9);
            await ctx.RespondAsync(embed: fightMessage).ConfigureAwait(false);
        }
    }
}
