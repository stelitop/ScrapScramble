using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public abstract class Card
    {
        public abstract bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy);
        public abstract bool BuyCard(int shopPos, ref GameHandler gameHandler, int curPlayer, int enemy);
        public abstract string GetInfo();
    }
}
