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
    }
}
