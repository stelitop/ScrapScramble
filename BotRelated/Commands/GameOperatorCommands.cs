using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ScrapScramble.BotRelated.Attributes;
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
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }

        [Command("sendshops")]
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
            }
            else
            {
                BotInfoHandler.shopsSent = true;
                //shops not sent yet
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
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
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
    }
}
