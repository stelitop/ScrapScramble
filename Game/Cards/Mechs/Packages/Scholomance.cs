using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class Tinkerpet : Mech
    {
        private bool spellburst = true;

        public Tinkerpet()
        {
            this.rarity = Rarity.Common;
            this.name = "Tinkerpet";
            this.cardText = this.writtenEffect = "Spellburst: Give your Mech +4 Spikes and +4 Shields.";
            this.SetStats(2, 1, 1);
        }

        public override void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
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

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class OneHitWonder : Mech
    {
        public OneHitWonder()
        {
            this.rarity = Rarity.Common;
            this.name = "One Hit Wonder";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +8 Attack.";
            this.SetStats(7, 1, 5);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 8;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s One Hit Wonder gives it +8 Attack, leaving it with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class SurveillanceBird : Mech
    {
        public SurveillanceBird()
        {
            this.rarity = Rarity.Common;
            this.name = "Surveillance Bird";
            this.cardText = this.writtenEffect = "Aftermath: Gain 2 Mana this turn only.";
            this.SetStats(3, 2, 2);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 2;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Surveillance Bird gave you +2 Mana this turn only.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class OnyxCrowbot : Mech
    {
        public OnyxCrowbot()
        {
            this.rarity = Rarity.Common;
            this.name = "Onyx Crowbot";
            this.cardText = this.writtenEffect = "Aftermath: Gain 4 Mana this turn only.";
            this.SetStats(5, 4, 4);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 4;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Onyx Crowbot gave you +4 Mana this turn only.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class SulfurNanoPhoenix : Mech
    {
        public SulfurNanoPhoenix()
        {
            this.rarity = Rarity.Common;
            this.name = "Sulfur Nano-Phoenix";
            this.cardText = this.writtenEffect = "Aftermath: Gain 6 Mana this turn only.";
            this.SetStats(7, 6, 6);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 6;
            gameHandler.players[curPlayer].aftermathMessages.Add("Your Sulfur Nano-Phoenix gave you +6 Mana this turn only.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class LivewireBramble : Mech
    {
        public LivewireBramble()
        {
            this.rarity = Rarity.Rare;
            this.name = "Livewire Bramble";
            this.cardText = this.writtenEffect = "Aftermath: Replace two random Upgrades in your shop with Livewire Brambles.";
            this.SetStats(0, 2, 1);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<int> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

            if (upgrades.Count() <= 2)
            {
                for (int i = 0; i < upgrades.Count(); i++)
                {
                    gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[i], new LivewireBramble());
                }
            }
            else
            {
                int pos1, pos2;
                pos1 = GameHandler.randomGenerator.Next(0, upgrades.Count());
                pos2 = GameHandler.randomGenerator.Next(0, upgrades.Count() - 1);
                if (pos2 >= pos1) pos2++;

                gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[pos1], new LivewireBramble());
                gameHandler.players[curPlayer].shop.TransformUpgrade(upgrades[pos2], new LivewireBramble());
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Livewire Bramble replace two Upgrades in your shop with Livewire Brambles.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class TightropeChampion : Mech
    {
        public TightropeChampion()
        {
            this.rarity = Rarity.Rare;
            this.name = "Tightrope Champion";
            this.cardText = this.writtenEffect = "Start of Combat: If your Mech's Attack is equal to its Health, gain +2/+2.";
            this.SetStats(4, 4, 4);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
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
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class CobaltConqueror : Mech
    {
        private bool comboTrig = false;
        private bool effectTrig = false;

        public CobaltConqueror()
        {
            this.rarity = Rarity.Rare;
            this.name = "Cobalt Conqueror";
            this.cardText = "Rush. Combo: Give the next Upgrade you buy this turn Rush.";
            this.printEffectInCombat = false;
            this.SetStats(10, 9, 7);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.effectTrig = false;
        }

        public override void Combo(GameHandler gameHandler, int curPlayer, int enemy)
        {
            this.writtenEffect = "Give the next Upgrade you buy this turn Rush.";
            this.comboTrig = true;
        }

        public override void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (this.comboTrig && !this.effectTrig)
            {
                this.effectTrig = true;
                this.writtenEffect = string.Empty;
                m.creatureData.staticKeywords[StaticKeyword.Rush]++;
            }
        }

        public override Card DeepCopy()
        {
            CobaltConqueror ret = (CobaltConqueror)base.DeepCopy();
            ret.comboTrig = this.comboTrig;
            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
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
    [Package(UpgradePackage.ScholomanceAcademy)]
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
            Bibliobot ret = (Bibliobot)base.DeepCopy();

            ret.letter = this.letter;
            return ret;
        }
    }

    [TokenAttribute]    
    public class LightningBloom : Spell
    {
        public LightningBloom()
        {
            this.rarity = SpellRarity.Spell;
            this.name = "Lightning Bloom";
            this.cardText = "Gain 2 Mana this turn only. Overload: (2).";
            this.cost = 0;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Overload] += 2;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.ScholomanceAcademy)]
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
    [Package(UpgradePackage.ScholomanceAcademy)]
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
    [Package(UpgradePackage.ScholomanceAcademy)]
    public class LordBarox : Mech
    {
        int bet = -1;

        public LordBarox()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lord Barox";
            this.cardText = "Battlecry: Name ANY other Mech. Aftermath: If it won last round, gain 5 Mana this turn only.";
            this.SetStats(3, 3, 2);
            this.printEffectInCombat = false;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.AlivePlayers() <= 1) return;

            var prompt = new PlayerInteraction("Name a Player", "The name needs to be exactly written.", "Capitalisation is ignored", AnswerType.StringAnswer);

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

                for (int i = 0; i < gameHandler.players.Count(); i++)
                {
                    if (gameHandler.players[i].name.Equals(res, StringComparison.OrdinalIgnoreCase))
                    {
                        if (gameHandler.players[i].lives <= 0) break;
                        if (i == curPlayer) break;
                        this.bet = i;
                        break;
                    }
                }

                if (this.bet == -1) continue;

                this.writtenEffect = $"You have bet on {gameHandler.players[this.bet].name}! If they win their next fight, you'll gain 5 Mana next turn.";
                this.printEffectInCombat = false;

                break;
            }
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (this.bet == -1)
                if (gameHandler.pairsHandler.playerResults.Count() < 2) return;

            if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count() - 2][this.bet] == FightResult.WIN)
            {
                gameHandler.players[curPlayer].curMana += 5;
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    $"The bet on your Lord Barox for {gameHandler.players[this.bet].name} was correct! You gain 5 Mana this turn only.");
            }
            else
            {
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    $"The bet on your Lord Barox for {gameHandler.players[this.bet].name} was incorrect.");
            }
        }

        public override Card DeepCopy()
        {
            LordBarox ret = (LordBarox)base.DeepCopy();
            ret.bet = this.bet;
            return ret;
        }
    }
}