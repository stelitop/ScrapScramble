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
            this.SetStats(2, 3, 1);
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
            this.SetStats(4, 3, 1);
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
            this.SetStats(5, 1, 1);
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
            this.SetStats(7, 5, 5);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(3, 5, 5);
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
            this.SetStats(2, 0, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(0, 0, 8);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(0, 1, 1);
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
            this.SetStats(1, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Mech m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.creatureData.attack += 2;
            m.creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Oilmental gives the {m.name} in your shop +2/+2.");
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
            this.SetStats(1, 1, 3);
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
            this.SetStats(1, 0, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
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
            this.SetStats(2, 1, 2);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(2, 2, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(2, 4, 2);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
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
            this.SetStats(2, 2, 3);
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
            this.SetStats(2, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(2, 3, 4);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
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
            this.SetStats(4, 4, 8);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack -= 4;
            if (gameHandler.players[curPlayer].creatureData.attack < 1) gameHandler.players[curPlayer].creatureData.attack = 1;

            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Malfunctioning Puncher reduces its Attack by 4, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
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
            this.SetStats(5, 2, 3);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(5, 3, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 8;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(5, 3, 2);
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
            this.SetStats(6, 6, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;
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
            this.SetStats(7, 7, 7);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Mech m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.creatureData.attack += 2;
            m.creatureData.health += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Steamfunk gives the {m.name} in your shop +2/+2.");
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
    public class TwoHeadedColossus : Mech
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
    public class ThreeFacedEmojitron : Mech
    {
        public ThreeFacedEmojitron()
        {
            this.rarity = Rarity.Common;
            this.name = "Three-Faced Emojitron";
            this.cardText = "Choose One - Gain Rush; or +2/+2.";
            this.SetStats(5, 4, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(2, 2, 4);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
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
            this.SetStats(4, 3, 3);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.staticKeywords[StaticKeyword.Rush]++;
            gameHandler.players[curPlayer].shop.At(shopIndex).cardText += " (Rush)";
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
            this.SetStats(4, 3, 3);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.staticKeywords[StaticKeyword.Taunt]++;
            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.attack += 3;
            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.health += 3;
            gameHandler.players[curPlayer].shop.At(shopIndex).cardText += " (Taunt)";
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
            this.SetStats(5, 6, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 2;            
        }
    }        

    [UpgradeAttribute]
    public class BootPolisher : Mech
    {
        public BootPolisher()
        {
            this.rarity = Rarity.Common;
            this.name = "Boot Polisher";
            this.cardText = this.writtenEffect = "Start of Combat: If the enemy Mech has Attack Priority, gain +4 Shields.";
            this.SetStats(3, 2, 3);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.combatOutputCollector.goingFirst == enemy)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Boot Polisher triggers, giving it +4 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Boot Polisher fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    public class CutleryDispencer : Mech
    {
        public CutleryDispencer()
        {
            this.rarity = Rarity.Common;
            this.name = "Cutlery Dispencer";
            this.cardText = this.writtenEffect = "Start of Combat: If your Mech has Attack Priority, gain +4 Spikes.";
            this.SetStats(3, 3, 2);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.combatOutputCollector.goingFirst == curPlayer)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer triggers, giving it +4 Spikes, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes]} Spikes.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer fails to trigger.");
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
        this.SetStats(0, 0, 0);
    }
}

 */