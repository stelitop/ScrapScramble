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
            this.cardText = this.writtenEffect = "Start of Combat: If your Mech's Attack is equal to its Health, gain +2/+2.";
            this.creatureData = new CreatureData(4, 4, 4);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.attack == gameHandler.players[curPlayer].creatureData.health)
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
                gameHandler.players[curPlayer].creatureData.health += 2;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Tighrope Champion triggers and gives it +2/+2, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
            }   
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Tighrope Champion failed to trigger.");
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
            this.cardText = this.writtenEffect = "Start of Combat: Gain Shields equal to the last digit of the enemy Mech's Attack.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[enemy].creatureData.attack%10;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Carbon Carapace gives it +{gameHandler.players[enemy].creatureData.attack % 10} Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
        }
    }

    [MechAttribute]
    public class CopperplatedPrince : Mech
    {
        public CopperplatedPrince()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Prince";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Health for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 3, 1);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Prince gives it +{2 * gameHandler.players[curPlayer].curMana} Health, leaving it with {gameHandler.players[curPlayer].creatureData.health} Health.");
        }
    }

    [MechAttribute]
    public class CopperplatedPrincess : Mech
    {
        public CopperplatedPrincess()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Princess";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Attack for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 1, 3);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Princess gives it +{2 * gameHandler.players[curPlayer].curMana} Attack, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [MechAttribute]
    public class HomingMissile : Mech
    {
        public HomingMissile()
        {
            this.rarity = Rarity.Rare;
            this.name = "Homing Missile";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.health -= 5;
            if (gameHandler.players[enemy].creatureData.health < 1) gameHandler.players[enemy].creatureData.health = 1;
            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Homing Missile reduced your Mech's Health to {gameHandler.players[enemy].creatureData.health}.");
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

    [MechAttribute]
    public class OffbrandShoe : Mech
    {
        public OffbrandShoe()
        {
            this.rarity = Rarity.Rare;
            this.name = "Offbrand Shoe";
            this.cardText = this.writtenEffect = "Aftermath: Deal 6 damage to your Mech.";
            this.creatureData = new CreatureData(1, 0, 6);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health -= 6;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Offbrand Shoe deals 6 damage to you.");
        }
    }

    [MechAttribute]
    public class Hypnodrone : Mech
    {
        public Hypnodrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Hypnodrone";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Attack of your opponent's Mech by 5 (but not below 1).";
            this.creatureData = new CreatureData(7, 6, 6);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.attack -= 5;
            if (gameHandler.players[enemy].creatureData.attack < 1) gameHandler.players[enemy].creatureData.attack = 1;
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Hypnodrone reduced your Mech's Attack by 5, leaving it with {gameHandler.players[enemy].creatureData.attack} Attack.");
        }
    }

    [MechAttribute]
    public class MkIVSuperCobra : Mech
    {
        public MkIVSuperCobra()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mk. IV Super Cobra";
            this.cardText = "Rush. Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.writtenEffect = "Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.creatureData = new CreatureData(6, 5, 2);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            if (gameHandler.players[enemy].shop.options.Count() == 0) return;
            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[enemy].shop.options.Count());
            gameHandler.players[enemy].shop.options.RemoveAt(shop);
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Mk. IV Super Cobra destroyed a random upgarde in your shop.");
        }
    }

    [MechAttribute]
    public class LivewireBramble : Mech
    {
        public LivewireBramble()
        {
            this.rarity = Rarity.Rare;
            this.name = "Livewire Bramble";
            this.cardText = this.writtenEffect = "Aftermath: Replace two random Upgrades in your shop with Livewire Brambles.";
            this.creatureData = new CreatureData(0, 2, 1);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() <= 2)
            {
                for (int i=0; i<gameHandler.players[curPlayer].shop.options.Count(); i++)
                {
                    gameHandler.players[curPlayer].shop.options[i] = new LivewireBramble();
                }
            }
            else
            {
                int pos1, pos2;
                pos1 = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count());
                pos2 = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count()-1);
                if (pos2 >= pos1) pos2++;

                gameHandler.players[curPlayer].shop.options[pos1] = new LivewireBramble();
                gameHandler.players[curPlayer].shop.options[pos2] = new LivewireBramble();
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Livewire Bramble replace two Upgrades in your shop with Livewire Brambles.");
        }
    }

    [MechAttribute]
    public class PeekABot : Mech
    {
        public PeekABot()
        {
            this.rarity = Rarity.Rare;
            this.name = "Peek-a-Bot";
            this.cardText = this.writtenEffect = "Aftermath: You are told the most expensive Upgrade in your opponent's shop.";
            this.creatureData = new CreatureData(1, 1, 1);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            List<int> highestCosts = new List<int>();
            int maxCost = -1;
            for (int i=0; i<gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (maxCost < gameHandler.players[enemy].shop.options[i].creatureData.cost) maxCost = gameHandler.players[enemy].shop.options[i].creatureData.cost;
            }

            for (int i=0; i<gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.options[i].creatureData.cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Peek-a-Bot tells you the most expensive Upgrade in your opponent's shop is {gameHandler.players[enemy].shop.options[pos].name}");
        }
    }

    [MechAttribute]
    public class LightningWeasel : Mech
    {
        public LightningWeasel()
        {
            this.rarity = Rarity.Rare;
            this.name = "Lightning Weasel";
            this.cardText = this.writtenEffect = "Aftermath: Replace the highest-Cost Upgrade in your opponent's shop with a Lightning Weasel.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            List<int> highestCosts = new List<int>();
            int maxCost = -1;
            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (maxCost < gameHandler.players[enemy].shop.options[i].creatureData.cost) maxCost = gameHandler.players[enemy].shop.options[i].creatureData.cost;
            }

            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.options[i].creatureData.cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[enemy].shop.options[pos] = new LightningWeasel();
            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Lightning Weasel replaced your highest-Cost Upgrade with a Lightning Weasel.");
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
