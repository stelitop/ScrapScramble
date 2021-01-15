using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public class CardsFilter
    {
        public delegate bool Criteria<T>(T card) where T: Card;

        public static List<T> FilterList<T>(ref List<T> cards, Criteria<T> criteria) where T: Card
        {
            List<T> ret = new List<T>();
            for (int i=0; i<cards.Count(); i++)
            {
                if (criteria(cards[i]))
                {
                    ret.Add((T)cards[i].DeepCopy());
                }
            }
            return ret;
        }

        public static List<T> FilterList<T>(ref List<List<T> > cards, Criteria<T> criteria) where T : Card
        {
            List<T> ret = new List<T>();
            for (int i = 0; i < cards.Count(); i++)
            {
                for (int j = 0; j < cards[i].Count(); j++)
                {
                    if (criteria(cards[i][j]))
                    {
                        ret.Add((T)cards[i][j].DeepCopy());
                    }
                }
            }
            return ret;
        }
    }
}
