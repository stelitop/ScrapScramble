using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.BotRelated.Commands
{
    [RequireRoles(RoleCheckMode.All, "Game Operator")]
    [Aliases("gamesettings")]
    [Group("gamesetting")]
    public class GameSettingsCommands : BaseCommandModule
    {
        [Command("startinglives")]
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

        [Command("manacap")]
        [Description("Sets the maximum mana reachable in a game.")]
        public async Task SetManaCap(CommandContext ctx, int mana)
        {
            if (mana < 0) mana = -1;

            BotInfoHandler.gameHandler.maxManaCap = mana;

            if (mana != -1)
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "Maximum Mana Cap Changed",
                    Description = $"The maximum mana cap has been changed to {mana}.",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
            else
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "Maximum Mana Cap Removed",
                    Description = $"The maximum mana cap has been removed.",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
        }

        [Command("shoprarity")]
        [Description("Sets the breakdown by rarities of what upgrades you get in shop each turn.")]
        public async Task SetShopRarity(CommandContext ctx, int c, int r, int e, int l)
        {
            BotInfoHandler.gameHandler.shopRarities.common = c;
            BotInfoHandler.gameHandler.shopRarities.rare = r;
            BotInfoHandler.gameHandler.shopRarities.epic = e;
            BotInfoHandler.gameHandler.shopRarities.legendary = l;

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = "Shop Rarity Breakdown Changed",
                Description = $"The new breakdown is {c}-{r}-{e}-{l}.",
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);
        }

        [Command("setsamount")]
        [Description("Sets the amount of sets that will be present in the next game")]
        public async Task SetSetAmount(CommandContext ctx, int amount)
        {
            if (amount < 0) amount = 0;
            else if (amount >= BotInfoHandler.gameHandler.setHandler.Sets.Count) amount = 0;
            BotInfoHandler.gameHandler.setsAmount = amount;

            if (amount == 0)
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "Number of Sets Changed",
                    Description = "All sets will be included in the next game.",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
            else
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "Number of Sets Changed",
                    Description = $"Only {amount} sets will be included in the next game.",
                    Color = DiscordColor.Green
                }).ConfigureAwait(false);
            }
        }
    }
}
