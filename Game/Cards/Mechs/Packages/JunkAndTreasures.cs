﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class BronzeBruiser : Mech
    {
        public BronzeBruiser()
        {
            this.rarity = Rarity.Common;
            this.name = "Bronze Bruiser";
            this.cardText = this.writtenEffect = "Aftermath: Add 4 random Common Upgrades to your shop.";
            this.SetStats(2, 1, 2);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Common) return true;
            return false;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 4; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Bronze Bruiser adds 4 random Common Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class SilverShogun : Mech
    {
        public SilverShogun()
        {
            this.rarity = Rarity.Common;
            this.name = "Silver Shogun";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 random Rare Upgrades to your shop.";
            this.SetStats(3, 2, 3);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Rare) return true;
            return false;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 3; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Silver Shogun adds 3 random Rare Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class GoldenGunner : Mech
    {
        public GoldenGunner()
        {
            this.rarity = Rarity.Common;
            this.name = "Golden Gunner";
            this.cardText = this.writtenEffect = "Aftermath: Add 2 random Epic Upgrades to your shop.";
            this.SetStats(4, 3, 4);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Epic) return true;
            return false;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 2; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Golden Gunner adds 2 random Epic Upgrades to your shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class PlatinumParagon : Mech
    {
        public PlatinumParagon()
        {
            this.rarity = Rarity.Common;
            this.name = "Platinum Paragon";
            this.cardText = this.writtenEffect = "Aftermath: Add 1 random Legendary Upgrade to your shop.";
            this.SetStats(5, 4, 5);
        }

        private bool Criteria(Mech m)
        {
            if (m.rarity == Rarity.Legendary) return true;
            return false;
        }
        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 1; i++)
            {
                int pos = GameHandler.randomGenerator.Next(0, list.Count());
                gameHandler.players[curPlayer].shop.AddUpgrade(list[pos]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Platinum Paragon adds 1 random Legendary Upgrade to your shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class CircusCircuit : Mech
    {
        public CircusCircuit()
        {
            this.rarity = Rarity.Common;
            this.name = "Circus Circuit";
            this.cardText = "Aftermath: Add a random Spare Part to your hand.";
            this.SetStats(3, 2, 3);
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = GameHandler.randomGenerator.Next(0, gameHandler.pool.spareparts.Count());

            gameHandler.players[curPlayer].hand.AddCard(gameHandler.pool.spareparts[pos]);

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Circus Circuit added a {gameHandler.pool.spareparts[pos].name} to your hand.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class MagnetBall : Mech
    {
        public MagnetBall()
        {
            this.rarity = Rarity.Common;
            this.name = "Magnet Ball";
            this.cardText = "Magnetic, Taunt. Overload: (4).";
            this.SetStats(2, 4, 5);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class BoomerangMagnet : Mech
    {
        public BoomerangMagnet()
        {
            this.rarity = Rarity.Common;
            this.name = "Boomerang Magnet";
            this.cardText = "Magnetic, Rush. Overload: (4)";
            this.SetStats(5, 5, 4);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 4;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class TrashCube : Mech
    {
        public TrashCube()
        {
            this.rarity = Rarity.Common;
            this.name = "Trash Cube";
            this.cardText = "Echo, Magnetic";
            this.SetStats(5, 4, 4);
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Echo] = 1;
        }
    }    

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class Scrapbarber : Mech
    {
        public Scrapbarber()
        {
            this.rarity = Rarity.Rare;
            this.name = "Scrapbarber";
            this.cardText = this.writtenEffect = "After this attacks the enemy Mech, steal 2 Attack and Health from it.";
            this.SetStats(5, 3, 3);
        }

        public override void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            int stolenat = Math.Min(gameHandler.players[enemy].creatureData.attack - 1, 2);
            int stolenhp = Math.Min(gameHandler.players[enemy].creatureData.health, 2);

            gameHandler.players[curPlayer].creatureData.attack += stolenat;
            gameHandler.players[curPlayer].creatureData.health += stolenhp;

            gameHandler.players[enemy].creatureData.attack -= stolenat;
            gameHandler.players[enemy].creatureData.health -= stolenhp;

            gameHandler.combatOutputCollector.combatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Scrapbarber steals {stolenat}/{stolenhp} from {gameHandler.players[enemy].name}, " +
                $"leaving it as a {gameHandler.players[enemy].creatureData.Stats()} and leaving {gameHandler.players[curPlayer].name} as a {gameHandler.players[curPlayer].creatureData.Stats()}.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class ScrapStacker : Mech
    {
        public ScrapStacker()
        {
            this.rarity = Rarity.Rare;
            this.name = "Scrap Stacker";
            this.cardText = this.writtenEffect = "After you buy another Upgrade, gain +2/+2.";
            this.printEffectInCombat = false;
            this.SetStats(8, 4, 4);
        }

        public override void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2;
            gameHandler.players[curPlayer].creatureData.health += 2;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
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

            for (int i = 0; i < 6; i++)
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
    [Package(UpgradePackage.JunkAndTreasures)]
    public class CompetentScrapper : Mech
    {
        public CompetentScrapper()
        {
            this.rarity = Rarity.Epic;
            this.name = "Competent Scrapper";
            this.cardText = "Battlecry: Discard all Spare Parts in your hand. Give your Mech +3/+3 for each.";
            this.SetStats(4, 3, 4);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<int> handIndexes = gameHandler.players[curPlayer].hand.GetAllUpgradeIndexes();

            for (int i = 0; i < handIndexes.Count(); i++)
            {
                if (Attribute.IsDefined(gameHandler.players[curPlayer].hand.At(handIndexes[i]).GetType(), typeof(SparePartAttribute)))
                {
                    gameHandler.players[curPlayer].hand.RemoveCard(handIndexes[i]);
                    gameHandler.players[curPlayer].creatureData.attack += 3;
                    gameHandler.players[curPlayer].creatureData.health += 3;
                }
            }
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class SuperScooper : Mech
    {
        public SuperScooper()
        {
            this.rarity = Rarity.Epic;
            this.name = "Super Scooper";
            this.cardText = this.writtenEffect = "Start of Combat: Steal the stats of the lowest-cost Upgrade your opponent bought last turn from their Mech.";
            this.SetStats(8, 3, 7);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int lowestCost = 9999;
            List<Mech> upgradesList = new List<Mech>();

            for (int i = 0; i < gameHandler.players[enemy].boughtThisTurn.Count(); i++)
            {
                if (gameHandler.players[enemy].boughtThisTurn[i].Cost < lowestCost && gameHandler.players[enemy].boughtThisTurn[i].creatureData.attack != 0 && gameHandler.players[enemy].boughtThisTurn[i].creatureData.health != 0)
                {
                    lowestCost = gameHandler.players[enemy].boughtThisTurn[i].Cost;
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
    [Package(UpgradePackage.JunkAndTreasures)]
    public class Solartron3000 : Mech
    {
        private bool triggered;

        public Solartron3000()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Solartron 3000";
            this.cardText = "Battlecry: The next Upgrade you buy this turn has Binary.";
            this.writtenEffect = "The next Upgrade you buy this turn has Binary.";
            this.printEffectInCombat = false;
            this.SetStats(4, 2, 2);
            this.triggered = false;
        }

        public override void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (triggered == false)
            {
                triggered = true;
                this.writtenEffect = string.Empty;
                if (m.creatureData.staticKeywords[StaticKeyword.Binary] < 1) m.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            }
        }
    }


    [TokenAttribute]
    public class Receipt : Spell
    {
        public Receipt()
        {
            this.name = "Receipt";
            this.cardText = "Name a number of Attack or Health. Remove that much from your Mech and gain half that Mana this turn only (rounded down).";
            this.Cost = 0;
            this.rarity = SpellRarity.Spell;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction playerInteraction = new PlayerInteraction("Name a number of Attack or Health", "First type the number, followed by 'Attack' or 'Health'", "Capitalisation is ignored", AnswerType.StringAnswer);

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

                var msg = res.Split();

                if (msg.Count() != 2) continue;

                int numb;

                if (int.TryParse(msg[0], out numb) && (msg[1].Equals("attack", StringComparison.OrdinalIgnoreCase) || msg[1].Equals("health", StringComparison.OrdinalIgnoreCase)))
                {
                    if (numb < 0) continue;

                    ref int stat = ref gameHandler.players[curPlayer].creatureData.attack;
                    if (msg[1].Equals("health", StringComparison.OrdinalIgnoreCase)) stat = ref gameHandler.players[curPlayer].creatureData.health;

                    if (numb >= stat) continue;
                    stat -= numb;
                    gameHandler.players[curPlayer].curMana += numb / 2;
                }
                else continue;

                break;
            }
        }
    }

    public class Scrap4CashEffect : Mech
    {
        public Scrap4CashEffect()
        {
            this.writtenEffect = "Permanent Aftermath: Add a Receipt to your hand. It can refund your Mech's stats for Mana.";
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].hand.AddCard(new Receipt());
            gameHandler.players[curPlayer].nextRoundEffects.Add(new Scrap4CashEffect());
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.JunkAndTreasures)]
    public class MrScrap4Cash : Mech
    {
        public MrScrap4Cash()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Mr. Scrap-4-Cash";
            this.cardText = "Permanent Aftermath: Add a Receipt to your hand. It can refund your Mech's stats for Mana.";
            this.SetStats(5, 5, 5);
            this.extraUpgradeEffects.Add(new Scrap4CashEffect());
        }
    }
}