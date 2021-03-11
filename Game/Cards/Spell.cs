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
            this.Cost = 0;
            this.name = string.Empty;
            this.cardText = string.Empty;
        }

        public override Card DeepCopy()
        {
            Spell ret = (Spell)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.Cost = this.Cost;
            return ret;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret;
            if (this.rarity == SpellRarity.Spare_Part) ret = $"{this.name} - Spare Part - {this.Cost} - {this.cardText}";
            else ret = $"{this.name} - {this.rarity} - {this.Cost} - {this.cardText}";
            return ret;
        }

        public override async Task<bool> PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            await this.OnPlay(gameHandler, curPlayer, enemy);

            for (int i = 0; i < gameHandler.players[curPlayer].attachedMechs.Count(); i++)
            {
                await gameHandler.players[curPlayer].attachedMechs[i].OnSpellCast(this, gameHandler, curPlayer, enemy);
            }
            foreach (var extraEffect in gameHandler.players[curPlayer].extraUpgradeEffects)
            {
                await extraEffect.OnSpellCast(this, gameHandler, curPlayer, enemy);
            }

            return true;
        }

        public virtual void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy) { }
    }
}
