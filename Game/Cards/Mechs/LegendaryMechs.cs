using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    //[UpgradeAttribute]
    public class CheapFillerLegendary : Upgrade
    {
        public CheapFillerLegendary()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Cheap Filler Legendary";
            this.cardText = "Binary. This card is for testing purposes only";
            this.SetStats(5, 5, 5);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }
    }

    [UpgradeAttribute]
    public class LadyInByte : Upgrade
    {
        public LadyInByte()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lady in Byte";
            this.cardText = this.writtenEffect = "Aftermath: Set your Upgrade's Attack equal to its Health.";
            this.SetStats(6, 5, 5);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Lady in Byte sets your Attack equal to Health, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}");
        }
    }           
}

/*

[UpgradeAttribute]
public class NextMech : Upgrade
{
    public NextMech()
    {
        this.rarity = Rarity.Legendary;
        this.name = "";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

*/