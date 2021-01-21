using ScrapScramble.Game.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class Hand
    {
        public List<Card> cards;

        public Hand()
        {
            this.cards = new List<Card>();
        }

        public List<string> GetHandInfo(ref GameHandler gameHandler, int player)
        {
            List<string> retList = new List<string>();

            if (this.cards.Count() == 0)
            {
                retList.Add("Your hand is empty.");
                return retList;
            }

            string ret = string.Empty;
            for (int i = 0; i < this.cards.Count(); i++)
            {
                string newBit = $"{i + 1}) " + this.cards[i].GetInfo(ref gameHandler, player);
                //ret += $"{i+1}) " + this.cards[i].GetInfo();
                if (ret.Length + newBit.Length > 1020)
                {
                    retList.Add(ret);
                    ret = string.Empty;
                }

                ret += newBit;
                if (i != this.cards.Count() - 1) ret += '\n';
            }
            retList.Add(ret);
            return retList;
        }
    }
}
