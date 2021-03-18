using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{       
    public class VentureCo
    {
        public static bool Criteria(Card m)
        {
            return m.name.Contains("Venture Co.");
        }
    }
    public class VentureCoMechNaming : Upgrade
    {
        public override string GetInfo(GameHandler gameHandler, int player)
        {
            return base.GetInfo(gameHandler, player) + $" *({CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, VentureCo.Criteria).Count})*";
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoSticker : Upgrade
    {
        public VentureCoSticker()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Sticker";
            this.cardText = string.Empty;
            this.SetStats(1, 0, 2);
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoSawblade : VentureCoMechNaming
    {
        public VentureCoSawblade()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Sawblade";
            this.cardText = "Battlecry: Gain +1 Attack for each Venture Co. Upgrade you've bought this game.";
            this.SetStats(3, 1, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();

            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoPauldrons : VentureCoMechNaming
    {
        public VentureCoPauldrons()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Pauldrons";
            this.cardText = "Taunt. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.";
            this.SetStats(4, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();
            gameHandler.players[curPlayer].creatureData.health += list.Count();

            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoThrusters : VentureCoMechNaming
    {
        public VentureCoThrusters()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Thrusters";
            this.cardText = "Rush. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.";
            this.SetStats(6, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();
            gameHandler.players[curPlayer].creatureData.health += list.Count();

            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoFlamethrower : VentureCoMechNaming
    {
        public VentureCoFlamethrower()
        {
            this.rarity = Rarity.Epic;
            this.name = "Venture Co. Flamethrower";
            this.cardText = this.writtenEffect = "Start of Combat: Deal 2 damage to the enemy Mech for each Venture Co. Upgrade you've played this game.";
            this.SetStats(4, 2, 2);            
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);

            gameHandler.players[enemy].TakeDamage(2*list.Count(), gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Venture Co. Flamethrower deals {2*list.Count()} damage, ");
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoVault : Upgrade
    {
        public VentureCoVault()
        {
            this.rarity = Rarity.Rare;
            this.name = "Venture Co. Vault";
            this.cardText = "Taunt. Aftermath: Add 3 other random Venture Co. Upgrades to your shop.";
            this.writtenEffect = "Aftermath: Add 3 other random Venture Co. Upgrades to your shop.";
            this.SetStats(3, 0, 6);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, VentureCo.Criteria);

            for (int i=list.Count()-1; i>=0; i--)
            {
                if (list[i].name.Equals(this.name))
                {
                    list.RemoveAt(i);
                    break;
                }
            }

            for (int i=0; i<3; i++)
            {
                int card = GameHandler.randomGenerator.Next(0, list.Count());

                gameHandler.players[curPlayer].shop.AddUpgrade(list[card]);
            }
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoCoolant : Upgrade
    {
        public VentureCoCoolant()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Coolant";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it -4 Attack. Overload: (1).";
            this.SetStats(2, 2, 3);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.OptionsCount() == 0) return;

            Upgrade chosen = await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy);

            chosen.creatureData.attack -= 4;
            if (chosen.creatureData.attack < 0) chosen.creatureData.attack = 0;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class SponsorshipScrubber : Upgrade
    {
        public SponsorshipScrubber()
        {
            this.rarity = Rarity.Epic;
            this.name = "Sponsorship Scrubber";
            this.cardText = this.writtenEffect = "Start of Combat: If your opponent has purchased a Venture Co. Upgrade this game, steal 6 Attack from their Upgrade.";
            this.SetStats(3, 1, 2);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[enemy].playHistory, VentureCo.Criteria);

            if (list.Count() > 0)
            {
                int stolen = Math.Min(6, gameHandler.players[enemy].creatureData.attack-1);
                gameHandler.players[enemy].creatureData.attack -= stolen;
                gameHandler.players[curPlayer].creatureData.attack += stolen;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Sponsorship Scrubbler steals {stolen} Attack from {gameHandler.players[enemy].name}, leaving it with {gameHandler.players[enemy].creatureData.attack} Attack and leaving {gameHandler.players[curPlayer].name} with {gameHandler.players[curPlayer].creatureData.attack} Attack.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Sponsorship Scrubbler fails to trigger.");
            }

        }
    }

    //[SetAttribute(UpgradeSet.VentureCo)]
    //[UpgradeAttribute]
    public class VentureCoPowerGenerator : VentureCoMechNaming
    {
        public VentureCoPowerGenerator()
        {
            this.rarity = Rarity.Rare;
            this.name = "Venture Co. Power Generator";
            this.cardText = "Battlecry: Refresh 1 Mana for each Venture Co. Upgrade you've bought this game.";
            this.SetStats(6, 4, 4);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);

            int refreshed = Math.Max(0, Math.Min(list.Count(), gameHandler.players[curPlayer].maxMana - gameHandler.players[curPlayer].curMana));

            gameHandler.players[curPlayer].curMana += refreshed;

            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class Shieldmobile : Upgrade
    {
        public Shieldmobile()
        {
            this.rarity = Rarity.Common;
            this.name = "Shieldmobile";
            this.cardText = "Magnetic. Battlecry: Gain +6 Shields. Overload: (4).";
            this.SetStats(2, 2, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 6;
            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class Spikecycle : Upgrade
    {
        public Spikecycle()
        {
            this.rarity = Rarity.Common;
            this.name = "Spikecycle";
            this.cardText = "Magnetic. Battlecry: Gain +6 Spikes. Overload: (4).";
            this.SetStats(2, 6, 2);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 6;
            return Task.CompletedTask;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class AnonymousSupplier : Upgrade
    {
        public AnonymousSupplier()
        {
            this.rarity = Rarity.Rare;
            this.name = "Anonymous Supplier";
            this.cardText = this.writtenEffect = "Your Upgrades are not shared in the combat log.";
            this.SetStats(3, 3, 3);
        }

        public override Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].specificEffects.hideUpgradesInLog = true;

            return base.OnPlay(gameHandler, curPlayer, enemy);
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class CopyShredder : Upgrade
    {
        public CopyShredder()
        {
            this.rarity = Rarity.Rare;
            this.name = "Copy Shredder";
            this.cardText = this.writtenEffect = "Start of Combat: Gain +1/+1 for each duplicate Upgrade your opponent bought this round.";
            this.SetStats(3, 2, 4);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<string> found = new List<string>();
            int buff = 0;

            for (int i = 0; i < gameHandler.players[enemy].boughtThisTurn.Count(); i++)
            {
                if (found.Contains(gameHandler.players[enemy].boughtThisTurn[i].name))
                {
                    buff++;
                }
                else found.Add(gameHandler.players[enemy].boughtThisTurn[i].name);
            }

            gameHandler.players[curPlayer].creatureData.attack += buff;
            gameHandler.players[curPlayer].creatureData.health += buff;

            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Copy Shredder gives it +{buff}/+{buff}.");
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class IllegalThermodynamo : Upgrade
    {
        public IllegalThermodynamo()
        {
            this.rarity = Rarity.Rare;
            this.name = "Illegal Thermodynamo";
            this.cardText = "After this is Frozen, it gains +3/+3.";
            this.SetStats(3, 2, 2);
        }

        public override void OnBeingFrozen(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 3;
            gameHandler.players[curPlayer].creatureData.health += 3;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class SiliconGrenadeBelt : Upgrade
    {
        public SiliconGrenadeBelt()
        {
            this.rarity = Rarity.Common;
            this.name = "Silicon Grenade Belt";
            this.cardText = this.writtenEffect = "Start of Combat: Deal 1 damage to the enemy Upgrade, twice.";
            this.SetStats(3, 4, 2);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].TakeDamage(1, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
            gameHandler.players[enemy].TakeDamage(1, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Silicon Grenade Belt deals 1 damage, ");
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class VentureCoExcavator : Upgrade
    {
        public VentureCoExcavator()
        {
            this.rarity = Rarity.Rare;
            this.name = "Venture Co. Excavator";
            this.cardText = "Magnetic. If your hand is empty, Magnetic x3 instead.";
            this.SetStats(7, 5, 6);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
        }

        public override Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].hand.OptionsCount() == 0)
            {
                this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 3;
            }

            return base.OnPlay(gameHandler, curPlayer, enemy);
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class TreasureMiner : Upgrade
    {
        public TreasureMiner()
        {
            this.rarity = Rarity.Rare;
            this.name = "Treasure Miner";
            this.cardText = "Battlecry: Discover a Legendary Upgrade.";
            this.SetStats(5, 4, 3);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.rarity == Rarity.Legendary);
            await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, "Discover a Legendary Upgrade", pool);
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class Investrotron : Upgrade
    {
        public Investrotron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Investotron";
            this.cardText = this.writtenEffect = "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Upgrade +4/+4.";
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


    public class SwitchboardEffect : Upgrade
    {
        bool comboTrig = false;
        bool spellburst = true;

        public SwitchboardEffect()
        {

        }

        public override void Combo(GameHandler gameHandler, int curPlayer, int enemy)
        {
            comboTrig = true;
            this.writtenEffect = "Spellburst: Gain Rush x4.";
        }

        public override Task OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (comboTrig && spellburst)
            {
                spellburst = false;
                this.writtenEffect = string.Empty;
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 4;
            }

            return base.OnSpellCast(spell, gameHandler, curPlayer, enemy);
        }

        public override Card DeepCopy()
        {
            SwitchboardEffect ret = (SwitchboardEffect)base.DeepCopy();
            ret.comboTrig = this.comboTrig;
            return ret;
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class EncodedSwitchboard : Upgrade
    {
        public EncodedSwitchboard()
        {
            this.rarity = Rarity.Epic;
            this.name = "Encoded Switchboard";
            this.cardText = this.writtenEffect = "Aftermath: Give a random Legendary Upgrade in your shop \"Combo: Gain \'Spellburst: Gain Rush x4.\'\"";
            this.SetStats(3, 0, 4);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> legendaries = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].shop.GetAllUpgrades(), x => x.rarity == Rarity.Legendary);
            int index = GameHandler.randomGenerator.Next(0, legendaries.Count());

            legendaries[index].extraUpgradeEffects.Add(new SwitchboardEffect());
            legendaries[index].cardText += " Combo: Gain \'Spellburst: Gain Rush x4.\'";

            gameHandler.players[curPlayer].aftermathMessages.Add($"Your Encoded Switchboard gives the {legendaries[index].name} in your shop \"Combo: Gain \'Spellburst: Gain Rush x4.\'\"");
        }
    }      

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class NeatoMagnetMagneto : Upgrade
    {
        private bool spellburst = true;

        public NeatoMagnetMagneto()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Neato Magnet Magneto";
            this.cardText = this.writtenEffect = "Spellburst: If the spell is a Spare Part other than Mana Capsule, apply its effect to all Upgrades in your shop.";
            this.SetStats(9, 8, 6);
        }

        public override Task OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!spellburst) return Task.CompletedTask;

            spellburst = false;
            this.writtenEffect = string.Empty;

            if (Attribute.IsDefined(spell.GetType(), typeof(SparePartAttribute)))
            {
                Spell sparePart = (Spell)spell;

                if (sparePart.name.Equals("Mana Capsule")) return Task.CompletedTask;

                List<int> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

                for (int i = 0; i < upgrades.Count(); i++)
                {
                    sparePart.CastOnUpgradeInShop(upgrades[i], gameHandler, curPlayer, enemy);
                }
            }

            return base.OnSpellCast(spell, gameHandler, curPlayer, enemy);
        }
    }

    [SetAttribute(UpgradeSet.VentureCo)]
    [UpgradeAttribute]
    public class GoldPrinceGallyWX  : Upgrade
    {
        public GoldPrinceGallyWX()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Gold Prince Gally-WX";
            this.cardText = this.writtenEffect = "After you play a Spare Part, Discover an Upgrade. It costs (2) less.";
            this.showEffectInCombat = false;
            this.SetStats(7, 5, 8);
        }
        public override async Task OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (Attribute.IsDefined(spell.GetType(), typeof(SparePartAttribute)))
            {
                var choice = await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, "Discover an Upgrade", gameHandler.players[curPlayer].pool.upgrades).ConfigureAwait(false);
                choice.Cost -= 2;
            }

            base.OnSpellCast(spell, gameHandler, curPlayer, enemy);
        }
    }
}

/*
 
[UpgradeAttribute]
public class VentureCo : Upgrade
{
    public VentureCo()
    {
        this.rarity = Rarity.;
        this.name = "Venture Co.";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

 */