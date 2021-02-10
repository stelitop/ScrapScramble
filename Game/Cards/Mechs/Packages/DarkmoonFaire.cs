using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class SyntheticSnowball : Mech
    {
        public SyntheticSnowball()
        {
            this.rarity = Rarity.Common;
            this.name = "Synthetic Snowball";
            this.cardText = "Echo. Battlecry: Freeze an Upgrade. Give it +2/+2.";
            this.SetStats(3, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.At(shopIndex).creatureData.health += 2;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class PeekABot : Mech
    {
        public PeekABot()
        {
            this.rarity = Rarity.Rare;
            this.name = "Peek-a-Bot";
            this.cardText = this.writtenEffect = "Aftermath: You are told the most expensive Upgrade in your opponent's shop.";
            this.SetStats(1, 1, 1);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;
            if (gameHandler.players[enemy].shop.OptionsCount() == 0)
            {
                gameHandler.players[curPlayer].aftermathMessages.Add("Your opponent's shop is empty.");
                return;
            }

            List<Mech> enemyMechs = gameHandler.players[enemy].shop.GetAllUpgrades();
            List<int> highestCosts = new List<int>();
            int maxCost = -1;

            for (int i = 0; i < enemyMechs.Count(); i++)
            {
                if (maxCost < enemyMechs[i].cost) maxCost = enemyMechs[i].cost;
            }

            for (int i = 0; i < enemyMechs.Count(); i++)
            {
                if (enemyMechs[i].cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Peek-a-Bot tells you the most expensive Upgrade in your opponent's shop is {enemyMechs[highestCosts[pos]].name}");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class RoboRabbit : Mech
    {
        public RoboRabbit()
        {
            this.rarity = Rarity.Rare;
            this.name = "Robo-Rabbit";
            this.cardText = "Battlecry: Gain +2/+2 for each other Robo-Rabbit you've played this game.";
            this.SetStats(2, 1, 1);
        }

        private bool Criteria(Card m)
        {
            return m.name.Equals("Robo-Rabbit");
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.Criteria);

            gameHandler.players[curPlayer].creatureData.attack += 2 * list.Count();
            gameHandler.players[curPlayer].creatureData.health += 2 * list.Count();
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            return base.GetInfo(gameHandler, player) + $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, Criteria).Count})*";
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class TrickRoomster : Mech
    {
        public TrickRoomster()
        {
            this.rarity = Rarity.Rare;
            this.name = "Trick Roomster";
            this.cardText = this.writtenEffect = "The Mech with the lower Attack Priority goes first instead.";
            this.SetStats(4, 1, 1);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.invertAttackPriority = true;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class SpringloadedJester : Mech
    {
        public SpringloadedJester()
        {
            this.rarity = Rarity.Epic;
            this.name = "Springloaded Jester";
            this.cardText = this.writtenEffect = "After this attacks, swap your Mech's Attack and Health.";
            this.SetStats(2, 1, 1);
        }

        public override void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            GeneralFunctions.Swap<int>(ref gameHandler.players[curPlayer].creatureData.attack, ref gameHandler.players[curPlayer].creatureData.health);
            gameHandler.combatOutputCollector.combatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Springloaded Jester swaps its stats, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class FortuneWheel : Mech
    {
        public FortuneWheel()
        {
            this.rarity = Rarity.Epic;
            this.name = "Fortune Wheel";
            this.cardText = this.writtenEffect = "Aftermath: Cast 3 random Spare Parts with random targets.";
            this.SetStats(3, 3, 3);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            string aftermathMsg = "Your Fortune Wheel casted ";

            for (int i = 0; i < 3; i++)
            {
                Spell sparePart = (Spell)gameHandler.pool.spareparts[GameHandler.randomGenerator.Next(0, gameHandler.pool.spareparts.Count())].DeepCopy();

                if (i == 0) aftermathMsg += $"{sparePart.name}";
                else if (i == 1) aftermathMsg += $", {sparePart.name}";
                else if (i == 2) aftermathMsg += $" and {sparePart.name}";

                if (sparePart.name == "Mana Capsule")
                {
                    sparePart.OnPlay(gameHandler, curPlayer, enemy);
                }
                else
                {
                    int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
                    sparePart.CastOnUpgradeInShop(index, gameHandler, curPlayer, enemy);
                    aftermathMsg += $"({gameHandler.players[curPlayer].shop.At(index).name})";
                }
            }

            aftermathMsg += " on random targets.";

            gameHandler.players[curPlayer].aftermathMessages.Add(aftermathMsg);
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class HighRoller : Mech
    {
        public HighRoller()
        {
            this.rarity = Rarity.Epic;
            this.name = "High Roller";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Cost of a random Upgrade in your shop by (4).";
            this.SetStats(4, 3, 3);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Mech m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.cost -= 4;
            if (m.cost < 0) m.cost = 0;

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Highroller reduces the cost of {m.name} in your shop by (4).");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class Mirrordome : Mech
    {
        public Mirrordome()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mirrordome";
            this.cardText = this.writtenEffect = "Aftermath: This turn, your shop is a copy of your opponent's.";
            this.SetStats(4, 0, 8);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[curPlayer].shop.Clear();

            for (int i = 0; i < gameHandler.players[enemy].shop.totalSize; i++)
            {
                gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[enemy].shop.At(i));
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Mirrordome replaced your shop with a copy of {gameHandler.players[enemy].name}'s shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.DarkmoonFaire)]
    public class HatChucker8000 : Mech
    {
        private Rarity chosenRarity = Rarity.NO_RARITY;
        public HatChucker8000()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Hat Chucker 8000";
            this.cardText = "Battlecry: Name a Rarity. Aftermath: Give all players' Upgrades of that rarity +2/+2.";
            this.SetStats(3, 3, 3);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            var prompt = new PlayerInteraction("Name a Rarity", "Common, Rare, Epic or Legendary", "Capitalisation is ignored", AnswerType.StringAnswer);

            string res;
            bool show = true;
            while (true)
            {
                res = prompt.SendInteractionAsync(curPlayer, show).Result;
                show = false;
                if (res.Equals(string.Empty)) continue;
                if (res.Equals("TimeOut"))
                {
                    show = true;
                    continue;
                }

                if (res.Equals("common", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Common;
                else if (res.Equals("rare", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Rare;
                else if (res.Equals("epic", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Epic;
                else if (res.Equals("legendary", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Legendary;
                else continue;

                this.writtenEffect = $"Aftermath: Give all players' Upgrades of rarity {this.chosenRarity} +2/+2.";
                this.printEffectInCombat = false;

                break;
            }
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgrades();

            for (int i = 0; i < upgrades.Count(); i++)
            {
                if (upgrades[i].rarity == this.chosenRarity)
                {
                    upgrades[i].creatureData.attack += 2;
                    upgrades[i].creatureData.health += 2;
                }
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Hat Chucker 8000 gave your {this.chosenRarity} Upgrades +2/+2.");
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            for (int j = 0; j < gameHandler.players.Count(); j++)
            {
                if (j == curPlayer) continue;
                if (gameHandler.players[j].lives <= 0) continue;

                List<Mech> upgrades = gameHandler.players[j].shop.GetAllUpgrades();

                for (int i = 0; i < upgrades.Count(); i++)
                {
                    if (upgrades[i].rarity == this.chosenRarity)
                    {
                        upgrades[i].creatureData.attack += 2;
                        upgrades[i].creatureData.health += 2;
                    }
                }

                gameHandler.players[j].aftermathMessages.Add(
                    $"{gameHandler.players[curPlayer].name}'s Hat Chucker 8000 gave your {this.chosenRarity} Upgrades +2/+2.");
            }
        }

        public override Card DeepCopy()
        {
            HatChucker8000 ret = (HatChucker8000)base.DeepCopy();
            ret.chosenRarity = this.chosenRarity;
            return ret;
        }
    }
}
