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

        private int cost;
        public int Cost { get { return cost; } set { cost = Math.Max(0, value); } }

        public bool inLimbo = false;

        public List<Upgrade> extraUpgradeEffects = new List<Upgrade>();

        public abstract bool PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy);
        public abstract string GetInfo(GameHandler gameHandler, int player);
        public abstract Card DeepCopy();

        public virtual void OnPlay(GameHandler gameHandler, int curPlayer, int enemy) { }
    }
}
