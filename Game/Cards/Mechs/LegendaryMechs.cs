using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    //[MechAttribute]
    public class CheapFillerLegendary : Mech
    {
        public CheapFillerLegendary()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Cheap Filler Legendary";
            this.cardText = "Binary. This card is for testing purposes only";
            this.creatureData = new CreatureData(5, 5, 5);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }
    }

    [MechAttribute]
    public class LadyInByte : Mech
    {
        public LadyInByte()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lady in Byte";
            this.cardText = this.writtenEffect = "Aftermath: Set your Mech's Attack equal to its Health.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Lady in Byte sets your Attack equal to Health, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}");
        }
    }
    
    [MechAttribute]
    public class Solartron3000 : Mech
    {
        private bool triggered;

        public Solartron3000()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Solartron 3000";
            this.cardText = "Battlecry: The next Upgrade you buy this turn has Binary.";
            this.writtenEffect = "The next Upgrade you buy this turn has Binary.";
            this.printEffectInCombat = false;
            this.creatureData = new CreatureData(4, 2, 2);
            this.triggered = false;
        }

        public override void OnBuyingAMech(Mech m, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (triggered == false)
            {
                triggered = true;
                this.writtenEffect = string.Empty;
                if (m.creatureData.staticKeywords[StaticKeyword.Binary] < 1) m.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            }
        }
    }
}

/*

[MechAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Legendary;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/