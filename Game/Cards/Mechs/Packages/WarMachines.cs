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
    public class ArmOfExotron : Mech
    {
        public ArmOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Arm of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes.";
            this.SetStats(2, 2, 1);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class LegOfExotron : Mech
    {
        public LegOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Leg of Exotron";
            this.cardText = "Battlecry: Gain +2 Shields.";
            this.SetStats(2, 1, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class MotherboardOfExotron : Mech
    {
        public MotherboardOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Motherboard of Exotron";
            this.cardText = "Tiebreaker. Overload: (1)";
            this.SetStats(2, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class WheelOfExotron : Mech
    {
        public WheelOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Wheel of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes and +2 Shields.";
            this.SetStats(2, 1, 1);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
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
    public class PacifisticRecruitomatic : Mech
    {
        public PacifisticRecruitomatic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Pacifistic Recruitomatic";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 random 0-Attack Upgrades to your shop.";
            this.SetStats(2, 0, 3);
        }

        private bool Criteria(Mech m)
        {
            if (m.creatureData.attack == 0) return true;
            return false;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 3; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Pacifistic Recruitomatic adds 3 random 0-Attack Upgrades to your shop.");
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

    [UpgradeAttribute]
    [Package(UpgradePackage.WarMachines)]
    public class ExotronTheForbidden : Mech
    {
        public ExotronTheForbidden()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Exotron the Forbidden";
            this.cardText = this.writtenEffect = "Start of Combat: If you've bought all 5 parts of Exotron this game, destroy the enemy Mech.";
            this.SetStats(15, 15, 15);
        }

        private bool Criteria(Card m)
        {
            if (m.name.Equals("Arm of Exotron")) return true;
            if (m.name.Equals("Leg of Exotron")) return true;
            if (m.name.Equals("Motherboard of Exotron")) return true;
            if (m.name.Equals("Wheel of Exotron")) return true;
            return false;
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4)
            {
                gameHandler.players[enemy].destroyed = true;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden triggers, destroying {gameHandler.players[enemy].name}.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden fails to trigger.");
            }
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret = base.GetInfo(gameHandler, player);

            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4) return ret += " *(Ready!)*";
            else if (arm + leg + mb + wheel == 0) return ret += " *(Missing: All)*";
            else
            {
                ret += " *(Missing:";

                if (arm == 0) ret += " Arm,";
                if (leg == 0) ret += " Leg,";
                if (mb == 0) ret += " Motherboard,";
                if (wheel == 0) ret += " Wheel,";

                ret = ret.Remove(ret.Length - 1, 1);
                ret += ")*";
            }

            return ret;
        }
    }
}
