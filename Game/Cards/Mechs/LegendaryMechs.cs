using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [MechAttribute]
    public class LadyInByte : Mech
    {
        public LadyInByte()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lady in Byte";
            this.cardText = "Aftermath: Set your Mech's Attack equal to its Health.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
        }
    }

    [MechAttribute]
    public class CheapFillerLegendary : Mech
    {
        public CheapFillerLegendary()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Cheap Filler Legendary";
            this.cardText = "Binary. Too lazy to write the buff (it exists tho dw). This card is for testing purposes only";
            this.creatureData = new CreatureData(5, 5, 5);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            for (int i=0; i<gameHandler.players[curPlayer].shop.options.Count(); i++)
            {
                gameHandler.players[curPlayer].shop.options[i].creatureData.attack++;
                gameHandler.players[curPlayer].shop.options[i].creatureData.health++;
            }
        }
    }
}
