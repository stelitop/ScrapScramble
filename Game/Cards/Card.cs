using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public class Card
    { 
        public virtual bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy) { return false; }
        public virtual bool BuyCard(int shopPos, ref GameHandler gameHandler, int curPlayer, int enemy) { return false; }
        public virtual string GetInfo() { return string.Empty; }
    }
}
