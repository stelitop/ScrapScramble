﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.EdgeOfScience)]
    public class OrbitalMechanosphere : Mech
    {
        public OrbitalMechanosphere()
        {
            this.rarity = Rarity.Common;
            this.name = "Orbital Mechanosphere";
            this.cardText = string.Empty;
            this.SetStats(30, 50, 50);
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.EdgeOfScience)]
    public class IndecisiveAutoshopper : Mech
    {
        public IndecisiveAutoshopper()
        {
            this.rarity = Rarity.Rare;
            this.name = "Indecisive Autoshopper";
            this.cardText = "Binary. Battlecry: Refresh your shop.";
            this.SetStats(4, 2, 4);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.maxMana, false);
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.EdgeOfScience)]
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
    [Package(UpgradePackage.EdgeOfScience)]
    public class PhilosophersStone : Mech
    {
        public PhilosophersStone()
        {
            this.rarity = Rarity.Epic;
            this.name = "Philosopher's Stone";
            this.cardText = "Battlecry: Transform all Upgrades in your shop into ones of the next Rarity.";
            this.SetStats(2, 1, 1);            
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> legendaries = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, x => x.rarity == Rarity.Legendary && x.Cost <= gameHandler.maxMana - 5);
            List<Mech> epics = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, x => x.rarity == Rarity.Epic && x.Cost <= gameHandler.maxMana - 5);
            List<Mech> rares = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, x => x.rarity == Rarity.Rare && x.Cost <= gameHandler.maxMana - 5);

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
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.EdgeOfScience)]
    public class ChaosPrism : Mech
    {
        private int spellbursts = 3;

        public ChaosPrism()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Chaos Prism";
            this.cardText = "Taunt x3. Spellburst: Gain \"Spellburst: Gain 'Spellburst: Gain Poisonous.'\"";
            this.writtenEffect = "Spellburst: Gain \"Spellburst: Gain 'Spellburst: Gain Poisonous.'\"";
            this.SetStats(6, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 3;
        }

        public override void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            spellbursts--;
            if (spellbursts == 2) this.writtenEffect = "Spellburst: Gain 'Spellburst: Gain Poisonous'";
            else if (spellbursts == 1) this.writtenEffect = "Spellburst: Gain Poisonous.";
            else if (spellbursts == 0)
            {
                this.writtenEffect = string.Empty;
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Poisonous] = 1;
            }
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.EdgeOfScience)]
    public class ParadoxEngine : Mech
    {
        public ParadoxEngine()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Paradox Engine";
            this.cardText = "Battlecry: This turn, after you buy an upgrade, refresh your shop.";
            this.writtenEffect = "After you buy an upgrade, refresh your shop.";
            this.SetStats(12, 10, 10);
        }

        public override void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.Refresh(gameHandler, gameHandler.maxMana, false);
        }
    }
}