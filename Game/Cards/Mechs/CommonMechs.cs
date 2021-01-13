﻿using ScrapScramble.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {            
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[pos].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[pos].creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Oilmental gives the {gameHandler.players[curPlayer].shop.options[pos].name} in your shop +2/+2.");
        }
    }

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
    public class VentureCoSticker : Mech
    {
        public VentureCoSticker()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Sticker";
            this.cardText = string.Empty;
            this.creatureData = new CreatureData(1, 0, 2);
        }
    }

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Shieldbot Clanker gives you +8 Shields.");
        }
    }

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Spikebot Shanker gives you +8 Spikes.");
        }
    }

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
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

    [MechAttribute]
    public class Steamfunk : Mech
    {
        public Steamfunk()
        {
            this.rarity = Rarity.Common;
            this.name = "Steamfunk";
            this.cardText = this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +2/+2.";
            this.creatureData = new CreatureData(7, 7, 7);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[pos].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[pos].creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Steamfunk gives the {gameHandler.players[curPlayer].shop.options[pos].name} in your shop +2/+2.");
        }
    }

    [MechAttribute]
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

    [MechAttribute]
    public class SixpistolConstable : Mech
    {
        public SixpistolConstable()
        {
            this.rarity = Rarity.Common;
            this.name = "Sixpistol Constable";
            this.cardText = "Rush x6";
            this.creatureData = new CreatureData(10, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 6;
        }
    }

    [MechAttribute]
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

    [MechAttribute]
    public class OrbitalMechanosphere : Mech
    {
        public OrbitalMechanosphere()
        {
            this.rarity = Rarity.Common;
            this.name = "Orbital Mechanosphere";
            this.cardText = string.Empty;
            this.creatureData = new CreatureData(30, 30, 30);            
        }
    }

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
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

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
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

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
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

    [MechAttribute]
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
        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
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

}

/*

[MechAttribute]
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