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
        public static bool pairsReady = false;

        public static List<DiscordMessage> UIMessages = new List<DiscordMessage>();

        public static DiscordMessage interactivePlayerList = null;
        public static DiscordUser interactivePlayerListCaller = null;

        public BotInfoHandler()
        {

        }        
        
        public static void AddPlayer(CommandContext ctx, string name)
        {
            BotInfoHandler.participantsDiscordIds.Add(ctx.User.Id);
            BotInfoHandler.gameHandler.AddPlayer(name);
            BotInfoHandler.UIMessages.Add(null);
        }
        public static void RemovePlayer(int index)
        {
            BotInfoHandler.participantsDiscordIds.RemoveAt(index);
            BotInfoHandler.UIMessages.RemoveAt(index);
            BotInfoHandler.gameHandler.players.RemoveAt(index);
            BotInfoHandler.gameHandler.pairsHandler.RemovePlayer(index);
        }

        public static async Task SendNewUI(CommandContext ctx, int index)
        {
            //tbh I don't see a reason to delete the old msg
            //if (UIMessages[index] != null) { await UIMessages[index].DeleteAsync().ConfigureAwait(false); UIMessages[index] = null; }
            UIMessages[index] = null;

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
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Type >help to see what commands are available. Commands related to your mech can only be used in DMs. When you're done with your turn, type >ready." }
            };
            
            string aftermathMsg = gameHandler.players[index].GetAftermathMessages();            
            if (!aftermathMsg.Equals(string.Empty)) msg.AddField("[Aftermath]", aftermathMsg);

            msg.AddField("[Mech Info]", gameHandler.players[index].PrintInfoGeneral(ref BotInfoHandler.gameHandler));            
            msg.AddField("[Keywords]", gameHandler.players[index].PrintInfoKeywords(ref BotInfoHandler.gameHandler));
            msg.AddField("[Effects]", gameHandler.players[index].PrintInfoEffects(ref BotInfoHandler.gameHandler));
            msg.AddField("[Upgrades]", gameHandler.players[index].PrintInfoUpgrades(ref BotInfoHandler.gameHandler));

            List<string> shopValue = gameHandler.players[index].shop.GetShopInfo(ref gameHandler, index);

            for (int i=0; i<shopValue.Count(); i++)
            {
                msg.AddField($"[Round {BotInfoHandler.gameHandler.currentRound} Shop]", shopValue[i]);
            }

            List<string> handValue = gameHandler.players[index].hand.GetHandInfo(ref BotInfoHandler.gameHandler, index);

            for (int i=0; i<handValue.Count(); i++)
            {
                msg.AddField("[Your Hand]", handValue[i]);
            }

            await UIMessages[index].ModifyAsync(embed: msg.Build()).ConfigureAwait(false);  
        }

        public static async Task SendNewPlayerList(CommandContext ctx)
        {
            if (interactivePlayerList != null)
            {
                interactivePlayerList.ModifyAsync(embed: new DiscordEmbedBuilder{
                    Title = "Old Interactive List of Participants",
                    Description = "A new interactive list has been called and this one is not updated.",
                    Color = DiscordColor.Gray
                }.Build()).ConfigureAwait(false);
            }

            interactivePlayerList = await ctx.RespondAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.SpringGreen}).ConfigureAwait(false);
            interactivePlayerListCaller = ctx.User;

            await RefreshPlayerList(ctx);
        }

        public static async Task RefreshPlayerList(CommandContext ctx)
        {
            if (interactivePlayerList == null) return;

            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "Interactive List of Participants",
                Color = DiscordColor.SpringGreen,
                Description = string.Empty
            };

            if (BotInfoHandler.gameHandler.players.Count() == 0) responseMessage.Description = "Nobody has signed up yet.";

            int readyNum = 0;

            for (int i = 0; i < BotInfoHandler.gameHandler.players.Count(); i++)
            {
                DiscordUser user = await ctx.Client.GetUserAsync(BotInfoHandler.participantsDiscordIds[i]);
                if (BotInfoHandler.inGame)
                {
                    if (BotInfoHandler.gameHandler.players[i].lives <= 0)
                    {
                        readyNum++;
                        responseMessage.Description += ":skull: ";
                    }
                    else if (BotInfoHandler.gameHandler.players[i].ready)
                    {
                        readyNum++;
                        responseMessage.Description += ":green_square: ";
                    }
                    else responseMessage.Description += ":red_square: ";
                }
                responseMessage.Description += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} ({user.Username})";
                if (BotInfoHandler.inGame) responseMessage.Description += $" - Lives: {BotInfoHandler.gameHandler.players[i].lives}";

                if (i != BotInfoHandler.gameHandler.players.Count()-1) responseMessage.Description += '\n';
            }

            await interactivePlayerList.ModifyAsync(embed: responseMessage.Build()).ConfigureAwait(false);

            if (readyNum >= BotInfoHandler.gameHandler.players.Count())
            {
                await interactivePlayerList.Channel.SendMessageAsync($"Hey {interactivePlayerListCaller.Mention}, all players are ready!").ConfigureAwait(false);
            }
        }
    }
}
