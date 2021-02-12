using ScrapScramble.Game.Cards;
using ScrapScramble.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class Hand
    {
        private List<Card> cards;
        public int totalSize { get { return cards.Count(); } }

        public Hand()
        {
            this.cards = new List<Card>();
        }

        public int AddCard(Card m)
        {
            this.cards.Add(m.DeepCopy());

            return this.cards.Count() - 1;
        }
        public Card At(int index)
        {
            if (index < 0 || index >= this.cards.Count()) return new BlankUpgrade();
            else if (this.cards[index].name == BlankUpgrade.name) return new BlankUpgrade();
            return this.cards[index];
        }
        public void RemoveCard(int index)
        {
            if (index < 0 || index >= this.cards.Count()) return;
            this.cards[index] = new BlankUpgrade();
            this.RemoveLeadingBlankUpgrades();
        }
        public void RemoveAllBlankUpgrades()
        {
            for (int i=cards.Count()-1; i>=0; i--)
            {
                if (cards[i].name == BlankUpgrade.name)
                {
                    cards.RemoveAt(i);
                }
            }
        }
        public int OptionsCount()
        {
            int ret = 0;
            for (int i = 0; i < this.cards.Count(); i++)
            {
                if (this.cards[i].name != BlankUpgrade.name) ret++;
            }
            return ret;
        }
        public List<Card> GetAllCards()
        {
            List<Card> ret = new List<Card>();
            for (int i = 0; i < cards.Count(); i++)
            {
                if (cards[i].name != BlankUpgrade.name) ret.Add(cards[i]);
            }
            return ret;
        }
        public List<int> GetAllCardIndexes()
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < cards.Count(); i++)
            {
                if (cards[i].name != BlankUpgrade.name) ret.Add(i);
            }
            return ret;
        }
        public void Clear()
        {
            this.cards.Clear();
        }
        private void RemoveLeadingBlankUpgrades()
        {
            for (int i = totalSize - 1; i >= 0; i--)
            {
                if (cards[i].name == BlankUpgrade.name)
                {
                    cards.RemoveAt(i);
                }
                else return;
            }
        }        

        public List<string> GetHandInfo(GameHandler gameHandler, int player)
        {
            List<string> retList = new List<string>();

            if (this.cards.Count() == 0)
            {
                retList.Add("Your hand is empty.");
                return retList;
            }

            string ret = string.Empty;
            bool lastBlank = false;

            for (int i = 0; i < this.cards.Count(); i++)
            {
                string newBit = $"{i + 1}) " + this.cards[i].GetInfo(gameHandler, player);
                if (this.At(i).name == BlankUpgrade.name) newBit = string.Empty;

                if (ret.Length + newBit.Length > 1020)
                {
                    retList.Add(ret);
                    ret = string.Empty;
                }

                ret += newBit;
                if (i != this.cards.Count() - 1 && !(lastBlank && newBit == string.Empty)) ret += '\n';

                lastBlank = (this.At(i).name == BlankUpgrade.name);
            }
            retList.Add(ret);
            return retList;
        }
    }
}
