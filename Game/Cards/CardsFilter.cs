using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public class CardsFilter
    {
        public delegate bool Criteria<T>(T card);

        public static List<T> FilterList<T>(List<T> cards, Criteria<T> criteria) 
        {
            List<T> ret = new List<T>();
            for (int i=0; i<cards.Count(); i++)
            {
                if (criteria(cards[i]))
                {
                    ret.Add(cards[i]);
                }
            }
            return ret;
        }

        public static List<T> FilterList<T>(List<List<T> > cards, Criteria<T> criteria)
        {
            List<T> ret = new List<T>();
            for (int i = 0; i < cards.Count(); i++)
            {
                for (int j = 0; j < cards[i].Count(); j++)
                {
                    if (criteria(cards[i][j]))
                    {
                        ret.Add(cards[i][j]);
                    }
                }
            }
            return ret;
        }
    }
}
