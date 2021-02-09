using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    //[UpgradeAttribute]
    public class CheapFillerLegendary : Mech
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
    public class LadyInByte : Mech
    {
        public LadyInByte()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lady in Byte";
            this.cardText = this.writtenEffect = "Aftermath: Set your Mech's Attack equal to its Health.";
            this.SetStats(6, 5, 5);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Lady in Byte sets your Attack equal to Health, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}");
        }
    }           

    [TokenAttribute]
    public class AbsoluteZero : Spell
    {
        public AbsoluteZero()
        {
            this.rarity = SpellRarity.Spell;
            this.name = "Absolute Zero";
            this.cardText = "Freeze an Upgrade. Return this to your hand.";
            this.cost = 1;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy);
            gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());
        }
    }

    [UpgradeAttribute]
    public class CelsiorX : Mech
    {
        public CelsiorX()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Celsior X";
            this.cardText = "Battlecry: Add a 1-Cost Absolute Zero to your hand. It Freezes an Upgrade and can be played any number of times.";
            this.SetStats(2, 2, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());            
        }
    }
}

/*

[UpgradeAttribute]
public class NextMech : Mech
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