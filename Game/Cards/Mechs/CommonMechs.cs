using ScrapScramble.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class HardPuncher : Upgrade
    {
        public HardPuncher()
        {
            this.rarity = Rarity.Common;
            this.name = "Hard Puncher";
            this.cardText = string.Empty;
            this.SetStats(2, 3, 1);
        }
    }

    [UpgradeAttribute]
    public class RunawayTire : Upgrade
    {
        public RunawayTire()
        {
            this.rarity = Rarity.Common;
            this.name = "Runaway Tire";
            this.cardText = "Rush x2";
            this.SetStats(5, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] += 2;
        }
    }

    [UpgradeAttribute]
    public class MedievalSpikeball : Upgrade
    {
        public MedievalSpikeball()
        {
            this.rarity = Rarity.Common;
            this.name = "Medieval Tire";
            this.cardText = "Battlecry: Gain +4 Spikes and +4 Shields";
            this.SetStats(7, 5, 5);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
            return Task.CompletedTask;
        }
    }    

    [UpgradeAttribute]
    public class Oilmental : Upgrade
    {
        public Oilmental()
        {
            this.rarity = Rarity.Common;
            this.name = "Oilmental";
            this.cardText = "Aftermath: Give a random Upgrade in your shop +2/+2. Overload: (3)";
            this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +2/+2.";
            this.SetStats(1, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Upgrade m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.creatureData.attack += 2;
            m.creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Oilmental gives the {m.name} in your shop +2/+2.");
        }
    }    

    [UpgradeAttribute]
    public class SwindlersCoin : Upgrade
    {
        public SwindlersCoin()
        {
            this.rarity = Rarity.Common;
            this.name = "Swindler's Coin";
            this.cardText = "Binary, Tiebreaker. Overload: (1)";
            this.SetStats(1, 0, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }
    }

    [UpgradeAttribute]
    public class ChainMail : Upgrade
    {
        public ChainMail()
        {
            this.rarity = Rarity.Common;
            this.name = "Chain Mail";
            this.cardText = "Binary. Battlecry: Gain +2 Shields. Overload: (1)";
            this.SetStats(2, 1, 2);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    public class TrafficCone : Upgrade
    {
        public TrafficCone()
        {
            this.rarity = Rarity.Common;
            this.name = "Traffic Cone";
            this.cardText = "Binary. Battlecry: Gain +2 Spikes. Overload: (1)";
            this.SetStats(2, 2, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    public class MetalgillSnapper : Upgrade
    {
        public MetalgillSnapper()
        {
            this.rarity = Rarity.Common;
            this.name = "Metalgill Snapper";
            this.cardText = "Overload: (2)";
            this.SetStats(2, 4, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
        }
    }

    [UpgradeAttribute]
    public class ProtectiveFirewall : Upgrade
    {
        public ProtectiveFirewall()
        {
            this.rarity = Rarity.Common;
            this.name = "Protective Firewall";
            this.cardText = "Binary, Taunt";
            this.SetStats(2, 2, 3);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }
    }

    [UpgradeAttribute]
    public class RivetedTrinked : Upgrade
    {
        public RivetedTrinked()
        {
            this.rarity = Rarity.Common;
            this.name = "Riveted Trinked";
            this.cardText = "Binary. Battlecry: Gain +2 Shields.";
            this.SetStats(2, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    public class TankTreads : Upgrade
    {
        public TankTreads()
        {
            this.rarity = Rarity.Common;
            this.name = "Tank Treads";
            this.cardText = "Taunt. Battlecry: Gain +4 Shields. Overload: (3)";
            this.SetStats(2, 3, 4);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    public class ShieldbotClanker : Upgrade
    {
        public ShieldbotClanker()
        {
            this.rarity = Rarity.Common;
            this.name = "Shieldbot Clanker";
            this.cardText = "Battlecry and Aftermath: Gain +8 Shields.";
            this.writtenEffect = "Aftermath: Gain +8 Shields.";
            this.SetStats(5, 2, 3);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
            return Task.CompletedTask;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Shieldbot Clanker gives you +8 Shields.");
        }
    }

    [UpgradeAttribute]
    public class SpikebotShanker : Upgrade
    {
        public SpikebotShanker()
        {
            this.rarity = Rarity.Common;
            this.name = "Spikebot Shanker";
            this.cardText = "Battlecry and Aftermath: Gain +8 Spikes.";
            this.writtenEffect = "Aftermath: Gain +8 Spikes.";
            this.SetStats(5, 3, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
            return Task.CompletedTask;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Spikebot Shanker gives you +8 Spikes.");
        }
    }

    [UpgradeAttribute]
    public class SpeedyProcessor : Upgrade
    {
        public SpeedyProcessor()
        {
            this.rarity = Rarity.Common;
            this.name = "Speedy Processor";
            this.cardText = "Binary, Rush";
            this.SetStats(5, 3, 2);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }
    }

    [UpgradeAttribute]
    public class LeadHead : Upgrade
    {
        public LeadHead()
        {
            this.rarity = Rarity.Common;
            this.name = "Lead Head";
            this.cardText = "Taunt x2";
            this.SetStats(6, 6, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
        }
    }

    [UpgradeAttribute]
    public class Steamfunk : Upgrade
    {
        public Steamfunk()
        {
            this.rarity = Rarity.Common;
            this.name = "Steamfunk";
            this.cardText = this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +2/+2.";
            this.SetStats(7, 7, 7);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Upgrade m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.creatureData.attack += 2;
            m.creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Steamfunk gives the {m.name} in your shop +2/+2.");
        }
    }

    [UpgradeAttribute]
    public class PrismaticBarrier : Upgrade
    {
        public PrismaticBarrier()
        {
            this.rarity = Rarity.Common;
            this.name = "Prismatic Barrier";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +10 Shields.";
            this.SetStats(8, 5, 6);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 10;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Prismatic Barrier gives it +10 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
        }
    }

    [UpgradeAttribute]
    public class TwoHeadedColossus : Upgrade
    {
        public TwoHeadedColossus()
        {
            this.rarity = Rarity.Common;
            this.name = "Two-Headed Colossus";
            this.cardText = "Tiebreaker";
            this.SetStats(11, 11, 11);
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
        }
    }

    [UpgradeAttribute]
    public class ThreeFacedEmojitron : Upgrade
    {
        public ThreeFacedEmojitron()
        {
            this.rarity = Rarity.Common;
            this.name = "Three-Faced Emojitron";
            this.cardText = "Choose One - Gain Rush; or +2/+2.";
            this.SetStats(5, 4, 2);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain Rush\n2) Gain +2/+2", "Write the corresponding number", AnswerType.IntAnswer);
            string defaultAns = GameHandler.randomGenerator.Next(1, 3).ToString();

            string ret = await chooseOne.SendInteractionAsync(curPlayer, (x, y, z) => GeneralFunctions.Within(x, 1, 2), defaultAns);

            if (int.Parse(ret) == 1)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
            }
            else if (int.Parse(ret) == 2)
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
                gameHandler.players[curPlayer].creatureData.health += 2;
            }
        }
    }

    [UpgradeAttribute]
    public class SystemRebooter : Upgrade
    {
        public SystemRebooter()
        {
            this.rarity = Rarity.Common;
            this.name = "System Rebooter";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it Rush.";
            this.SetStats(4, 3, 3);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy);

            chosen.creatureData.staticKeywords[StaticKeyword.Rush]++;
            chosen.cardText += " (Rush)";
        }
    }

    [UpgradeAttribute]
    public class BigFan : Upgrade
    {
        public BigFan()
        {
            this.rarity = Rarity.Common;
            this.name = "Big Fan";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it +3/+3 and Taunt.";
            this.SetStats(4, 3, 3);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy);

            chosen.creatureData.staticKeywords[StaticKeyword.Taunt]++;
            chosen.creatureData.attack += 3;
            chosen.creatureData.health += 3;
            chosen.cardText += " (Taunt)";
        }
    }



    [UpgradeAttribute]
    public class DjinniDecelerator : Upgrade
    {
        public DjinniDecelerator()
        {
            this.rarity = Rarity.Common;
            this.name = "Djinni Decelerator";
            this.cardText = "Magnetic, Taunt x2";
            this.SetStats(5, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
        }
    }

    [UpgradeAttribute]
    public class SyntheticSnowball : Upgrade
    {
        public SyntheticSnowball()
        {
            this.rarity = Rarity.Common;
            this.name = "Synthetic Snowball";
            this.cardText = "Echo. Battlecry: Freeze an Upgrade. Give it +2/+2.";
            this.SetStats(3, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy);

            chosen.creatureData.attack += 2;
            chosen.creatureData.health += 2;
        }
    }
}

/*

[UpgradeAttribute]
public class NextMech : Upgrade
{
    public NextMech()
    {
        this.rarity = Rarity.Common;
        this.name = "";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

 */