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

        public string GetHandInfo()
        {
            if (this.cards.Count() == 0) return "Your hand is empty.";
            string ret = string.Empty;
            for (int i=0; i<this.cards.Count(); i++)
            {
                ret += $"{i+1}) " + this.cards[i].GetInfo();
                if (i != this.cards.Count() - 1) ret += '\n';
            }
            return ret;
        }
    }
}
