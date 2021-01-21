using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class GoldBolts : Mech
    {
        public GoldBolts()
        {
            this.rarity = Rarity.Rare;
            this.name = "Gold Bolts";
            this.cardText = "Battlecry: Transform your Shields into Health.";
            this.creatureData = new CreatureData(3, 3, 2);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] = 0;
        }
    }

    [UpgradeAttribute]
    public class WupallSmasher : Mech
    {
        public WupallSmasher()
        {
            this.rarity = Rarity.Rare;
            this.name = "Wupall Smasher";
            this.cardText = "Battlecry: Transform your Spikes into Attack.";
            this.creatureData = new CreatureData(5, 4, 5);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] = 0;
        }
    }

    [UpgradeAttribute]
    public class TightropeChampion : Mech
    {
        public TightropeChampion()
        {
            this.rarity = Rarity.Rare;
            this.name = "Tightrope Champion";
            this.cardText = this.writtenEffect = "Start of Combat: If your Mech's Attack is equal to its Health, gain +2/+2.";
            this.creatureData = new CreatureData(4, 4, 4);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.attack == gameHandler.players[curPlayer].creatureData.health)
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
                gameHandler.players[curPlayer].creatureData.health += 2;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Tighrope Champion triggers and gives it +2/+2, leaving it as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Tighrope Champion failed to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    public class CarbonCarapace : Mech
    {
        public CarbonCarapace()
        {
            this.rarity = Rarity.Rare;
            this.name = "Carbon Carapace";
            this.cardText = this.writtenEffect = "Start of Combat: Gain Shields equal to the last digit of the enemy Mech's Attack.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[enemy].creatureData.attack % 10;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Carbon Carapace gives it +{gameHandler.players[enemy].creatureData.attack % 10} Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
        }
    }

    [UpgradeAttribute]
    public class CopperplatedPrince : Mech
    {
        public CopperplatedPrince()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Prince";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Health for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 3, 1);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Prince gives it +{2 * gameHandler.players[curPlayer].curMana} Health, leaving it with {gameHandler.players[curPlayer].creatureData.health} Health.");
        }
    }

    [UpgradeAttribute]
    public class CopperplatedPrincess : Mech
    {
        public CopperplatedPrincess()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copperplated Princess";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +2 Attack for each unspent Mana you have.";
            this.creatureData = new CreatureData(3, 1, 3);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2 * gameHandler.players[curPlayer].curMana;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copperplated Princess gives it +{2 * gameHandler.players[curPlayer].curMana} Attack, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    public class HomingMissile : Mech
    {
        public HomingMissile()
        {
            this.rarity = Rarity.Rare;
            this.name = "Homing Missile";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Health of your opponent's Mech by 5 (but not below 1).";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.health -= 5;
            if (gameHandler.players[enemy].creatureData.health < 1) gameHandler.players[enemy].creatureData.health = 1;
            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Homing Missile reduced your Mech's Health to {gameHandler.players[enemy].creatureData.health}.");
        }
    }

    [UpgradeAttribute]
    public class TwilightDrone : Mech
    {
        public TwilightDrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Twilight Drone";
            this.cardText = "Battlecry: Give your Mech +1/+1 for each card in your hand.";
            this.creatureData = new CreatureData(4, 2, 4);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].hand.cards.Count();
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].hand.cards.Count();
        }
    }

    [UpgradeAttribute]
    public class OffbrandShoe : Mech
    {
        public OffbrandShoe()
        {
            this.rarity = Rarity.Rare;
            this.name = "Offbrand Shoe";
            this.cardText = this.writtenEffect = "Aftermath: Deal 6 damage to your Mech.";
            this.creatureData = new CreatureData(1, 0, 6);
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health -= 6;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Offbrand Shoe deals 6 damage to you.");
        }
    }

    [UpgradeAttribute]
    public class Hypnodrone : Mech
    {
        public Hypnodrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Hypnodrone";
            this.cardText = this.writtenEffect = "Aftermath: Reduce the Attack of your opponent's Mech by 5 (but not below 1).";
            this.creatureData = new CreatureData(7, 6, 6);
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[enemy].creatureData.attack -= 5;
            if (gameHandler.players[enemy].creatureData.attack < 1) gameHandler.players[enemy].creatureData.attack = 1;
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Hypnodrone reduced your Mech's Attack by 5, leaving it with {gameHandler.players[enemy].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    public class MkIVSuperCobra : Mech
    {
        public MkIVSuperCobra()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mk. IV Super Cobra";
            this.cardText = "Rush. Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.writtenEffect = "Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.creatureData = new CreatureData(6, 5, 2);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            if (gameHandler.players[enemy].shop.options.Count() == 0) return;
            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[enemy].shop.options.Count());
            gameHandler.players[enemy].shop.options.RemoveAt(shop);
            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Mk. IV Super Cobra destroyed a random upgarde in your shop.");
        }
    }

    [UpgradeAttribute]
    public class LivewireBramble : Mech
    {
        public LivewireBramble()
        {
            this.rarity = Rarity.Rare;
            this.name = "Livewire Bramble";
            this.cardText = this.writtenEffect = "Aftermath: Replace two random Upgrades in your shop with Livewire Brambles.";
            this.creatureData = new CreatureData(0, 2, 1);
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() <= 2)
            {
                for (int i = 0; i < gameHandler.players[curPlayer].shop.options.Count(); i++)
                {
                    gameHandler.players[curPlayer].shop.options[i] = new LivewireBramble();
                }
            }
            else
            {
                int pos1, pos2;
                pos1 = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count());
                pos2 = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() - 1);
                if (pos2 >= pos1) pos2++;

                gameHandler.players[curPlayer].shop.options[pos1] = new LivewireBramble();
                gameHandler.players[curPlayer].shop.options[pos2] = new LivewireBramble();
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Livewire Bramble replace two Upgrades in your shop with Livewire Brambles.");
        }
    }

    [UpgradeAttribute]
    public class PeekABot : Mech
    {
        public PeekABot()
        {
            this.rarity = Rarity.Rare;
            this.name = "Peek-a-Bot";
            this.cardText = this.writtenEffect = "Aftermath: You are told the most expensive Upgrade in your opponent's shop.";
            this.creatureData = new CreatureData(1, 1, 1);
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            List<int> highestCosts = new List<int>();
            int maxCost = -1;
            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (maxCost < gameHandler.players[enemy].shop.options[i].creatureData.cost) maxCost = gameHandler.players[enemy].shop.options[i].creatureData.cost;
            }

            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.options[i].creatureData.cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Peek-a-Bot tells you the most expensive Upgrade in your opponent's shop is {gameHandler.players[enemy].shop.options[pos].name}");
        }
    }

    [UpgradeAttribute]
    public class LightningWeasel : Mech
    {
        public LightningWeasel()
        {
            this.rarity = Rarity.Rare;
            this.name = "Lightning Weasel";
            this.cardText = this.writtenEffect = "Aftermath: Replace the highest-Cost Upgrade in your opponent's shop with a Lightning Weasel.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            List<int> highestCosts = new List<int>();
            int maxCost = -1;
            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (maxCost < gameHandler.players[enemy].shop.options[i].creatureData.cost) maxCost = gameHandler.players[enemy].shop.options[i].creatureData.cost;
            }

            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.options[i].creatureData.cost == maxCost) highestCosts.Add(i);
            }

            int pos = GameHandler.randomGenerator.Next(0, highestCosts.Count());

            gameHandler.players[enemy].shop.options[pos] = new LightningWeasel();
            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Lightning Weasel replaced your highest-Cost Upgrade with a Lightning Weasel.");
        }
    }

    [UpgradeAttribute]
    public class SocietyProgressor : Mech
    {
        public SocietyProgressor()
        {
            this.rarity = Rarity.Rare;
            this.name = "Society Progressor";
            this.cardText = this.writtenEffect = "Aftermath: Remove Binary from all Upgrades in your opponent's shop.";
            this.creatureData = new CreatureData(4, 1, 6);
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            for (int i = 0; i < gameHandler.players[enemy].shop.options.Count(); i++)
            {
                if (gameHandler.players[enemy].shop.options[i].creatureData.staticKeywords[StaticKeyword.Binary] > 0)
                {
                    gameHandler.players[enemy].shop.options[i].creatureData.staticKeywords[StaticKeyword.Binary] = 0;
                    gameHandler.players[enemy].shop.options[i].cardText += "(No Binary)";
                }
            }

            gameHandler.players[enemy].aftermathMessages.Add(
                $"{gameHandler.players[curPlayer].name}'s Society Progressor removed Binary from all Upgrades in your shop.");
        }
    }

    [UpgradeAttribute]
    public class SiliconGrenadeBelt : Mech
    {
        public SiliconGrenadeBelt()
        {
            this.rarity = Rarity.Rare;
            this.name = "Silicon Grenade Belt";
            this.cardText = this.writtenEffect = "Start of Combat: Deal 1 damage to the enemy Mech, twice.";
            this.creatureData = new CreatureData(4, 4, 2);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].TakeDamage(1, ref gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
            gameHandler.players[enemy].TakeDamage(1, ref gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
        }
    }

    [UpgradeAttribute]
    public class ScrapStacker : Mech
    {
        public ScrapStacker()
        {
            this.rarity = Rarity.Rare;
            this.name = "Scrap Stacker";
            this.cardText = this.writtenEffect = "After you buy another Upgrade, gain +2/+2.";
            this.printEffectInCombat = false;
            this.creatureData = new CreatureData(8, 4, 4);
        }

        public override void OnBuyingAMech(Mech m, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2;
            gameHandler.players[curPlayer].creatureData.health += 2;
        }
    }

    [UpgradeAttribute]
    public class PacifisticRecruitomatic : Mech
    {
        public PacifisticRecruitomatic()
        {
            this.rarity = Rarity.Rare;
            this.name = "Pacifistic Recruitomatic";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 random 0-Attack Upgrades to your shop.";
            this.creatureData = new CreatureData(2, 0, 3);
        }

        private bool Criteria(Mech m)
        {
            if (m.creatureData.attack == 0) return true;
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
                "Your Pacifistic Recruitomatic adds 3 random 0-Attack Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    public class ElectricBoogaloo : Mech
    {
        public ElectricBoogaloo()
        {
            this.rarity = Rarity.Rare;
            this.name = "Electric Boogaloo";
            this.cardText = "Echo. Aftermath: Give a random Upgrade in your shop +4 Attack.";
            this.writtenEffect = "Aftermath: Give a random Upgrade in your shop +4 Attack.";
            this.creatureData = new CreatureData(3, 1, 4);
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count());
            gameHandler.players[curPlayer].shop.options[shop].creatureData.attack += 4;

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Electric Boogaloo gave the {gameHandler.players[curPlayer].shop.options[shop].name} in your shop +4 Attack.");
        }
    }

    [UpgradeAttribute]
    public class Pacerager : Mech
    {
        public Pacerager()
        {
            this.rarity = Rarity.Rare;
            this.name = "Pacerager";
            this.cardText = "Rush x2. After this takes damage, destroy it.";
            this.writtenEffect = "After this takes damage, destroy it";
            this.creatureData = new CreatureData(3, 5, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 2;
        }

        public override void AfterThisTakesDamage(int damage, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].destroyed = true;
            gameHandler.combatOutputCollector.combatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Pacerager triggers, destroying {gameHandler.players[curPlayer].name}.");
        }
    }

    [UpgradeAttribute]
    public class PrismaticReflectotron : Mech
    {
        private bool triggered;
        public PrismaticReflectotron()
        {
            this.rarity = Rarity.Rare;
            this.name = "Prismatic Reflectotron";
            this.cardText = this.writtenEffect = "After your Mech takes damage for the first time, deal the same amount to the enemy Mech.";
            this.creatureData = new CreatureData(6, 2, 2);
            this.triggered = false;
        }

        public override void AfterThisTakesDamage(int damage, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!this.triggered)
            {
                this.triggered = true;
                gameHandler.players[enemy].TakeDamage(damage, ref gameHandler, curPlayer, enemy,
                    $"{gameHandler.players[curPlayer].name}'s Prismatic Reflectotron triggers, dealing {damage} damage, ");
            }
        }
    }

    [UpgradeAttribute]
    public class RoboRabbit : Mech
    {
        public RoboRabbit()
        {
            this.rarity = Rarity.Rare;
            this.name = "Robo-Rabbit";
            this.cardText = "Battlecry: Gain +2/+2 for each other Robo-Rabbit you've played this game.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        private bool Criteria(Card m)
        {
            return m.name.Equals("Robo-Rabbit");
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, this.Criteria);

            gameHandler.players[curPlayer].creatureData.attack += 2 * list.Count();
            gameHandler.players[curPlayer].creatureData.health += 2 * list.Count();
        }

        public override string GetInfo(ref GameHandler gameHandler, int player)
        {
            return base.GetInfo(ref gameHandler, player) + $" *({CardsFilter.FilterList<Card>(ref gameHandler.players[player].playHistory, Criteria).Count})*";
        }
    }

    [UpgradeAttribute]
    public class Autobalancer : Mech
    {
        public Autobalancer()
        {
            this.rarity = Rarity.Rare;
            this.name = "Autobalancer";
            this.cardText = this.writtenEffect = "Start of Combat: The player who bought fewer Upgrades last turn gives their Mech +2/+2. If tied, gain +1/+1.";
            this.creatureData = new CreatureData(4, 4, 4);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
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
    public class PlatedBeetleDrone : Mech
    {
        private int attacks;
        public PlatedBeetleDrone()
        {
            this.rarity = Rarity.Rare;
            this.name = "Plated Beetle Drone";
            this.cardText = this.writtenEffect = "The first time your Mech takes damage, gain +3 Shields. The second time, gain +2. The third, +1.";
            this.creatureData = new CreatureData(4, 2, 3);
            this.attacks = 0;
        }

        public override void AfterThisTakesDamage(int damage, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            attacks++;
            if (attacks < 4)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += (4 - attacks);
                gameHandler.combatOutputCollector.combatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Plated Beetle Drone triggers, giving it +{4 - attacks} Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
            }
        }
    }

    [UpgradeAttribute]
    public class ByteBarker : Mech
    {
        public ByteBarker()
        {
            this.rarity = Rarity.Rare;
            this.name = "Byte Barker";
            this.cardText = "Binary. Choose One - Gain +6 Spikes; or +6 Shields.";
            this.creatureData = new CreatureData(6, 4, 4);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }
        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
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

    [UpgradeAttribute]
    public class ThreeDPrinter : Mech
    {
        public ThreeDPrinter()
        {
            this.rarity = Rarity.Rare;
            this.name = "3D Printer";
            this.cardText = "Battlecry: Name an Upgrade in the game. Add a copy of it to your shop.";
            this.creatureData = new CreatureData(5, 0, 7);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
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
                            gameHandler.players[curPlayer].shop.options.Add((Mech)gameHandler.pool.mechs[i].DeepCopy());
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
    public class ArcaneAutomatron : Mech
    {
        public ArcaneAutomatron()
        {
            this.rarity = Rarity.Rare;
            this.name = "Arcane Automatron";
            this.cardText = "Buying this Upgrade also counts as casting a spell.";
            this.creatureData = new CreatureData(2, 1, 3);
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            for (int i = 0; i < gameHandler.players[curPlayer].attachedMechs.Count(); i++)
            {
                gameHandler.players[curPlayer].attachedMechs[i].OnSpellCast(this, ref gameHandler, curPlayer, enemy);
            }
        }
    }

    [UpgradeAttribute]
    public class AnonymousSupplier : Mech
    {
        public AnonymousSupplier()
        {
            this.rarity = Rarity.Rare;
            this.name = "Anonymous Supplier";
            this.cardText = this.writtenEffect = "Your Upgrades are not shared in the combat log.";
            this.creatureData = new CreatureData(3, 3, 3);
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.hideUpgradesInLog = true;
        }
    }

    [UpgradeAttribute]
    public class TrickRoomster : Mech
    {
        public TrickRoomster()
        {
            this.rarity = Rarity.Rare;
            this.name = "Trick Roomster";
            this.cardText = this.writtenEffect = "The Mech with the lower Attack Priority goes first instead.";
            this.creatureData = new CreatureData(4, 1, 1);
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.invertAttackPriority = true;
        }
    }

    [UpgradeAttribute]
    public class BrassBracer : Mech
    {
        public BrassBracer()
        {
            this.rarity = Rarity.Rare;
            this.name = "Brass Bracer";
            this.cardText = this.writtenEffect = "This minion ignores damage from Spikes.";
            this.creatureData = new CreatureData(5, 3,4);
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.ignoreSpikes = true;
        }
    }

    [UpgradeAttribute]
    public class RadiatingCrucible : Mech
    {
        public RadiatingCrucible()
        {
            this.rarity = Rarity.Rare;
            this.name = "Radiating Crucible";
            this.cardText = this.writtenEffect = "This minion's attacks ignore Shields.";
            this.creatureData = new CreatureData(5, 4, 3);
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.ignoreShields = true;
        }
    }
}

/*

[UpgradeAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Rare;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/
