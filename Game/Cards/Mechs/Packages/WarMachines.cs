using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class HelicopterBlades : Mech
    {
        public HelicopterBlades()
        {
            this.rarity = Rarity.Common;
            this.name = "Helicopter Blades";
            this.cardText = "Rush. Battlecry: Gain +4 Spikes. Overload: (3)";
            this.SetStats(5, 4, 3);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class SixpistolConstable : Mech
    {
        public SixpistolConstable()
        {
            this.rarity = Rarity.Common;
            this.name = "Sixpistol Constable";
            this.cardText = "Rush x6";
            this.SetStats(15, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 6;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class CopperplatedPrince : Mech
    {
        public CopperplatedPrince()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Prince";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Health for each unspent Mana you have.";
            this.SetStats(3, 3, 1);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Prince gives it +{2 * gameHandler.players[curPlayer].curMana} Health, leaving it with {gameHandler.players[curPlayer].creatureData.health} Health.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class CopperplatedPrincess : Mech
    {
        public CopperplatedPrincess()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Princess";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Attack for each unspent Mana you have.";
            this.SetStats(3, 1, 3);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Princess gives it +{2 * gameHandler.players[curPlayer].curMana} Attack, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class HomingMissile : Mech
    {
        public HomingMissile()
        {
            this.rarity = Rarity.Rare;
            this.name = "Homing Missile";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).";
            this.SetStats(4, 3, 3);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.health -= 5;
            if (gameHandler.players[enemy].creatureData.health < 1) gameHandler.players[enemy].creatureData.health = 1;
            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Homing Missile reduced your Mech's Health to {gameHandler.players[enemy].creatureData.health}.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class Hypnodrone : Mech
    {
        public Hypnodrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Hypnodrone";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Attack of your opponent's Mech by 5 (but not below 1).";
            this.SetStats(7, 6, 6);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.attack -= 5;
            if (gameHandler.players[enemy].creatureData.attack < 1) gameHandler.players[enemy].creatureData.attack = 1;
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Hypnodrone reduced your Mech's Attack by 5, leaving it with {gameHandler.players[enemy].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class CopperCommander : Mech
    {
        public CopperCommander()
        {
            this.rarity = Rarity.Epic;
            this.name = "Copper Commander";
            this.cardText = this.writtenEffect = "Your Start of Combat effects trigger twice.";
            this.SetStats(4, 3, 3);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.multiplierStartOfCombat = 2;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class DeflectOShield : Mech
    {
        public DeflectOShield()
        {
            this.rarity = Rarity.Epic;
            this.name = "Deflect-o-Shield";
            this.cardText = this.writtenEffect = "Prevent any damage to your Mech that would deal 2 or less damage.";
            this.SetStats(4, 2, 2);
        }

        public override void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg)
        {
            if (1 <= damage && damage <= 2)
            {
                damage = 0;
                msg += "prevented by Deflect-o-Shield, ";
            }
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class PanicButton : Mech
    {
        private bool triggered;
        public PanicButton()
        {
            this.rarity = Rarity.Epic;
            this.name = "Panic Button";
            this.cardText = this.writtenEffect = "After your Mech is reduced to 5 or less Health, deal 10 damage to the enemy Mech.";
            this.SetStats(5, 3, 3);
            this.triggered = false;
        }

        public override void AfterThisTakesDamage(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!this.triggered && gameHandler.players[curPlayer].creatureData.health <= 5)
            {
                this.triggered = true;
                gameHandler.players[enemy].TakeDamage(10, gameHandler, curPlayer, enemy,
                    $"{gameHandler.players[curPlayer].name}'s Panic Button triggers, dealing 10 damage, ");
            }
        }
    }
}
