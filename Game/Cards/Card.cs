using ScrapScramble.Game.Effects;
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

        public virtual async Task<bool> PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy) { return false; }
        public abstract string GetInfo(GameHandler gameHandler, int player);
        public abstract Card DeepCopy();

        public virtual async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy) { }

        public virtual bool CanBePlayed(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (handPos >= gameHandler.players[curPlayer].hand.LastIndex) return false;
            if (this.name == BlankUpgrade.name) return false;            
            if (this.inLimbo) return false;
            if (this.Cost > gameHandler.players[curPlayer].curMana) return false;

            return true;
        }
    }
}
