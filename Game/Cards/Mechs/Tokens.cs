using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [TokenAttribute]
    public class LightningBloom : Spell
    {
        public LightningBloom()
        {
            this.rarity = SpellRarity.Spell;
            this.name = "Lightning Bloom";
            this.cardText = "Gain 2 Mana this turn only. Overload: (2).";
            this.cost = 0;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] += 2;
        }
    }
}
