using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [MechAttribute]
    public class GoldBolts : Mech
    {
        public GoldBolts()
        {
            this.rarity = Rarity.Rare;
            this.name = "Gold Bolts";
            this.cardText = "Battlecry: Transform your Shields into Health.";
            this.creatureData = new CreatureData(3, 1, 1);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] = 0;
        }
    }

    [MechAttribute]
    public class WupallSmasher : Mech
    {
        public WupallSmasher()
        {
            this.rarity = Rarity.Rare;
            this.name = "Wupall Smasher";
            this.cardText = "Battlecry: Transform your Spikes into Attack.";
            this.creatureData = new CreatureData(5, 3, 3);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] = 0;
        }
    }

    [MechAttribute]
    public class TightropeChampion : Mech
    {
        public TightropeChampion()
        {
            this.rarity = Rarity.Rare;
            this.name = "Tightrope Champion";
            this.cardText = "Start of Combat: If your Mech's Attack is equal to its Health, gain +2/+2.";
            this.creatureData = new CreatureData(4, 4, 4);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.attack == gameHandler.players[curPlayer].creatureData.health)
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
                gameHandler.players[curPlayer].creatureData.health += 2;
            }   
        }
    }

    [MechAttribute]
    public class CarbonCarapace : Mech
    {
        public CarbonCarapace()
        {
            this.rarity = Rarity.Rare;
            this.name = "Carbon Carapace";
            this.cardText = "Start of Combat: Gain Shields equal to the last digit of the enemy Mech's Attack.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[enemy].creatureData.attack%10;            
        }
    }

    [MechAttribute]
    public class CopperplatedPrince : Mech
    {
        public CopperplatedPrince()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Prince";
            this.cardText = "Start of Combat: Gain +2 Health for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 3, 1);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2 * gameHandler.players[curPlayer].curMana;
        }
    }

    [MechAttribute]
    public class CopperplatedPrincess : Mech
    {
        public CopperplatedPrincess()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Princess";
            this.cardText = "Start of Combat: Gain +2 Attack for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 1, 3);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2 * gameHandler.players[curPlayer].curMana;
        }
    }

    [MechAttribute]
    public class HomingMissile : Mech
    {
        public HomingMissile()
        {
            this.rarity = Rarity.Rare;
            this.name = "Homing Missile";
            this.cardText = "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].creatureData.health -= 5;
            if (gameHandler.players[enemy].creatureData.health < 1) gameHandler.players[enemy].creatureData.health = 1;
        }
    }

    [MechAttribute]
    public class TwilightDrone : Mech
    {
        public TwilightDrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Twilight Drone";
            this.cardText = "Battlecry: Give your Mech +1/+1 for each card in your hand.";
            this.creatureData = new CreatureData(4, 2, 4);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].hand.cards.Count();
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].hand.cards.Count();
        }
    }
}

/*

[MechAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Rare;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/
