using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using ScrapScramble.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.BotRelated
{
    class BotInfoHandler
    {
        public static GameHandler gameHandler = new GameHandler();
        public static List<ulong> participantsDiscordIds = new List<ulong>();
        public static bool inGame = false;
        public static bool shopsSent = false;
        public static int currentRound = 1;
        public static List<DiscordMessage> UIMessages = new List<DiscordMessage>();

        public BotInfoHandler()
        {

        }

        public static void AddPlayer(CommandContext ctx, string name)
        {
            BotInfoHandler.participantsDiscordIds.Add(ctx.User.Id);
            BotInfoHandler.gameHandler.AddMech(name);
            BotInfoHandler.UIMessages.Add(null);
        }
        public static async Task SendNewUI(CommandContext ctx, int index)
        {
            if (UIMessages[index] != null) { await UIMessages[index].DeleteAsync().ConfigureAwait(false); UIMessages[index] = null; }

            if (!ctx.Channel.IsPrivate) 
            {
                var member = await ctx.Guild.GetMemberAsync(participantsDiscordIds[index]).ConfigureAwait(false);
                var userdm = await member.CreateDmChannelAsync().ConfigureAwait(false);
                UIMessages[index] = await userdm.SendMessageAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.Brown}).ConfigureAwait(false);
            }
            else
            {
                UIMessages[index] = await ctx.RespondAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.Brown}).ConfigureAwait(false);
            }

            await RefreshUI(ctx, index);
        }

        public static async Task RefreshUI(CommandContext ctx, int index)
        {
            if (UIMessages[index] == null)
            {
                await SendNewUI(ctx, index);
                return;
            }

            DiscordEmbedBuilder msg = new DiscordEmbedBuilder
            {
                Title = $"{gameHandler.players[index].name}'s Information",
                Color = DiscordColor.Brown,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Type >help to see what commands are available. Commands related to your mech can only be used in DMs." }
            };

            msg.AddField("[Mech Info]", gameHandler.players[index].PrintInfo(ref BotInfoHandler.gameHandler));
            msg.AddField($"[Round {currentRound} Shop]", gameHandler.players[index].shop.GetShopInfo());
            msg.AddField("[Your Hand]", gameHandler.players[index].hand.GetHandInfo());

            DiscordEmbed embedMsg = msg;            
            await UIMessages[index].ModifyAsync(embed: embedMsg).ConfigureAwait(false);  
        }
    }
}
