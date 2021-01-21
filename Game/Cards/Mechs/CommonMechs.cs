using ScrapScramble.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class HardPuncher : Mech
    {
        public HardPuncher()
        {
            this.rarity = Rarity.Common;
            this.name = "Hard Puncher";
            this.cardText = string.Empty;
            this.creatureData = new CreatureData(2, 3, 1);
        }
    }

    [UpgradeAttribute]
    public class SpringedSmilebot : Mech
    {
        public SpringedSmilebot()
        {
            this.rarity = Rarity.Common;
            this.name = "Springed Smilebot";
            this.cardText = "Rush";
            this.creatureData = new CreatureData(4, 3, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] += 1;
        }
    }

    [UpgradeAttribute]
    public class RunawayTire : Mech
    {
        public RunawayTire()
        {
            this.rarity = Rarity.Common;
            this.name = "Runaway Tire";
            this.cardText = "Rush x2";
            this.creatureData = new CreatureData(5, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] += 2;
        }
    }

    [UpgradeAttribute]
    public class MedievalSpikeball : Mech
    {
        public MedievalSpikeball()
        {
            this.rarity = Rarity.Common;
            this.name = "Medieval Tire";
            this.cardText = "Battlecry: Gain +4 Spikes and +4 Shields";
            this.creatureData = new CreatureData(7, 5, 5);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
        }
    }

    [UpgradeAttribute]
    public class HeavyDutyPlating : Mech
    {
        public HeavyDutyPlating()
        {
            this.rarity = Rarity.Common;
            this.name = "Heavy-Duty Plating";
            this.cardText = "Taunt x2";
            this.creatureData = new CreatureData(3, 5, 5);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] += 2;
        }
    }

    [UpgradeAttribute]
    public class RefurbishePlating : Mech
    {
        public RefurbishePlating()
        {
            this.rarity = Rarity.Common;
            this.name = "Refurbished Plating";
            this.cardText = "Battlecry: Gain Shields equal to twice your Taunt.";
            this.creatureData = new CreatureData(2, 0, 2);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt] * 2;
        }
    }

    [UpgradeAttribute]
    public class Healthbox : Mech
    {
        public Healthbox()
        {
            this.rarity = Rarity.Common;
            this.name = "Healthbox";
            this.cardText = this.writtenEffect = "Start of Combat: Give the enemy Mech +8 Health.";
            this.creatureData = new CreatureData(0, 0, 8);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].creatureData.health += 8;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Healthbox gives {gameHandler.players[enemy].name} +8 Health, leaving it with {gameHandler.players[enemy].creatureData.health} Health.");
        }
    }

    [UpgradeAttribute]
    public class Microchip : Mech
    {
        public Microchip()
        {
            this.rarity = Rarity.Common;
            this.name = "Microchip";
            this.cardText = "Overload: (2)";
            this.creatureData = new CreatureData(0, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
        }
    }

    [UpgradeAttribute]
    public class Oilmental : Mech
    {
        public Oilmental()
        {
            this.rarity = Rarity.Common;
            this.name = "Oilmental";
            this.cardText = "Aftermath: Give a random Upgrade in your shop +2/+2. Overload: (3)";
            this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +2/+2.";
            this.creatureData = new CreatureData(1, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {            
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[pos].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[pos].creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Oilmental gives the {gameHandler.players[curPlayer].shop.options[pos].name} in your shop +2/+2.");
        }
    }

    [UpgradeAttribute]
    public class RustedJunkwarden : Mech
    {
        public RustedJunkwarden()
        {
            this.rarity = Rarity.Common;
            this.name = "Rusted Junkwarden";
            this.cardText = "Taunt";
            this.creatureData = new CreatureData(1, 1, 3);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }
    }

    [UpgradeAttribute]
    public class SwindlersCoin : Mech
    {
        public SwindlersCoin()
        {
            this.rarity = Rarity.Common;
            this.name = "Swindler's Coin";
            this.cardText = "Binary, Tiebreaker. Overload: (1)";
            this.creatureData = new CreatureData(1, 0, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }
    }

    [UpgradeAttribute]
    public class ArmOfExotron : Mech
    {
        public ArmOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Arm of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes.";
            this.creatureData = new CreatureData(2, 2, 1);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
        }
    }

    [UpgradeAttribute]
    public class LegOfExotron : Mech
    {
        public LegOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Leg of Exotron";
            this.cardText = "Battlecry: Gain +2 Shields.";
            this.creatureData = new CreatureData(2, 1, 2);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }
    }

    [UpgradeAttribute]
    public class ChainMail : Mech
    {
        public ChainMail()
        {
            this.rarity = Rarity.Common;
            this.name = "Chain Mail";
            this.cardText = "Binary. Battlecry: Gain +2 Shields. Overload: (1)";
            this.creatureData = new CreatureData(2, 1, 2);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }
    }

    [UpgradeAttribute]
    public class TrafficCone : Mech
    {
        public TrafficCone()
        {
            this.rarity = Rarity.Common;
            this.name = "Traffic Cone";
            this.cardText = "Binary. Battlecry: Gain +2 Spikes. Overload: (1)";
            this.creatureData = new CreatureData(2, 2, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
        }
    }

    [UpgradeAttribute]
    public class MetalgillSnapper : Mech
    {
        public MetalgillSnapper()
        {
            this.rarity = Rarity.Common;
            this.name = "Metalgill Snapper";
            this.cardText = "Overload: (2)";
            this.creatureData = new CreatureData(2, 4, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
        }
    }

    [UpgradeAttribute]
    public class MotherboardOfExotron : Mech
    {
        public MotherboardOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Motherboard of Exotron";
            this.cardText = "Tiebreaker. Overload: (1)";
            this.creatureData = new CreatureData(2, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }
    }

    [UpgradeAttribute]
    public class ProtectiveFirewall : Mech
    {
        public ProtectiveFirewall()
        {
            this.rarity = Rarity.Common;
            this.name = "Protective Firewall";
            this.cardText = "Binary, Taunt";
            this.creatureData = new CreatureData(2, 2, 3);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }
    }

    [UpgradeAttribute]
    public class RivetedTrinked : Mech
    {
        public RivetedTrinked()
        {
            this.rarity = Rarity.Common;
            this.name = "Riveted Trinked";
            this.cardText = "Binary. Battlecry: Gain +2 Shields.";
            this.creatureData = new CreatureData(2, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }
    }

    [UpgradeAttribute]
    public class TankTreads : Mech
    {
        public TankTreads()
        {
            this.rarity = Rarity.Common;
            this.name = "Tank Treads";
            this.cardText = "Taunt. Battlecry: Gain +4 Shields. Overload: (3)";
            this.creatureData = new CreatureData(2, 3, 4);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
        }
    }

    [UpgradeAttribute]
    public class WheelOfExotron : Mech
    {
        public WheelOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Wheel of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes and +2 Shields.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }
    }

    [UpgradeAttribute]
    public class MalfunctioningPuncher : Mech
    {
        public MalfunctioningPuncher()
        {
            this.rarity = Rarity.Common;
            this.name = "Malfunctioning Puncher";
            this.cardText = "Start of Combat: Your Mech loses -4 Attack. Overload: (1)";
            this.writtenEffect = "Start of Combat: Your Mech loses -4 Attack.";
            this.creatureData = new CreatureData(4, 4, 8);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack -= 4;
            if (gameHandler.players[curPlayer].creatureData.attack < 1) gameHandler.players[curPlayer].creatureData.attack = 1;

            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Malfunctioning Puncher reduces its Attack by 4, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    public class HelicopterBlades : Mech
    {
        public HelicopterBlades()
        {
            this.rarity = Rarity.Common;
            this.name = "Helicopter Blades";
            this.cardText = "Rush. Battlecry: Gain +4 Spikes. Overload: (3)";
            this.creatureData = new CreatureData(5, 4, 3);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
        }
    }

    [UpgradeAttribute]
    public class ShieldbotClanker : Mech
    {
        public ShieldbotClanker()
        {
            this.rarity = Rarity.Common;
            this.name = "Shieldbot Clanker";
            this.cardText = "Battlecry and Aftermath: Gain +8 Shields.";
            this.writtenEffect = "Aftermath: Gain +8 Shields.";
            this.creatureData = new CreatureData(5, 2, 3);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Shieldbot Clanker gives you +8 Shields.");
        }
    }

    [UpgradeAttribute]
    public class SpikebotShanker : Mech
    {
        public SpikebotShanker()
        {
            this.rarity = Rarity.Common;
            this.name = "Spikebot Shanker";
            this.cardText = "Battlecry and Aftermath: Gain +8 Spikes.";
            this.writtenEffect = "Aftermath: Gain +8 Spikes.";
            this.creatureData = new CreatureData(5, 3, 2);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Spikebot Shanker gives you +8 Spikes.");
        }
    }

    [UpgradeAttribute]
    public class SpeedyProcessor : Mech
    {
        public SpeedyProcessor()
        {
            this.rarity = Rarity.Common;
            this.name = "Speedy Processor";
            this.cardText = "Binary, Rush";
            this.creatureData = new CreatureData(5, 3, 2);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }
    }

    [UpgradeAttribute]
    public class LeadHead : Mech
    {
        public LeadHead()
        {
            this.rarity = Rarity.Common;
            this.name = "Lead Head";
            this.cardText = "Taunt x2";
            this.creatureData = new CreatureData(6, 6, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
        }
    }

    [UpgradeAttribute]
    public class OneHitWonder : Mech
    {
        public OneHitWonder()
        {
            this.rarity = Rarity.Common;
            this.name = "One Hit Wonder";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +8 Attack.";
            this.creatureData = new CreatureData(7, 1, 5);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 8;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s One Hit Wonder gives it +8 Attack, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    public class Steamfunk : Mech
    {
        public Steamfunk()
        {
            this.rarity = Rarity.Common;
            this.name = "Steamfunk";
            this.cardText = this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +2/+2.";
            this.creatureData = new CreatureData(7, 7, 7);
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[pos].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[pos].creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Steamfunk gives the {gameHandler.players[curPlayer].shop.options[pos].name} in your shop +2/+2.");
        }
    }

    [UpgradeAttribute]
    public class PrismaticBarrier : Mech
    {
        public PrismaticBarrier()
        {
            this.rarity = Rarity.Common;
            this.name = "Prismatic Barrier";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +10 Shields.";
            this.creatureData = new CreatureData(8, 5, 6);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 10;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Prismatic Barrier gives it +10 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
        }
    }

    [UpgradeAttribute]
    public class SixpistolConstable : Mech
    {
        public SixpistolConstable()
        {
            this.rarity = Rarity.Common;
            this.name = "Sixpistol Constable";
            this.cardText = "Rush x6";
            this.creatureData = new CreatureData(15, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 6;
        }
    }

    [UpgradeAttribute]
    public class TwoHeadedColossus : Mech
    {
        public TwoHeadedColossus()
        {
            this.rarity = Rarity.Common;
            this.name = "Two-Headed Colossus";
            this.cardText = "Tiebreaker";
            this.creatureData = new CreatureData(11, 11, 11);
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
        }
    }

    [UpgradeAttribute]
    public class OrbitalMechanosphere : Mech
    {
        public OrbitalMechanosphere()
        {
            this.rarity = Rarity.Common;
            this.name = "Orbital Mechanosphere";
            this.cardText = string.Empty;
            this.creatureData = new CreatureData(30, 50, 50);            
        }
    }

    [UpgradeAttribute]
    public class BronzeBruiser : Mech
    {
        public BronzeBruiser()
        {
            this.rarity = Rarity.Common;
            this.name = "Bronze Bruiser";
            this.cardText = this.writtenEffect = "Aftermath: Add 4 random Common Upgrades to your shop.";
            this.creatureData = new CreatureData(2, 1, 2);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Common) return true;
            return false;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(ref gameHandler.pool.mechs, this.Criteria);

            for (int i=0; i<4; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.options.Add((Mech)list[pos].DeepCopy());
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Bronze Bruiser adds 4 random Common Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    public class SilverShogun : Mech
    {
        public SilverShogun()
        {
            this.rarity = Rarity.Common;
            this.name = "Silver Shogun";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 random Rare Upgrades to your shop.";
            this.creatureData = new CreatureData(3, 2, 3);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Rare) return true;
            return false;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(ref gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 3; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.options.Add((Mech)list[pos].DeepCopy());
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Silver Shogun adds 3 random Rare Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    public class GoldenGunner : Mech
    {
        public GoldenGunner()
        {
            this.rarity = Rarity.Common;
            this.name = "Golden Gunner";
            this.cardText = this.writtenEffect = "Aftermath: Add 2 random Epic Upgrades to your shop.";
            this.creatureData = new CreatureData(4, 3, 4);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Epic) return true;
            return false;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(ref gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 2; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.options.Add((Mech)list[pos].DeepCopy());
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Golden Gunner adds 2 random Epic Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    public class PlatinumParagon : Mech
    {
        public PlatinumParagon()
        {
            this.rarity = Rarity.Common;
            this.name = "Platinum Paragon";
            this.cardText = this.writtenEffect = "Aftermath: Add 1 random Legendary Upgrade to your shop.";
            this.creatureData = new CreatureData(5, 4, 5);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Legendary) return true;
            return false;
        }
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(ref gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 1; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.options.Add((Mech)list[pos].DeepCopy());
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Platinum Paragon adds 1 random Legendary Upgrade to your shop.");
        }
    }

    [UpgradeAttribute]
    public class ThreeFacedEmojitron : Mech
    {
        public ThreeFacedEmojitron()
        {
            this.rarity = Rarity.Common;
            this.name = "Three-Faced Emojitron";
            this.cardText = "Choose One - Gain Rush; or +2/+2.";
            this.creatureData = new CreatureData(5, 4, 2);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain Rush\n2) Gain +2/+2", "Write the corresponding number", AnswerType.IntAnswer);
            string res;
            bool show = true;
            while (true)
            {
                res = chooseOne.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    continue;
                }
                else
                {
                    if (int.Parse(res) == 1)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
                    }
                    else if (int.Parse(res) == 2)
                    {
                        gameHandler.players[curPlayer].creatureData.attack += 2;
                        gameHandler.players[curPlayer].creatureData.health += 2;
                    }
                    else continue;
                    break;
                }
            }
        }
    }

    [UpgradeAttribute]
    public class CorrodedBastion : Mech
    {
        public CorrodedBastion()
        {
            this.rarity = Rarity.Common;
            this.name = "Corroded Bastion";
            this.cardText = "Choose One - Gain Taunt; or Overload: (2).";
            this.creatureData = new CreatureData(2, 2, 4);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain Taunt\n2) Gain Overload: (2)", "Write the corresponding number", AnswerType.IntAnswer);
            string res;
            bool show = true;
            while (true)
            {
                res = chooseOne.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    continue;
                }
                else
                {
                    if (int.Parse(res) == 1)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt]++;
                    }
                    else if (int.Parse(res) == 2)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload]+=2;
                    }
                    else continue;
                    break;
                }
            }
        }
    }

    [UpgradeAttribute]
    public class SystemRebooter : Mech
    {
        public SystemRebooter()
        {
            this.rarity = Rarity.Common;
            this.name = "System Rebooter";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it Rush.";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(ref gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.staticKeywords[StaticKeyword.Rush]++;
            gameHandler.players[curPlayer].shop.options[shopIndex].cardText += " (Rush)";
        }
    }

    [UpgradeAttribute]
    public class BigFan : Mech
    {
        public BigFan()
        {
            this.rarity = Rarity.Common;
            this.name = "Big Fan";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it +3/+3 and Taunt.";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(ref gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.staticKeywords[StaticKeyword.Taunt]++;
            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.attack += 3;
            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.health += 3;
            gameHandler.players[curPlayer].shop.options[shopIndex].cardText += " (Taunt)";
        }
    }

    [UpgradeAttribute]
    public class SyntheticSnowball : Mech
    {
        public SyntheticSnowball()
        {
            this.rarity = Rarity.Common;
            this.name = "Synthetic Snowball";
            this.cardText = "Echo. Battlecry: Freeze an Upgrade. Give it +2/+2.";
            this.creatureData = new CreatureData(3, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(ref gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.health += 2;
        }
    }

    [UpgradeAttribute]
    public class MagnetBall : Mech
    {
        public MagnetBall()
        {
            this.rarity = Rarity.Common;
            this.name = "Magnet Ball";
            this.cardText = "Magnetic, Taunt. Overload: (4).";
            this.creatureData = new CreatureData(2, 4, 5);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }
    }

    [UpgradeAttribute]
    public class Shieldmobile : Mech
    {
        public Shieldmobile()
        {
            this.rarity = Rarity.Common;
            this.name = "Shieldmobile";
            this.cardText = "Magnetic. Battlecry: Gain +6 Shields. Overload: (4).";
            this.creatureData = new CreatureData(2, 2, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 6;
        }
    }

    [UpgradeAttribute]
    public class BoomerangMagnet : Mech
    {
        public BoomerangMagnet()
        {
            this.rarity = Rarity.Common;
            this.name = "Boomerang Magnet";
            this.cardText = "Magnetic, Rush. Overload: (4)";
            this.creatureData = new CreatureData(5, 5, 3);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }
    }

    [UpgradeAttribute]
    public class DjinniDecelerator : Mech
    {
        public DjinniDecelerator()
        {
            this.rarity = Rarity.Common;
            this.name = "Djinni Decelerator";
            this.cardText = "Magnetic, Taunt x2";
            this.creatureData = new CreatureData(5, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;            
        }
    }

    [UpgradeAttribute]
    public class TrashCube : Mech
    {
        public TrashCube()
        {
            this.rarity = Rarity.Common;
            this.name = "Trash Cube";
            this.cardText = "Echo, Magnetic";
            this.creatureData = new CreatureData(5, 4, 4);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }
    }

    [UpgradeAttribute]
    public class Spikecycle : Mech
    {
        public Spikecycle()
        {
            this.rarity = Rarity.Common;
            this.name = "Shieldmobile";
            this.cardText = "Magnetic. Battlecry: Gain +6 Spikes. Overload: (4).";
            this.creatureData = new CreatureData(2, 6, 2);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 6;
        }
    }

    [UpgradeAttribute]
    public class CircusCircuit : Mech
    {
        public CircusCircuit()
        {
            this.rarity = Rarity.Common;
            this.name = "Circus Circuit";
            this.cardText = "Aftermath: Add a random Spare Part to your hand.";
            this.creatureData = new CreatureData(3, 2, 3);
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.pool.spareparts.Count());

            gameHandler.players[curPlayer].hand.cards.Add(gameHandler.pool.spareparts[pos].DeepCopy());

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Circus Circuit added a {gameHandler.pool.spareparts[pos].name} to your hand.");
        }
    }

    [UpgradeAttribute]
    public class Tinkerpet: Mech
    {
        private bool spellburst = true;

        public Tinkerpet()
        {
            this.rarity = Rarity.Common;
            this.name = "Tinkerpet";
            this.cardText = this.writtenEffect = "Spellburst: Give your Mech +4 Spikes and +4 Shields.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        public override void OnSpellCast(Card spell, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (this.spellburst)
            {
                this.spellburst = false;
                this.writtenEffect = string.Empty;
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
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
        this.rarity = Rarity.Common;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

 */