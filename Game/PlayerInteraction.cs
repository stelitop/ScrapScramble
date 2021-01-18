using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ScrapScramble.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public enum AnswerType
    {
        VoidAnswer,
        IntAnswer,
        CharAnswer,
        StringAnswer
    }

    public class PlayerInteraction
    {
        public string title;
        public string description;
        public string footer;
        public AnswerType answerType;

        public PlayerInteraction()
        {
            this.title = string.Empty;
            this.description = string.Empty;
            this.answerType = AnswerType.VoidAnswer;
            this.footer = string.Empty;
        }
        public PlayerInteraction(string title, string description, string footer, AnswerType answerType)
        {
            this.title = title;
            this.description = description;
            this.answerType = answerType;
            this.footer = footer;
        }

        public async Task<string> SendInteractionAsync(int index, bool showEmbed = true)
        {
            if (index < 0 || index >= BotInfoHandler.gameHandler.players.Count()) return string.Empty;            

            var interactivity = BotInfoHandler.gameHandler.players[index].ctx.Client.GetInteractivity();

            if (showEmbed) await BotInfoHandler.gameHandler.players[index].ctx.RespondAsync(embed: new DiscordEmbedBuilder
            {
                Title = this.title,
                Description = this.description,
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = this.footer }
            }).ConfigureAwait(false);

            var result = await interactivity.WaitForMessageAsync(x => x.Channel == BotInfoHandler.gameHandler.players[index].ctx.Channel && 
                                                                 x.Author == BotInfoHandler.gameHandler.players[index].ctx.User).ConfigureAwait(false);            

            if (result.TimedOut) return "TimedOut";

            string msg = result.Result.Content;
            int dummyint = 0;

            switch (this.answerType)
            {
                case AnswerType.IntAnswer:
                    if (int.TryParse(msg, out dummyint)) return msg;
                    else return string.Empty;

                case AnswerType.CharAnswer:
                    if (msg.Length == 1) return msg;
                    else return string.Empty;

                case AnswerType.StringAnswer:
                    if (msg == "TimedOut") return string.Empty;
                    return msg;

                default:
                    return string.Empty;
            }
        }

        public static int FreezeUpgradeInShop(ref GameHandler gameHandler, int curPlayer, int enemy, int freezeAmount = 1)
        {
            PlayerInteraction freeze = new PlayerInteraction("Choose an Upgrade in your shop to Freeze", string.Empty, "Write the corresponding index", AnswerType.IntAnswer);
            string res;
            bool show = true;
            while (true)
            {
                res = freeze.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    continue;
                }
                else
                {
                    int shopIndex = int.Parse(res) - 1;
                    if (0 <= shopIndex && shopIndex < gameHandler.players[curPlayer].shop.options.Count())
                    {
                        if (gameHandler.players[curPlayer].shop.options[shopIndex].inLimbo) continue;

                        gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.staticKeywords[StaticKeyword.Freeze] =
                            Math.Max(freezeAmount, gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.staticKeywords[StaticKeyword.Freeze]);

                        return shopIndex;
                    }
                    else continue;                    
                }
            }
        }
    }
}
