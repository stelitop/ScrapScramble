using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public class Spell : Card
    {
        public SpellRarity rarity = SpellRarity.Spell;
        public int cost;

        public Spell()
        {
            this.cost = 0;
            this.name = string.Empty;
            this.cardText = string.Empty;
        }

        public override Card DeepCopy()
        {
            Spell ret = (Spell)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.cost = this.cost;
            return ret;
        }

        public override string GetInfo()
        {
            string ret = string.Empty;
            ret = $"{this.name} - {this.rarity} - {this.cost} - {this.cardText}";
            return ret;
        }

        public override bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].hand.cards.Count <= handPos) return false;
            if (this.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.cost;

            this.OnPlay(ref gameHandler, curPlayer, enemy);

            return true;
        }

        public virtual void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy) { }
    }
}
