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
        //public async Task MechInfo(CommandContext ctx)
        //{
        //    int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

        //    DiscordEmbedBuilder responseMessage = new DiscordEmbedBuilder
        //    {
        //        Title = "Your Upgrade's Data",
        //        Description = BotInfoHandler.gameHandler.players[index].PrintInfo(BotInfoHandler.gameHandler),
        //        Color = DiscordColor.Brown
        //    };

        //    await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        //}

        [Command("shop")]
        public async Task ResentShop(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            var shopEmbed = new DiscordEmbedBuilder
            {
                Title = $"Round {BotInfoHandler.gameHandler.currentRound} Shop",               
                Color = DiscordColor.Azure
            };
            List<string> shopList = BotInfoHandler.gameHandler.players[index].shop.GetShopInfo(BotInfoHandler.gameHandler, index);
            for (int i=0; i<shopList.Count(); i++)
            {
                shopEmbed.AddField("Shop", shopList[i]);
            }

            await ctx.RespondAsync(embed: shopEmbed).ConfigureAwait(false);
        }

        [Command("buy")]
        [Description("Buys an Upgrade in your shop and attaches it to your Upgrade.")]
        public async Task BuyUpgrade(CommandContext ctx, [Description("Index of the Upgrade in your shop")]int shopPos)
        {
            shopPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            BotInfoHandler.gameHandler.players[index].ctx = ctx;

            if (shopPos >= BotInfoHandler.gameHandler.players[index].shop.LastIndex || shopPos < 0)
            {
                //invalid shop position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!await BotInfoHandler.gameHandler.players[index].BuyCard(shopPos, BotInfoHandler.gameHandler, index, BotInfoHandler.gameHandler.pairsHandler.opponents[index]))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                BotInfoHandler.gameHandler.players[index].ready = false;
                BotInfoHandler.RefreshPlayerList(ctx);

                await BotInfoHandler.RefreshUI(ctx, index);
            }            
        }

        [Command("play")]
        [Description("Plays an Upgrade from your hand and attaches it to your Upgrade.")]
        public async Task PlayCard(CommandContext ctx, [Description("Index of the Upgrade in your hand")]int handPos)
        {
            handPos--;
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            BotInfoHandler.gameHandler.players[index].ctx = ctx;

            if (handPos >= BotInfoHandler.gameHandler.players[index].hand.LastIndex || handPos < 0)
            {
                //invalid hand position
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else if (!(await BotInfoHandler.gameHandler.players[index].PlayCard(handPos, BotInfoHandler.gameHandler, index, BotInfoHandler.gameHandler.pairsHandler.opponents[index])))
            {
                //upgrade is too expensive
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:")).ConfigureAwait(false);
            }
            else
            {
                //valid pos
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":+1:")).ConfigureAwait(false);

                BotInfoHandler.gameHandler.players[index].ready = false;
                BotInfoHandler.RefreshPlayerList(ctx);

                await BotInfoHandler.RefreshUI(ctx, index);
            }
        }

        [Command("refreshui")]
        [Description("Sends a new UI that displays your Upgrade's information and deletes the old one (if possible).")]
        public async Task NewUI(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            await BotInfoHandler.SendNewUI(ctx, index);
        }

        [Command("ready")]
        [Description("Indicates that you're ready with your play this round.")]
        public async Task ReadyRound(CommandContext ctx)
        {
            int index = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

            DiscordEmbedBuilder responseMessage;

            if (BotInfoHandler.gameHandler.players[index].ready)
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Already Readied",
                    Color = DiscordColor.Red
                };
            }
            else
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Have Readied Successfully",
                    Description = "You can still make changes to your Upgrade but you will need to use >ready again.",
                    Color = DiscordColor.Green
                };
                BotInfoHandler.gameHandler.players[index].ready = true;
                BotInfoHandler.RefreshPlayerList(ctx);
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }
    }
}
