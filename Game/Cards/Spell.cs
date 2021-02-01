using ScrapScramble.Game.Effects;
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

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret = string.Empty;
            if (this.rarity == SpellRarity.Spare_Part) ret = $"{this.name} - Spare Part - {this.cost} - {this.cardText}";
            else ret = $"{this.name} - {this.rarity} - {this.cost} - {this.cardText}";
            return ret;
        }

        public override bool PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].hand.totalSize <= handPos) return false;
            if (gameHandler.players[curPlayer].hand.At(handPos).name == BlankUpgrade.name) return false;
            if (this.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.cost;

            for (int i = 0; i < gameHandler.players[curPlayer].attachedMechs.Count(); i++)
            {
                gameHandler.players[curPlayer].attachedMechs[i].OnSpellCast(this, gameHandler, curPlayer, enemy);
            }

            this.OnPlay(gameHandler, curPlayer, enemy);

            return true;
        }

        public virtual void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy) { }
    }
}
