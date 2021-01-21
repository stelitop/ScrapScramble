using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public abstract class Card
    {
        public string name;
        public string cardText;

        public bool inLimbo = false;

        public abstract bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy);
        public abstract string GetInfo(ref GameHandler gameHandler, int player);
        public abstract Card DeepCopy();

        public virtual void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy) { }
    }
}
