using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ScrapScramble.BotRelated;
using ScrapScramble.Game.Effects;
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

            BotInfoHandler.RefreshUI(BotInfoHandler.gameHandler.players[index].ctx, index);

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

        public static int ChooseUpgradeInShop(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return -1;

            PlayerInteraction chooseUpgrade = new PlayerInteraction("Choose an Upgrade in your shop.", string.Empty, "Write the corresponding index", AnswerType.IntAnswer);
            string res;
            bool show = true;
            while (true)
            {
                res = chooseUpgrade.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    show = true;
                    continue;
                }
                else
                {
                    int shopIndex = int.Parse(res) - 1;
                    if (0 <= shopIndex && shopIndex < gameHandler.players[curPlayer].shop.totalSize)
                    {
                        if (gameHandler.players[curPlayer].shop.At(shopIndex).inLimbo) continue;
                        if (gameHandler.players[curPlayer].shop.At(shopIndex).name == BlankUpgrade.name) continue;

                        return shopIndex;
                    }
                    else continue;
                }
            }
        }

        public static int FreezeUpgradeInShop(GameHandler gameHandler, int curPlayer, int enemy, int freezeAmount = 1)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return -1;

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
                    show = true;
                    continue;
                }
                else
                {
                    int shopIndex = int.Parse(res) - 1;
                    if (0 <= shopIndex && shopIndex < gameHandler.players[curPlayer].shop.totalSize)
                    {
                        if (gameHandler.players[curPlayer].shop.At(shopIndex).inLimbo) continue;
                        if (gameHandler.players[curPlayer].shop.At(shopIndex).name == BlankUpgrade.name) continue;

                        gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.staticKeywords[StaticKeyword.Freeze] =
                            Math.Max(freezeAmount, gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.staticKeywords[StaticKeyword.Freeze]);

                        gameHandler.players[curPlayer].shop.At(shopIndex).OnBeingFrozen(gameHandler, curPlayer, enemy);

                        return shopIndex;
                    }
                    else continue;                    
                }
            }
        }

        public static int ActivateMagnetic(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction magnetic = new PlayerInteraction("Write the number of a Spare Part", string.Empty, "Write the corresponding index", AnswerType.IntAnswer);
            
            for (int i=0; i<gameHandler.pool.spareparts.Count(); i++)
            {
                magnetic.description += $"{i+1}) {gameHandler.pool.spareparts[i].GetInfo(gameHandler, curPlayer)}";
                if (i != gameHandler.pool.spareparts.Count() - 1) magnetic.description += "\n";
            }

            string res;
            bool show = true;
            while (true)
            {
                res = magnetic.SendInteractionAsync(curPlayer, show).Result;
                show = false;

                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    show = true;
                    continue;
                }
                else
                {
                    int index = int.Parse(res) - 1;
                    if (0 <= index && index < gameHandler.pool.spareparts.Count())
                    {
                        gameHandler.players[curPlayer].hand.AddCard(gameHandler.pool.spareparts[index]);                        
                        return index;
                    }
                    else continue;
                }
            }
        }
    }
}
