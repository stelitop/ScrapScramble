using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class ToyRocket : Upgrade
    {
        public ToyRocket()
        {
            this.rarity = Rarity.Common;
            this.name = "Toy Rocket";
            this.cardText = "Rush";
            this.SetStats(4, 3, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class ToyTank : Upgrade
    {
        public ToyTank()
        {
            this.rarity = Rarity.Common;
            this.name = "Toy Tank";
            this.cardText = "Taunt";
            this.SetStats(1, 1, 3);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }
    }


    [TokenAttribute]
    public class PlushieClawMachine : Upgrade
    {
        public PlushieClawMachine()
        {
            this.name = "Plushie";
            this.SetStats(1, 1, 1);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class ClawMachine : Upgrade
    {
        public ClawMachine()
        {
            this.rarity = Rarity.Common;
            this.name = "Claw Machine";
            this.cardText = "Battlecry: Add three 1/1 Plushies to your hand.";
            this.SetStats(3, 3, 2);            
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Card token = new PlushieClawMachine();

            for (int i=0; i<3; i++)
            {
                gameHandler.players[curPlayer].hand.AddCard(
                    gameHandler.players[curPlayer].pool.FindBasicCard(token.name));
            }
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class FerrisWheel : Upgrade
    {
        public FerrisWheel()
        {
            this.rarity = Rarity.Common;
            this.name = "Ferris Wheel";
            this.cardText = "Aftermath: Return this to your shop. It costs (1) less than last time.";
            this.SetStats(6, 5, 5);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Upgrade copy = (Upgrade)this.DeepCopy();
            copy.Cost = Math.Max(0, copy.Cost - 1);
            gameHandler.players[curPlayer].shop.AddUpgrade(copy);
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Ferris Wheel has returned to your shop. It now costs {copy.Cost}.");
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class MalfunctioningGuard : Upgrade
    {
        public MalfunctioningGuard()
        {
            this.rarity = Rarity.Common;
            this.name = "Malfunctioning Guard";
            this.cardText = "Start of Combat: Your Mech loses -4 Attack. Overload: (1)";
            this.writtenEffect = "Start of Combat: Your Mech loses -4 Attack.";
            this.SetStats(4, 4, 8);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
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
    [Set(UpgradeSet.IronmoonFaire)]
    public class PeekABot : Upgrade
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

            List<Upgrade> enemyMechs = gameHandler.players[enemy].shop.GetAllUpgrades();
            List<int> highestCosts = new List<int>();
            int maxCost = -1;

            for (int i = 0; i < enemyMechs.Count(); i++)
            {
                if (maxCost < enemyMechs[i].Cost) maxCost = enemyMechs[i].Cost;
            }

            for (int i = 0; i < enemyMechs.Count(); i++)
            {
                if (enemyMechs[i].Cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Peek-a-Bot tells you the most expensive Upgrade in your opponent's shop is {enemyMechs[highestCosts[pos]].name}");
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class RoboRabbit : Upgrade
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

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.Criteria);

            gameHandler.players[curPlayer].creatureData.attack += 2 * list.Count();
            gameHandler.players[curPlayer].creatureData.health += 2 * list.Count();
            return Task.CompletedTask;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            return base.GetInfo(gameHandler, player) + $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, Criteria).Count})*";
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class TrickRoomster : Upgrade
    {
        public TrickRoomster()
        {
            this.rarity = Rarity.Rare;
            this.name = "Trick Roomster";
            this.cardText = this.writtenEffect = "The Mech with the lower Attack Priority goes first instead.";
            this.SetStats(4, 1, 1);
        }

        public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.invertAttackPriority = true;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class PrizeStacker : Upgrade
    {
        public PrizeStacker()
        {
            this.rarity = Rarity.Rare;
            this.name = "Prize Stacker";
            this.cardText = "Battlecry: Give your Mech +1/+1 for each card in your hand.";
            this.SetStats(4, 2, 4);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].hand.OptionsCount();
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].hand.OptionsCount();
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class SpringloadedJester : Upgrade
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
    [Set(UpgradeSet.IronmoonFaire)]
    public class FortuneWheel : Upgrade
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
                Spell sparePart = (Spell)gameHandler.players[curPlayer].pool.spareparts[GameHandler.randomGenerator.Next(0, gameHandler.pool.spareparts.Count())].DeepCopy();

                if (i == 0) aftermathMsg += $"{sparePart.name}";
                else if (i == 1) aftermathMsg += $", {sparePart.name}";
                else if (i == 2) aftermathMsg += $" and {sparePart.name}";

                if (sparePart.name == "Mana Capsule")
                {
                    //await sparePart.OnPlay(gameHandler, curPlayer, enemy);
                    gameHandler.players[curPlayer].curMana += 2;
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
    [Set(UpgradeSet.IronmoonFaire)]
    public class HighRoller : Upgrade
    {
        public HighRoller()
        {
            this.rarity = Rarity.Epic;
            this.name = "High Roller";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the cost of a random Upgrade in your shop by (4).";
            this.SetStats(4, 3, 3);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade m = gameHandler.players[curPlayer].shop.GetRandomUpgrade();
            m.Cost -= 4;
            if (m.Cost < 0) m.Cost = 0;

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Highroller reduces the cost of {m.name} in your shop by (4).");
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class Mirrordome : Upgrade
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

            for (int i = 0; i < gameHandler.players[enemy].shop.LastIndex; i++)
            {
                gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[enemy].shop.At(i));
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Mirrordome replaced your shop with a copy of {gameHandler.players[enemy].name}'s shop.");
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class HatChucker8000 : Upgrade
    {
        private Rarity chosenRarity = Rarity.NO_RARITY;
        public HatChucker8000()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Hat Chucker 8000";
            this.cardText = "Battlecry: Name a Rarity. Aftermath: Give all players' Upgrades of that rarity +2/+2.";
            this.SetStats(3, 3, 3);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            var prompt = new PlayerInteraction("Name a Rarity", "Common, Rare, Epic or Legendary", "Capitalisation is ignored", AnswerType.StringAnswer);
            List<string> rarities = new List<string>(){ "common", "rare", "epic", "legendary"};
            string defaultAns = rarities[GameHandler.randomGenerator.Next(4)];

            string ret = await prompt.SendInteractionAsync(curPlayer, (x, y, z) => rarities.Contains(x.ToLower()), defaultAns);

            if (ret.Equals("common", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Common;
            else if (ret.Equals("rare", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Rare;
            else if (ret.Equals("epic", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Epic;
            else if (ret.Equals("legendary", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Legendary;
            else this.chosenRarity = Rarity.NO_RARITY;

            this.writtenEffect = $"Aftermath: Give all players' {this.chosenRarity} Upgrades in their shops +2/+2.";
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgrades();

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

            for (int j = 0; j < gameHandler.players.Count(); j++)
            {
                if (j == curPlayer) continue;
                if (gameHandler.players[j].lives <= 0) continue;

                upgrades = gameHandler.players[j].shop.GetAllUpgrades();

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


    //[TokenAttribute]
    //public class IronmoonTicket : Spell
    //{
    //    public int value = 1;
    //    public IronmoonTicket()
    //    {
    //        this.rarity = SpellRarity.Spell;
    //        this.name = "Ironmoon Ticket";
    //        this.cost = 0;
    //        this.UpdateCardText();
    //    }

    //    private void UpdateCardText()
    //    {
    //        this.cardText = $"Gain +{value}/+{value}. Gain {value} Mana this turn only.";
    //    }

    //    public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
    //    {
    //        gameHandler.players[curPlayer].creatureData.attack += value;
    //        gameHandler.players[curPlayer].creatureData.health += value;
    //        gameHandler.players[curPlayer].curMana += value;
    //    }

    //    public override string GetInfo(GameHandler gameHandler, int player)
    //    {
    //        this.UpdateCardText();
    //        return base.GetInfo(gameHandler, player);
    //    }
    //    public override Card DeepCopy()
    //    {
    //        IronmoonTicket ret =  (IronmoonTicket)base.DeepCopy();
    //        ret.value = this.value;
    //        return ret;
    //    }
    //}

    [TokenAttribute]
    public class IronmoonTicket : Spell
    {
        public IronmoonTicket()
        {
            this.rarity = SpellRarity.Spell;
            this.name = "Ironmoon Ticket";
            this.cardText = "Gain +1/+1 and 1 Mana for each Ironmoon Ticket you're holding.";
            this.Cost = 0;
        }

        public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int amountOfTickets = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].hand.GetAllCards(), x => x.name == this.name).Count();

            gameHandler.players[curPlayer].creatureData.attack += amountOfTickets+1;
            gameHandler.players[curPlayer].creatureData.health += amountOfTickets+1;
            gameHandler.players[curPlayer].curMana += amountOfTickets+1;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> amountOfTickets = CardsFilter.FilterList<Card>(gameHandler.players[player].hand.GetAllCards(), x => x.name == this.name);

            return base.GetInfo(gameHandler, player) + $" *({amountOfTickets.Count()})*";
        }
    }

    public class SilasIronmoonEffect : Upgrade
    {
        public SilasIronmoonEffect()
        {
            //this.writtenEffect = "Permanent Aftermath: Increase your Tickets's bonuses by 1.";   
            this.writtenEffect = "Permanent Aftermath: Add a Ticket to your hand. It gives you +1/+1 and 1 Mana for each Ticket you're holding.";

        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            //List<Card> handCards = gameHandler.players[curPlayer].hand.GetAllCards();

            //foreach (var card in handCards)
            //{
            //    if (card.GetType() == typeof(IronmoonTicket))
            //    {
            //        ((IronmoonTicket)card).value++;
            //    }
            //}
            Card token = new IronmoonTicket();

            gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.FindBasicCard(token.name));
            gameHandler.players[curPlayer].nextRoundEffects.Add(new SilasIronmoonEffect());
            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Silas Ironmoon adds an Ironmoon Ticket to your hand.");
        }
     
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.IronmoonFaire)]
    public class SilasIronmoon : Upgrade
    {
        public SilasIronmoon()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Silas Ironmoon";
            //this.cardText = "Battlecry: Add a Ticket to your hand that gives your Mech +1/+1 and 1 Mana. Permanent Aftermath: Increase your Tickets's bonuses by 1.";
            this.cardText = "Permanent Aftermath: Add a Ticket to your hand. It gives you +1/+1 and 1 Mana for each Ticket you're holding.";
            this.SetStats(7, 4, 4);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            //gameHandler.players[curPlayer].hand.AddCard(new IronmoonTicket());
            this.extraUpgradeEffects.Add(new SilasIronmoonEffect());
            return Task.CompletedTask;
        }
    }
}
