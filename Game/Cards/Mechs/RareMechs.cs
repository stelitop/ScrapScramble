using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class CarbonCarapace : Upgrade
    {
        public CarbonCarapace()
        {
            this.rarity = Rarity.Rare;
            this.name = "Carbon Carapace";
            this.cardText = this.writtenEffect = "Start of Combat: Gain Shields equal to the last digit of the enemy Upgrade's Attack.";
            this.SetStats(6, 5, 5);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[enemy].creatureData.attack % 10;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Carbon Carapace gives it +{gameHandler.players[enemy].creatureData.attack % 10} Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
        }
    }            

    [UpgradeAttribute]
    public class OffbrandShoe : Upgrade
    {
        public OffbrandShoe()
        {
            this.rarity = Rarity.Rare;
            this.name = "Offbrand Shoe";
            this.cardText = this.writtenEffect = "Aftermath: Deal 6 damage to your Upgrade.";
            this.SetStats(1, 0, 6);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health -= 6;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Offbrand Shoe deals 6 damage to you.");
        }
    }    

    [UpgradeAttribute]
    public class MkIVSuperCobra : Upgrade
    {
        public MkIVSuperCobra()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mk. IV Super Cobra";
            this.cardText = "Rush. Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.writtenEffect = "Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.SetStats(6, 5, 2);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            if (gameHandler.players[enemy].shop.OptionsCount() == 0) return;

            int index = gameHandler.players[enemy].shop.GetRandomUpgradeIndex();
            gameHandler.players[enemy].shop.RemoveUpgrade(index);
            
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Mk. IV Super Cobra destroyed a random upgrade in your shop.");
        }
    }    

    [UpgradeAttribute]
    public class LightningWeasel : Upgrade
    {
        public LightningWeasel()
        {
            this.rarity = Rarity.Rare;
            this.name = "Lightning Weasel";
            this.cardText = this.writtenEffect = "Aftermath: Replace the highest-cost Upgrade in your opponent's shop with a Lightning Weasel.";
            this.SetStats(2, 1, 1);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;
            if (gameHandler.players[enemy].shop.OptionsCount() == 0) return;

            List<int> highestCosts = new List<int>();
            List<int> enemyIndexes = gameHandler.players[enemy].shop.GetAllUpgradeIndexes();

            int maxCost = -1;
            for (int i = 0; i < enemyIndexes.Count; i++)
            {
                if (maxCost < gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost) 
                    maxCost = gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost;
            }

            for (int i = 0; i < enemyIndexes.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.At(enemyIndexes[i]).Cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            string oldName = gameHandler.players[enemy].shop.At(highestCosts[pos]).name;
            gameHandler.players[enemy].shop.TransformUpgrade(highestCosts[pos], new LightningWeasel());

            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Lightning Weasel replaced your highest-cost Upgrade ({oldName}) with a Lightning Weasel.");
        }
    }

    [UpgradeAttribute]
    public class SocietyProgressor : Upgrade
    {
        public SocietyProgressor()
        {
            this.rarity = Rarity.Rare;
            this.name = "Society Progressor";
            this.cardText = this.writtenEffect = "Aftermath: Remove Binary from all Upgrades in your opponent's shop.";
            this.SetStats(4, 1, 6);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            for (int i = 0; i < gameHandler.players[enemy].shop.totalSize; i++)
            {
                if (gameHandler.players[enemy].shop.At(i).creatureData.staticKeywords[StaticKeyword.Binary] > 0)
                {
                    gameHandler.players[enemy].shop.At(i).creatureData.staticKeywords[StaticKeyword.Binary] = 0;
                    gameHandler.players[enemy].shop.At(i).cardText += "(No Binary)";
                }
            }

            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Society Progressor removed Binary from all Upgrades in your shop.");
        }
    }    

    [UpgradeAttribute]
    public class ElectricBoogaloo : Upgrade
    {
        public ElectricBoogaloo()
        {
            this.rarity = Rarity.Rare;
            this.name = "Electric Boogaloo";
            this.cardText = "Echo. Aftermath: Give a random Upgrade in your shop +4 Attack.";
            this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +4 Attack.";
            this.SetStats(3, 1, 4);
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.creatureData.attack += 4;

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Electric Boogaloo gave the {m.name} in your shop +4 Attack.");
        }
    }

    [UpgradeAttribute]
    public class Pacerager : Upgrade
    {
        public Pacerager()
        {
            this.rarity = Rarity.Rare;
            this.name = "Pacerager";
            this.cardText = "Rush x2. After this takes damage, destroy it.";
            this.writtenEffect = "After this takes damage, destroy it";
            this.SetStats(3, 5, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 2;
        }

        public override void AfterThisTakesDamage(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].destroyed = true;
            gameHandler.combatOutputCollector.combatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Pacerager triggers, destroying {gameHandler.players[curPlayer].name}.");
        }
    }

    //[UpgradeAttribute]
    public class PrismaticReflectotron : Upgrade
    {
        private bool triggered;
        public PrismaticReflectotron()
        {
            this.rarity = Rarity.Rare;
            this.name = "Prismatic Reflectotron";
            this.cardText = this.writtenEffect = "After your Upgrade takes damage for the first time, deal the same amount to the enemy Upgrade.";
            this.SetStats(6, 2, 2);
            this.triggered = false;
        }

        public override void AfterThisTakesDamage(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!this.triggered)
            {
                this.triggered = true;
                gameHandler.players[enemy].TakeDamage(damage, gameHandler, curPlayer, enemy,
                    $"{gameHandler.players[curPlayer].name}'s Prismatic Reflectotron triggers, dealing {damage} damage, ");
            }
        }
    }    

    [UpgradeAttribute]
    public class Autobalancer : Upgrade
    {
        public Autobalancer()
        {
            this.rarity = Rarity.Rare;
            this.name = "Autobalancer";
            this.cardText = this.writtenEffect = "Start of Combat: The player who bought fewer Upgrades last turn gives their Upgrade +2/+2. If tied, gain +1/+1.";
            this.SetStats(4, 4, 4);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].boughtThisTurn.Count() < gameHandler.players[enemy].boughtThisTurn.Count())
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
                gameHandler.players[curPlayer].creatureData.health += 2;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Autobalancer gives it +2/+2, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
            }
            else if (gameHandler.players[curPlayer].boughtThisTurn.Count() > gameHandler.players[enemy].boughtThisTurn.Count())
            {
                gameHandler.players[enemy].creatureData.attack += 2;
                gameHandler.players[enemy].creatureData.health += 2;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Autobalancer gives {gameHandler.players[enemy].name} +2/+2, leaving it as a {gameHandler.players[enemy].creatureData.Stats()}.");
            }
            else
            {
                gameHandler.players[curPlayer].creatureData.attack += 1;
                gameHandler.players[curPlayer].creatureData.health += 1;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Autobalancer gives it +1/+1 due to a tie, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
            }
        }
    }    

    [UpgradeAttribute]
    public class ByteBarker : Upgrade
    {
        public ByteBarker()
        {
            this.rarity = Rarity.Rare;
            this.name = "Byte Barker";
            this.cardText = "Binary. Choose One - Gain +6 Spikes; or +6 Shields.";
            this.SetStats(6, 4, 4);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }
        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction chooseOne = new PlayerInteraction("Choose One", "1) Gain +6 Spikes\n2) Gain +6 Shields", "Write the corresponding number", AnswerType.IntAnswer);
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
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 6;
                    }
                    else if (int.Parse(res) == 2)
                    {
                        gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 6;
                    }
                    else continue;
                    break;
                }
            }
        }
    }

    //[UpgradeAttribute]
    public class ThreeDPrinter : Upgrade
    {
        public ThreeDPrinter()
        {
            this.rarity = Rarity.Rare;
            this.name = "3D Printer";
            this.cardText = "Battlecry: Name an Upgrade in the game. Add a copy of it to your shop.";
            this.SetStats(5, 0, 7);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            var playerInteraction = new PlayerInteraction("Name an Upgrade", string.Empty, "Capitalisation is ignored", AnswerType.StringAnswer);

            string res;
            bool show = true;
            while (true)
            {
                res = playerInteraction.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    show = true;
                    continue;
                }
                else
                {
                    bool end = false;
                    for (int i = 0; i < gameHandler.pool.mechs.Count(); i++)
                        if (gameHandler.pool.mechs[i].name.Equals(res, StringComparison.OrdinalIgnoreCase))
                        {
                            gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.pool.mechs[i]);
                            end = true;
                            break;
                        }
                    if (end) break;
                    continue;
                }
            }
        }
    }

    [UpgradeAttribute]
    public class BrassBracer : Upgrade
    {
        public BrassBracer()
        {
            this.rarity = Rarity.Rare;
            this.name = "Brass Bracer";
            this.cardText = this.writtenEffect = "This minion ignores damage from Spikes.";
            this.SetStats(5, 3,4);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.ignoreSpikes = true;
        }
    }

    [UpgradeAttribute]
    public class RadiatingCrucible : Upgrade
    {
        public RadiatingCrucible()
        {
            this.rarity = Rarity.Rare;
            this.name = "Radiating Crucible";
            this.cardText = this.writtenEffect = "This minion's attacks ignore Shields.";
            this.SetStats(5, 4, 3);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.ignoreShields = true;
        }
    }   

    //[UpgradeAttribute]
    public class FuriousRelic : Upgrade
    {
        bool triggered = false;

        public FuriousRelic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Furious Relic";
            this.cardText = this.writtenEffect = "Start of Combat: The next time your Upgrade attacks, it attacks twice in a row.";
            this.SetStats(6, 2, 2);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!triggered)
            {
                triggered = true;

                int dmg = gameHandler.players[curPlayer].AttackMech(gameHandler, curPlayer, enemy);

                if (!gameHandler.players[curPlayer].IsAlive() || !gameHandler.players[enemy].IsAlive()) return;

                for (int i = 0; i < gameHandler.players[curPlayer].attachedMechs.Count() && gameHandler.players[curPlayer].IsAlive() && gameHandler.players[enemy].IsAlive(); i++)
                {
                    gameHandler.players[curPlayer].attachedMechs[i].AfterThisAttacks(dmg, gameHandler, curPlayer, enemy);
                    
                }

                foreach (var extraEffect in gameHandler.players[curPlayer].extraUpgradeEffects)
                {
                    if (!(gameHandler.players[curPlayer].IsAlive() && gameHandler.players[enemy].IsAlive())) break;
                    extraEffect.AfterThisAttacks(dmg, gameHandler, curPlayer, enemy);
                }


                for (int i = 0; i < gameHandler.players[enemy].attachedMechs.Count() && gameHandler.players[curPlayer].IsAlive() && gameHandler.players[enemy].IsAlive(); i++)
                {
                    gameHandler.players[enemy].attachedMechs[i].AfterTheEnemyAttacks(dmg, gameHandler, curPlayer, enemy);                    
                }

                foreach (var extraEffect in gameHandler.players[curPlayer].extraUpgradeEffects)
                {
                    if (!(gameHandler.players[curPlayer].IsAlive() && gameHandler.players[enemy].IsAlive())) break;
                    extraEffect.AfterTheEnemyAttacks(dmg, gameHandler, curPlayer, enemy);
                }
            }
        }
    }

    //[UpgradeAttribute]
    public class DivineRelic : Upgrade
    {
        private bool triggered = false;
        public DivineRelic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Divine Relic";
            this.cardText = this.writtenEffect = "Start of Combat: The next time your Upgrade would take damage this turn, ignore it.";
            this.SetStats(6, 2, 2);
        }

        public override void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg)
        {
            if (triggered) return;

            if (damage <= 0) return;

            triggered = true;
            damage = 0;
            msg += $"prevented by Divine Relic, ";
        }
    }    

    [TokenAttribute]
    public class BeeBot : Upgrade
    {
        public BeeBot()
        {
            this.name = "Bee Bot";
            this.rarity = Rarity.NO_RARITY;
            this.SetStats(1, 1, 1);
        }
    }

    public class HiveMindEffect : Upgrade
    {
        public HiveMindEffect()
        {
            this.printEffectInCombat = false;
            this.writtenEffect = "After you buy an Upgrade this turn, add a 1/1 Bee Bot to your shop.";
        }

        public override void OnBuyingAMech(Upgrade m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.AddUpgrade(new BeeBot());
        }
    }

    [UpgradeAttribute]
    public class HiveMind : Upgrade
    {
        public HiveMind()
        {
            this.rarity = Rarity.Rare;
            this.name = "Hive Mind";
            this.cardText = this.writtenEffect = "Aftermath: After you buy an Upgrade this turn, add a 1/1 Bee Bot to your shop.";
            this.SetStats(2, 2, 2);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].nextRoundEffects.Add(new HiveMindEffect());

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Due to your {this.name}, after you buy an Upgrade this turn, add a 1/1 Bee Bot to your shop.");
        }
    }    
}

/*

[UpgradeAttribute]
public class NextMech : Upgrade
{
    public NextMech()
    {
        this.rarity = Rarity.Rare;
        this.name = "";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

*/
