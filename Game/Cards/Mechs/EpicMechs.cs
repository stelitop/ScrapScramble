using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class Naptron : Mech
    {
        public Naptron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Naptron";
            this.cardText = "Taunt x2. Aftermath: Give your Mech Rush x2.";
            this.writtenEffect = "Aftermath: Give your Mech Rush x2.";
            this.creatureData = new CreatureData(4, 1, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] += 2;
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Naptron gives you Rush x2.");
        }
    }

    [UpgradeAttribute]
    public class HighRoller : Mech
    {
        public HighRoller()
        {
            this.rarity = Rarity.Epic;
            this.name = "High Roller";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Cost of a random Upgrade in your shop by (4).";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[shop].creatureData.cost -= 4;
            if (gameHandler.players[curPlayer].shop.options[shop].creatureData.cost < 0) gameHandler.players[curPlayer].shop.options[shop].creatureData.cost = 0;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Highroller reduces the cost of {gameHandler.players[curPlayer].shop.options[shop].name} in your shop by (4).");
        }
    }

    [UpgradeAttribute]
    public class FallenReaver : Mech
    {
        public FallenReaver()
        {
            this.rarity = Rarity.Epic;
            this.name = "Fallen Reaver";
            this.cardText = this.writtenEffect = "Aftermath: Destroy 6 random Upgrades in your shop.";
            this.creatureData = new CreatureData(5, 8, 8);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {

            for (int i=0; i<6; i++)
            {
                if (gameHandler.players[curPlayer].shop.options.Count() == 0) break;                

                int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
                gameHandler.players[curPlayer].shop.options.RemoveAt(shop);
            }
            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Fallen Reaver destroys 6 random Upgrades in your shop.");
        }
    }

    [UpgradeAttribute]
    public class Investrotron : Mech
    {
        public Investrotron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Investotron";
            this.cardText = this.writtenEffect = "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Mech +4/+4.";
            this.creatureData = new CreatureData(5, 4, 4);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[shop] = new Investrotron();
            gameHandler.players[curPlayer].creatureData.attack += 4;
            gameHandler.players[curPlayer].creatureData.health += 4;

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Investotron transforms a random Upgrade in your shop into an Investotron and gives you +4/+4, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
        }
    }

    [UpgradeAttribute]
    public class MassAccelerator : Mech
    {
        public MassAccelerator()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mass Accelerator";
            this.cardText = this.writtenEffect = "Start of Combat: If you're Overloaded, deal 5 damage to the enemy Mech.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 0 || gameHandler.players[curPlayer].overloaded > 0)
            {
                gameHandler.players[enemy].TakeDamage(5, ref gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Mass Accelerator triggers and deals 5 damage, ");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Mass Accelerator fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    public class PanicButton : Mech
    {
        private bool triggered;
        public PanicButton()
        {
            this.rarity = Rarity.Epic;
            this.name = "Panic Button";
            this.cardText = this.writtenEffect = "After your Mech is reduced to 5 or less Health, deal 10 damage to the enemy Mech.";
            this.creatureData = new CreatureData(5, 3, 3);
            this.triggered = false;
        }

        public override void AfterThisTakesDamage(int damage, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!this.triggered && gameHandler.players[curPlayer].creatureData.health <= 5)
            {
                this.triggered = true;
                gameHandler.players[enemy].TakeDamage(10, ref gameHandler, curPlayer, enemy,
                    $"{gameHandler.players[curPlayer].name}'s Panic Button triggers, dealing 10 damage, ");
            }
        }
    }

}

/*

[UpgradeAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Epic;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/