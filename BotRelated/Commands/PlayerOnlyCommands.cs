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
        //[Command("mechinfo")]
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

        //[Command("shop")]
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
        [Description("Buys an Upgrade in your shop and attaches it to your Mech.")]
        public async Task BuyUpgrade(CommandContext ctx, [Description("Index of the Upgrade in your shop")]int shopPos)
        {
            shopPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            if (shopPos >= BotInfoHandler.gameHandler.players[index].shop.options.Count() || shopPos < 0)
            {
                //invalid shop position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!BotInfoHandler.gameHandler.players[index].BuyCard(shopPos, ref BotInfoHandler.gameHandler, index, BotInfoHandler.gameHandler.opponents[index]))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                BotInfoHandler.gameHandler.players[index].submitted = false;
                BotInfoHandler.RefreshPlayerList(ctx);

                await BotInfoHandler.RefreshUI(ctx, index);
            }            
        }

        [Command("play")]
        [Description("Plays an Upgrade from your hand and attaches it to your Mech.")]
        public async Task PlayCard(CommandContext ctx, [Description("Index of the Upgrade in your hand")]int handPos)
        {
            handPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            if (handPos >= BotInfoHandler.gameHandler.players[index].hand.cards.Count() || handPos < 0)
            {
                //invalid hand position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!BotInfoHandler.gameHandler.players[index].PlayCard(handPos, ref BotInfoHandler.gameHandler, index, BotInfoHandler.gameHandler.opponents[index]))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                BotInfoHandler.gameHandler.players[index].submitted = false;
                BotInfoHandler.RefreshPlayerList(ctx);

                await BotInfoHandler.RefreshUI(ctx, index);
            }
        }

        [Command("refreshui")]
        [Description("Sends a new UI that displays your Mech's information and deletes the old one (if possible).")]
        public async Task NewUI(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            await BotInfoHandler.SendNewUI(ctx, index);
        }

        [Command("submit")]
        [Description("Submits the things you've done this round.")]
        public async Task SubmitRound(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            DiscordEmbedBuilder responseMessage;

            if (BotInfoHandler.gameHandler.players[index].submitted)
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Already Submitted",
                    Color = DiscordColor.Red
                };
            }
            else
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Submitted Successfully",
                    Description = "You can still make changes to your Mech but you will need to resubmit.",
                    Color = DiscordColor.Green
                };
                BotInfoHandler.gameHandler.players[index].submitted = true;
                BotInfoHandler.RefreshPlayerList(ctx);
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }
    }
}
