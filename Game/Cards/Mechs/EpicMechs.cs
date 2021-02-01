using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [UpgradeAttribute]
    public class Naptron : Mech
    {
        public Naptron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Naptron";
            this.cardText = "Taunt x2. Aftermath: Give your Mech Rush x2.";
            this.writtenEffect = "Aftermath: Give your Mech Rush x2.";
            this.SetStats(4, 1, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] += 2;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Naptron gives you Rush x2.");
        }
    }

    [UpgradeAttribute]
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
    public class FallenReaver : Mech
    {
        public FallenReaver()
        {
            this.rarity = Rarity.Epic;
            this.name = "Fallen Reaver";
            this.cardText = this.writtenEffect = "Aftermath: Destroy 6 random Upgrades in your shop.";
            this.SetStats(5, 8, 8);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {

            for (int i=0; i<6; i++)
            {
                if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) break;

                int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
                Console.WriteLine(index);
                gameHandler.players[curPlayer].shop.RemoveUpgrade(index);
            }
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Fallen Reaver destroys 6 random Upgrades in your shop.");
        }
    }

    [UpgradeAttribute]
    public class Investrotron : Mech
    {
        public Investrotron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Investotron";
            this.cardText = this.writtenEffect = "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Mech +4/+4.";
            this.SetStats(5, 4, 4);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            int index = gameHandler.players[curPlayer].shop.GetRandomUpgradeIndex();
            gameHandler.players[curPlayer].shop.TransformUpgrade(index, new Investrotron());
            gameHandler.players[curPlayer].creatureData.attack += 4;
            gameHandler.players[curPlayer].creatureData.health += 4;

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Investotron transforms a random Upgrade in your shop into an Investotron and gives you +4/+4, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
        }
    }

    [UpgradeAttribute]
    public class MassAccelerator : Mech
    {
        public MassAccelerator()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mass Accelerator";
            this.cardText = this.writtenEffect = "Start of Combat: If you're Overloaded, deal 5 damage to the enemy Mech.";
            this.SetStats(6, 5, 5);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] > 0 || gameHandler.players[curPlayer].overloaded > 0)
            {
                gameHandler.players[enemy].TakeDamage(5, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Mass Accelerator triggers and deals 5 damage, ");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Mass Accelerator fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
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
    public class Bibliobot : Mech
    {
        private string letter = "  ";

        public Bibliobot()
        {
            this.rarity = Rarity.Epic;
            this.name = "Bibliobot";
            this.cardText = "Battlecry: Name a letter. This round, after you buy an Upgrade that starts with that letter, gain +2 Attack.";
            this.writtenEffect = "Not Frog";
            this.SetStats(5, 5, 3);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            var playerInteraction = new PlayerInteraction("Name a Letter", string.Empty, "Capitalisation is ignored", AnswerType.CharAnswer);

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
            
                letter = res;

                this.writtenEffect = $"After you buy an Upgrade that starts with the letter '{letter}', gain +2 Attack.";
                this.printEffectInCombat = false;

                break;
            }
            Console.WriteLine(letter);
        }

        public override void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            Console.WriteLine(m.name[0] + " " + this.letter);
            if (m.name.StartsWith(this.letter, StringComparison.OrdinalIgnoreCase))
            {
                gameHandler.players[curPlayer].creatureData.attack += 2;
            }
        }

        public override Card DeepCopy()
        {
            Bibliobot ret = (Bibliobot)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.creatureData = this.creatureData.DeepCopy();
            ret.writtenEffect = this.writtenEffect;
            ret.letter = this.letter;
            return ret;
        }
    }

    [UpgradeAttribute]
    public class Mirrordome : Mech
    {
        public Mirrordome()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mirrordome";
            this.cardText = this.writtenEffect = "Aftermath: This turn, your shop is a copy of your opponent's.";
            this.SetStats(4, 0, 8);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            gameHandler.players[curPlayer].shop.Clear();

            for (int i=0; i<gameHandler.players[enemy].shop.totalSize; i++)
            {
                gameHandler.players[curPlayer].shop.AddUpgrade(gameHandler.players[enemy].shop.At(i));
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Mirrordome replaced your shop with a copy of {gameHandler.players[enemy].name}'s shop.");
        }
    }

    [UpgradeAttribute]
    public class SuperScooper : Mech
    {
        public SuperScooper()
        {
            this.rarity = Rarity.Epic;
            this.name = "Super Scooper";
            this.cardText = this.writtenEffect = "Start of Combat: Steal the stats of the lowest-Cost Upgrade your opponent bought last turn from their Mech.";
            this.SetStats(8, 3, 7);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int lowestCost = 9999;
            List<Mech> upgradesList = new List<Mech>();

            for (int i=0; i<gameHandler.players[enemy].boughtThisTurn.Count(); i++)
            {
                if (gameHandler.players[enemy].boughtThisTurn[i].cost < lowestCost && gameHandler.players[enemy].boughtThisTurn[i].creatureData.attack != 0 && gameHandler.players[enemy].boughtThisTurn[i].creatureData.health != 0)
                {
                    lowestCost = gameHandler.players[enemy].boughtThisTurn[i].cost;
                    upgradesList.Clear();
                    upgradesList.Add(gameHandler.players[enemy].boughtThisTurn[i]);
                }
            }
            
            if (upgradesList.Count == 0)
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Super Scooper triggers but doesn't do anything.");
                return;
            }

            int pos = GameHandler.randomGenerator.Next(0, upgradesList.Count());

            int attackst = upgradesList[pos].creatureData.attack;
            int healthst = upgradesList[pos].creatureData.health;

            upgradesList[pos].creatureData.attack = 0;
            upgradesList[pos].creatureData.health = 0;

            gameHandler.players[curPlayer].creatureData.attack += attackst;
            gameHandler.players[curPlayer].creatureData.health += healthst;

            gameHandler.players[enemy].creatureData.attack -= attackst;
            gameHandler.players[enemy].creatureData.health -= healthst;

            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Super Scooper steals {attackst}/{healthst} from {gameHandler.players[enemy].name}'s {upgradesList[pos].name}, " +
                $"leaving it as a {gameHandler.players[enemy].creatureData.Stats()} and leaving {gameHandler.players[curPlayer].name} as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
        }
    }

    [UpgradeAttribute]
    public class TitaniumBloomer : Mech
    {
        public TitaniumBloomer()
        {
            this.rarity = Rarity.Epic;
            this.name = "Titanium Bloomer";
            this.cardText = "Battlecry: Add a Lightning Bloom to your hand.";
            this.SetStats(4, 4, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].hand.AddCard(new LightningBloom());
        }
    }

    [UpgradeAttribute]
    public class SpellPrinter : Mech
    {
        private bool spellburst = true;

        public SpellPrinter()
        {
            this.rarity = Rarity.Epic;
            this.name = "Spell Printer";
            this.cardText = this.writtenEffect = "Spellburst: Add a copy of the spell to your hand.";
            this.SetStats(5, 4, 5);
        }

        public override void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (spellburst)
            {
                spellburst = false;
                this.writtenEffect = string.Empty;

                gameHandler.players[curPlayer].hand.AddCard(spell);
            }
        }
    }

    [UpgradeAttribute]
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

            for (int i=0; i<3; i++)
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
    public class DungeonDragonling : Mech
    {
        public DungeonDragonling()
        {
            this.rarity = Rarity.Epic;
            this.name = "Dungeon Dragonling";
            this.cardText = this.writtenEffect = "Whenever you would take damage, roll a d20. You take that much less damage.";
            this.SetStats(20, 4, 12);
        }

        public override void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg)
        {
            int red = GameHandler.randomGenerator.Next(1, 21);
            damage -= red;
            if (damage < 0) damage = 0;
            msg += $"reduced to {damage} by Dungeon Dragonling(rolled {red}), ";
        }
    }

    [UpgradeAttribute]
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
}

/*

[UpgradeAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Epic;
        this.name = "";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

*/