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
        public static DiscordMessage interactivePlayerList = null;

        public BotInfoHandler()
        {

        }        
        
        public static void AddPlayer(CommandContext ctx, string name)
        {
            BotInfoHandler.participantsDiscordIds.Add(ctx.User.Id);
            BotInfoHandler.gameHandler.AddMech(name);
            BotInfoHandler.UIMessages.Add(null);
        }
        public static void RemovePlayer(int index)
        {
            BotInfoHandler.participantsDiscordIds.RemoveAt(index);
            BotInfoHandler.UIMessages.RemoveAt(index);
            BotInfoHandler.gameHandler.players.RemoveAt(index);
            BotInfoHandler.gameHandler.opponents.RemoveAt(index);

            //move opponent indexes by 1
            for (int i=0; i<BotInfoHandler.gameHandler.opponents.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.opponents[i] == index) BotInfoHandler.gameHandler.opponents[i] = i;
                else if (BotInfoHandler.gameHandler.opponents[i] > index) BotInfoHandler.gameHandler.opponents[i]--;
            }
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
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Type >help to see what commands are available. Commands related to your mech can only be used in DMs. When you're done with your turn, type >submit." }
            };
            
            string aftermathMsg = gameHandler.players[index].GetAftermathMessages();            
            if (!aftermathMsg.Equals(string.Empty)) msg.AddField("[Aftermath]", aftermathMsg);            

            msg.AddField("[Mech Info]", gameHandler.players[index].PrintInfo(ref BotInfoHandler.gameHandler));            
            // BREAKS BECAUSE OF THE NEXT LINE
            msg.AddField($"[Round {currentRound} Shop]", gameHandler.players[index].shop.GetShopInfo());            
            msg.AddField("[Your Hand]", gameHandler.players[index].hand.GetHandInfo());            
         
            await UIMessages[index].ModifyAsync(embed: msg.Build()).ConfigureAwait(false);  
        }

        public static async Task SendNewPlayerList(CommandContext ctx)
        {
            interactivePlayerList = await ctx.RespondAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.Aquamarine}).ConfigureAwait(false);

            await RefreshPlayerList(ctx);
        }

        public static async Task RefreshPlayerList(CommandContext ctx)
        {
            if (interactivePlayerList == null) return;

            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "List of participants",
                Color = DiscordColor.Azure,
                Description = string.Empty
            };

            if (BotInfoHandler.gameHandler.players.Count() == 0) responseMessage.Description = "Nobody has signed up yet.";            

            for (int i = 0; i < BotInfoHandler.gameHandler.players.Count(); i++)
            {
                DiscordUser user = await ctx.Client.GetUserAsync(BotInfoHandler.participantsDiscordIds[i]);
                if (BotInfoHandler.inGame)
                {
                    if (BotInfoHandler.gameHandler.players[i].submitted) responseMessage.Description += ":green_square: ";
                    else responseMessage.Description += ":red_square: ";
                }
                responseMessage.Description += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} - {user.Username}";
                if (i != BotInfoHandler.gameHandler.players.Count()) responseMessage.Description += '\n';
            }

            await interactivePlayerList.ModifyAsync(embed: responseMessage.Build()).ConfigureAwait(false);
        }
    }
}
