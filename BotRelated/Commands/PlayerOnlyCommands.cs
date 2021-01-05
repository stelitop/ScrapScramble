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
    [RequireIngame]
    [RequireBeingPlayer]
    [RequireDirectMessage]
    class PlayerOnlyCommands : BaseCommandModule
    {
        [Command("mechinfo")]
        public async Task MechInfo(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            DiscordEmbedBuilder responseMessage = new DiscordEmbedBuilder
            {
                Title = "Your Mech's Data",
                Description = BotInfoHandler.gameHandler.players[index].PrintInfo(ref BotInfoHandler.gameHandler),
                Color = DiscordColor.Brown
            };

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }
        [Command("shop")]
        public async Task ResentShop(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            var shopEmbed = new DiscordEmbedBuilder
            {
                Title = $"Round {BotInfoHandler.currentRound} Shop",
                Description = BotInfoHandler.gameHandler.players[index].shop.GetShopInfo(),
                Color = DiscordColor.Azure
            };

            await ctx.RespondAsync(embed: shopEmbed).ConfigureAwait(false);
        }
        [Command("buy")]
        public async Task BuyUpgrade(CommandContext ctx, int shopPos)
        {
            shopPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            if (shopPos >= BotInfoHandler.gameHandler.players[index].shop.options.Count() || shopPos < 0)
            {
                //invalid shop position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!BotInfoHandler.gameHandler.players[index].BuyCard(shopPos, ref BotInfoHandler.gameHandler, index, index))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);                
                
                await BotInfoHandler.RefreshUI(ctx, index);
            }            
        }

        [Command("play")]
        public async Task PlayCard(CommandContext ctx, int handPos)
        {
            handPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            if (handPos >= BotInfoHandler.gameHandler.players[index].shop.options.Count() || handPos < 0)
            {
                //invalid hand position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!BotInfoHandler.gameHandler.players[index].PlayCard(handPos, ref BotInfoHandler.gameHandler, index, index))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                await BotInfoHandler.RefreshUI(ctx, index);
            }
        }

        [Command("refreshui")]
        public async Task NewUI(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            await BotInfoHandler.SendNewUI(ctx, index);
        }
    }
}
