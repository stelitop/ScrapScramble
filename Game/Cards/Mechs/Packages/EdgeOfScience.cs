using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class OrbitalMechanosphere : Upgrade
    {
        public OrbitalMechanosphere()
        {
            this.rarity = Rarity.Common;
            this.name = "Orbital Mechanosphere";
            this.cardText = string.Empty;
            this.SetStats(22, 33, 33);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class PoolOfBronze : Upgrade
    {
        public PoolOfBronze()
        {
            this.rarity = Rarity.Common;
            this.name = "Pool of Bronze";
            this.cardText = this.writtenEffect = "Aftermath: Replace your shop with 6 Common Upgrades.";
            this.SetStats(2, 6, 2);
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.Clear();

            List<Upgrade> subList = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Common && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);
            for (int i=0; i<6; i++)
            {                                
                Upgrade m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                gameHandler.players[curPlayer].shop.AddUpgrade(m);
            }
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class GiantPhoton : Upgrade
    {
        public GiantPhoton()
        {
            this.rarity = Rarity.Common;
            this.name = "Giant Photon";
            this.cardText = "Rush. Overload: (3)";
            this.SetStats(4, 5, 4);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class VoltageTracker : Upgrade
    {
        public VoltageTracker()
        {
            this.rarity = Rarity.Common;
            this.name = "Voltage Tracker";
            this.cardText = "Battlecry: Gain +1/+1 for each Overloaded Mana Crystal you have.";
            this.SetStats(4, 2, 3);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack +=
                gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];
            gameHandler.players[curPlayer].creatureData.health +=
                gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];

            return base.Battlecry(gameHandler, curPlayer, enemy);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class StasisCrystal : Upgrade
    {
        public StasisCrystal()
        {
            this.rarity = Rarity.Common;
            this.name = "Stasis Crystal";
            this.cardText = "Battlecry: Increase your Maximum Mana by 1.";
            this.SetStats(2, 0, 2);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class IndecisiveAutoshopper : Upgrade
    {
        public IndecisiveAutoshopper()
        {
            this.rarity = Rarity.Rare;
            this.name = "Indecisive Autoshopper";
            this.cardText = "Binary. Battlecry: Refresh your shop.";
            this.SetStats(4, 2, 4);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.players[curPlayer].pool, gameHandler.players[curPlayer].maxMana, false);
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class SocietyProgressor : Upgrade
    {
        public SocietyProgressor()
        {
            this.rarity = Rarity.Rare;
            this.name = "Society Progressor";
            this.cardText = this.writtenEffect = "Aftermath: Remove Binary from all Upgrades in your opponent's shop.";
            this.SetStats(4, 1, 7);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            for (int i = 0; i < gameHandler.players[enemy].shop.LastIndex; i++)
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
    [Set(UpgradeSet.EdgeOfScience)]
    public class EnergeticField : Upgrade
    {
        public EnergeticField()
        {
            this.rarity = Rarity.Rare;
            this.name = "Energetic Field";
            this.cardText = "Battlecry: If you're Overloaded, add 3 random Upgrades that Overload to your hand.";
            this.SetStats(3, 3, 3);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].overloaded > 0 || gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 0)
            {
                List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.creatureData.staticKeywords[StaticKeyword.Overload] > 0);
                for (int i=0; i<3; i++)
                {
                    int x = GameHandler.randomGenerator.Next(0, pool.Count);
                    gameHandler.players[curPlayer].hand.AddCard(pool[x]);
                }
            }

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class LightChaser : Upgrade
    {
        public LightChaser()
        {
            this.rarity = Rarity.Rare;
            this.name = "Light Chaser";
            this.cardText = "Battlecry: Gain Rush x1 for each Overloaded Mana Crystal you have.";
            this.SetStats(12, 9, 7);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] +=
                gameHandler.players[curPlayer].overloaded + gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload];

            return base.Battlecry(gameHandler, curPlayer, enemy);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class EvolveOMatic : Upgrade
    {
        public EvolveOMatic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Evolve-o-Matic";
            this.cardText = "Battlecry: Choose an Upgrade in your shop. Discover an Upgrade that costs (1) more to replace it with.";
            this.SetStats(5, 4, 6);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int chosenIndex = await PlayerInteraction.ChooseUpgradeInShopAsync(gameHandler, curPlayer, enemy);
            if (chosenIndex == -1) return;

            int newCost = gameHandler.players[curPlayer].shop.At(chosenIndex).Cost+1;

            List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == newCost);

            if (pool.Count > 0)
            {
                Upgrade x = (Upgrade)(await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, "Discover an Upgrade to replace with", pool, false));
                gameHandler.players[curPlayer].shop.TransformUpgrade(chosenIndex, x);                
            }
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class DevolveOMatic : Upgrade
    {
        public DevolveOMatic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Devolve-o-Matic";
            this.cardText = "Battlecry: Choose an Upgrade in your shop. Discover an Upgrade that costs (1) less to replace it with.";
            this.SetStats(5, 6, 4);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int chosenIndex = await PlayerInteraction.ChooseUpgradeInShopAsync(gameHandler, curPlayer, enemy);
            if (chosenIndex == -1) return;

            int newCost = gameHandler.players[curPlayer].shop.At(chosenIndex).Cost - 1;

            List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == newCost);

            if (pool.Count > 0)
            {
                Upgrade x = (Upgrade)(await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, "Discover an Upgrade to replace with", pool, false));
                gameHandler.players[curPlayer].shop.TransformUpgrade(chosenIndex, x);
            }
        }
    }


    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class MassAccelerator : Upgrade
    {
        public MassAccelerator()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mass Accelerator";
            this.cardText = this.writtenEffect = "Start of Combat: If you're Overloaded, deal 10 damage to the enemy Mech.";
            this.SetStats(6, 5, 5);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 0 || gameHandler.players[curPlayer].overloaded > 0)
            {
                gameHandler.players[enemy].TakeDamage(10, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Mass Accelerator triggers and deals 10 damage, ");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Mass Accelerator fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class PhilosophersStone : Upgrade
    {
        public PhilosophersStone()
        {
            this.rarity = Rarity.Epic;
            this.name = "Philosopher's Stone";
            this.cardText = "Battlecry: Transform all Upgrades in your shop into ones of the next Rarity.";
            this.SetStats(2, 1, 1);            
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> legendaries = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Legendary && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);
            List<Upgrade> epics = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Epic && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);
            List<Upgrade> rares = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Rare && x.Cost <= gameHandler.players[curPlayer].maxMana - 5);

            List<int> shopIndexes = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

            for (int i=0; i<shopIndexes.Count; i++)
            {
                switch (gameHandler.players[curPlayer].shop.At(shopIndexes[i]).rarity)
                {
                    case Rarity.Common:
                        gameHandler.players[curPlayer].shop.TransformUpgrade(shopIndexes[i], rares[GameHandler.randomGenerator.Next(0, rares.Count())]);
                        break;
                    case Rarity.Rare:
                        gameHandler.players[curPlayer].shop.TransformUpgrade(shopIndexes[i], epics[GameHandler.randomGenerator.Next(0, epics.Count())]);
                        break;
                    case Rarity.Epic:
                        gameHandler.players[curPlayer].shop.TransformUpgrade(shopIndexes[i], legendaries[GameHandler.randomGenerator.Next(0, legendaries.Count())]);
                        break;
                    default:
                        break;
                }
            }

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class DatabaseCore : Upgrade
    {
        private CreatureData _keywords;
        private string _aftermathMsg;

        public DatabaseCore()
        {
            this.rarity = Rarity.Epic;
            this.name = "Database Core";
            this.cardText = "Battlecry: Remember your Mech's keywords. Aftermath: Add them to your Mech.";            
            this.writtenEffect = string.Empty;            
            this.SetStats(6, 4, 5);

            this._keywords = new CreatureData();
            this._aftermathMsg = string.Empty;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            this._keywords = gameHandler.players[curPlayer].creatureData.DeepCopy();

            this.writtenEffect = "Aftermath: Gain ";
            this._aftermathMsg = $"Your {this.name} gives you ";

            bool checkedFirst = false;
            foreach (var kw in _keywords.staticKeywords)
            {
                if (kw.Value != 0)
                {
                    if (!checkedFirst)
                    {
                        writtenEffect += $"{kw.Key} x{kw.Value}";
                        _aftermathMsg += $"{kw.Key} x{kw.Value}";
                    }
                    else
                    {
                        writtenEffect += $", {kw.Key} x{kw.Value}";
                        _aftermathMsg += $", {kw.Key} x{kw.Value}";
                    }
                    checkedFirst = true;
                }
            }
            writtenEffect += ".";
            _aftermathMsg += ".";

            if (!checkedFirst)
            {
                writtenEffect = string.Empty;
                _aftermathMsg = string.Empty;
            }

            return Task.CompletedTask;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            foreach (var kw in _keywords.staticKeywords)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[kw.Key] += kw.Value;
            }
            gameHandler.players[curPlayer].aftermathMessages.Add(_aftermathMsg);
        }

        public override Card DeepCopy()
        {
            DatabaseCore ret = (DatabaseCore)base.DeepCopy();
            ret._keywords = this._keywords.DeepCopy();
            ret._aftermathMsg = this._aftermathMsg;
            return ret;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class HyperMagneticCloud : Upgrade
    {
        public HyperMagneticCloud()
        {
            this.rarity = Rarity.Epic;
            this.name = "Hyper-Magnetic Cloud";
            this.cardText = this.writtenEffect = "Aftermath: If you're Overloaded for at least 7 Mana, add all 7 Spare Parts to your hand.";
            this.SetStats(7, 7, 6);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].overloaded >= 7)
            {
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    $"Your {this.name} triggers, adding all 7 Spare Parts to your hand.");

                foreach (var sp in gameHandler.players[curPlayer].pool.spareparts)
                {
                    gameHandler.players[curPlayer].hand.AddCard(sp);    
                }
            }
            else
            {
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    $"Your {this.name} fails to trigger.");
            }

            base.AftermathMe(gameHandler, curPlayer, enemy);
        }
    }    

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class ParadoxEngine : Upgrade
    {
        public ParadoxEngine()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Paradox Engine";
            this.cardText = "Battlecry: This turn, after you buy an Upgrade, refresh your shop.";
            this.writtenEffect = "After you buy an Upgrade, refresh your shop.";
            this.SetStats(12, 10, 10);
            this.showEffectInCombat = false;
        }

        public override void OnBuyingAMech(Upgrade m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.players[curPlayer].pool, gameHandler.players[curPlayer].maxMana, false);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.EdgeOfScience)]
    public class EarthsPrototypeCore : Upgrade
    {
        public EarthsPrototypeCore()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Earth's Prototype Core";
            this.cardText = "Battlecry: For each Overload Upgrade applied to your Mech this game, increase your Maximum Mana by 1.";
            this.SetStats(7, 0, 12);
        }

        private bool criteria(Card c)
        {
            if (c is Upgrade u)
            {
                return (u.creatureData.staticKeywords[StaticKeyword.Overload] > 0);
            }

            return false;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int bonus = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.criteria).Count;

            gameHandler.players[curPlayer].maxMana += bonus;

            return Task.CompletedTask;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            return base.GetInfo(gameHandler, player) + $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, this.criteria).Count})*";
        }

    }

}
