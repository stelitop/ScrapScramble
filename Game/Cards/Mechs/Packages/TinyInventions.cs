using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class Healthbox : Upgrade
    {
        public Healthbox()
        {
            this.rarity = Rarity.Common;
            this.name = "Healthbox";
            this.cardText = this.writtenEffect = "Start of Combat: Give the enemy Upgrade +6 Health.";
            this.SetStats(1, 0, 6);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].creatureData.health += 6;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Healthbox gives {gameHandler.players[enemy].name} +6 Health, leaving it with {gameHandler.players[enemy].creatureData.health} Health.");
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class Microchip : Upgrade
    {
        public Microchip()
        {
            this.rarity = Rarity.Common;
            this.name = "Microchip";
            this.cardText = "Overload: (2)";
            this.SetStats(0, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 2;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class RefurbishePlating : Upgrade
    {
        public RefurbishePlating()
        {
            this.rarity = Rarity.Common;
            this.name = "Refurbished Plating";
            this.cardText = "Battlecry: Gain Shields equal to twice your Taunt.";
            this.SetStats(2, 0, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt] * 2;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class BootPolisher : Upgrade
    {
        public BootPolisher()
        {
            this.rarity = Rarity.Common;
            this.name = "Boot Polisher";
            this.cardText = this.writtenEffect = "Start of Combat: If the enemy Mech has Attack Priority, gain +4 Shields.";
            this.SetStats(3, 2, 3);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.combatOutputCollector.goingFirst == enemy)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Boot Polisher triggers, giving it +4 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Boot Polisher fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class CutleryDispencer : Upgrade
    {
        public CutleryDispencer()
        {
            this.rarity = Rarity.Common;
            this.name = "Cutlery Dispencer";
            this.cardText = this.writtenEffect = "Start of Combat: If your Mech has Attack Priority, gain +4 Spikes.";
            this.SetStats(3, 3, 2);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.combatOutputCollector.goingFirst == curPlayer)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer triggers, giving it +4 Spikes, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes]} Spikes.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Cutlery Dispencer fails to trigger.");
            }
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class GoldBolts : Upgrade
    {
        public GoldBolts()
        {
            this.rarity = Rarity.Rare;
            this.name = "Gold Bolts";
            this.cardText = "Battlecry: Transform your Mech's Shields into Health.";
            this.SetStats(3, 3, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] = 0;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class WupallSmasher : Upgrade
    {
        public WupallSmasher()
        {
            this.rarity = Rarity.Rare;
            this.name = "Wupall Smasher";
            this.cardText = "Battlecry: Transform your Mech's Spikes into Attack.";
            this.SetStats(5, 4, 5);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] = 0;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class BrawlersPlating : Upgrade
    {
        private bool enemyatk = false;
        private bool meatk = false;
        private bool activated = false;

        public BrawlersPlating()
        {
            this.rarity = Rarity.Rare;
            this.name = "Brawler's Plating";
            this.cardText = this.writtenEffect = "After both Mechs have attacked, gain +8 Shields.";
            this.SetStats(2, 1, 2);
        }

        private void Effect(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!activated)
            {
                activated = true;
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
                gameHandler.combatOutputCollector.combatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Brawler's Plating triggers and gives it +8 Shields, leaving it with {gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields]} Shields.");
            }
        }

        public override void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            meatk = true;
            if (meatk && enemyatk) this.Effect(gameHandler, curPlayer, enemy);
        }

        public override void AfterTheEnemyAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            enemyatk = true;
            if (meatk && enemyatk) this.Effect(gameHandler, curPlayer, enemy);
        }
        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            meatk = enemyatk = activated = false;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class PowerGlove : Upgrade
    {
        private bool enemyatk = false;
        private bool meatk = false;
        private bool activated = false;

        public PowerGlove()
        {
            this.rarity = Rarity.Rare;
            this.name = "Power Glove";
            this.cardText = this.writtenEffect = "After both Mechs have attacked, deal 4 damage to the enemy Mech.";
            this.SetStats(3, 2, 3);
        }

        private void Effect(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!activated)
            {
                activated = true;
                gameHandler.players[enemy].TakeDamage(4, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Power Glove triggers and deals 4 damage, ");
            }
        }

        public override void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            meatk = true;
            if (meatk && enemyatk) this.Effect(gameHandler, curPlayer, enemy);
        }

        public override void AfterTheEnemyAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy)
        {
            enemyatk = true;
            if (meatk && enemyatk) this.Effect(gameHandler, curPlayer, enemy);
        }
        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            meatk = enemyatk = activated = false;
        }
    }


    [TokenAttribute]
    public class AbsoluteZero : Spell
    {
        public AbsoluteZero()
        {
            this.rarity = SpellRarity.Spell;
            this.name = "Absolute Zero";
            this.cardText = "Freeze an Upgrade. Return this to your hand.";
            this.Cost = 1;
        }

        public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            await PlayerInteraction.FreezeUpgradeInShopAsync(gameHandler, curPlayer, enemy);
            Card token = new AbsoluteZero();
            gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.FindBasicCard(token.name));
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class CelsiorX : Upgrade
    {
        public CelsiorX()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Celsior X";
            this.cardText = "Battlecry: Add a 1-cost Absolute Zero to your hand. It Freezes an Upgrade and can be played any number of times.";
            this.SetStats(2, 2, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            Card token = new AbsoluteZero();
            gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.FindBasicCard(token.name));
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class MetallicJar : Upgrade
    {
        public MetallicJar()
        {
            this.rarity = Rarity.Common;
            this.name = "Metallic Jar";
            this.cardText = "Battlecry: Discover a 1-Cost Upgrade.";
            this.SetStats(1, 1, 1);
        }

        public override async Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> pool = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, x => x.Cost == 1 && x.name != this.name);
            await PlayerInteraction.DiscoverACardAsync<Upgrade>(gameHandler, curPlayer, enemy, "1-Cost Upgrade", pool);                        
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class TwoLayeredSpikeball : Upgrade
    {
        public TwoLayeredSpikeball()
        {
            this.rarity = Rarity.Rare;
            this.name = "Two-Layered Spikeball";
            this.cardText = "Has Spikes equal to its Attack and Shields equal to its Health.";
            this.SetStats(1, 1, 1);
        }

        public override Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += this.creatureData.attack;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += this.creatureData.health;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class NextGenerationCPU : Upgrade
    {
        public NextGenerationCPU()
        {
            this.rarity = Rarity.Epic;
            this.name = "Next Generation CPU";
            this.cardText = "Battlecry: For the rest of the game, your 1-Cost Upgrades have +1/+1.";
            this.SetStats(1, 1, 1);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            foreach (var upgrade in gameHandler.players[curPlayer].pool.upgrades)
            {
                if (upgrade.Cost == 1)
                {
                    upgrade.creatureData.attack++;
                    upgrade.creatureData.health++;
                }
            }
            foreach (var card in gameHandler.players[curPlayer].pool.tokens)
            {
                if (card.GetType().IsSubclassOf(typeof(Upgrade)) || card.GetType() == typeof(Upgrade))
                {
                    ((Upgrade)card).creatureData.attack++;
                    ((Upgrade)card).creatureData.health++;
                }
            }

            var shop = gameHandler.players[curPlayer].shop.GetAllUpgrades();
            foreach (var upgrade in shop)
            {
                if (upgrade.Cost == 1)
                {
                    upgrade.creatureData.attack++;
                    upgrade.creatureData.health++;
                }
            }

            var hand = gameHandler.players[curPlayer].hand.GetAllCards();
            foreach (var card in hand)
            {
                if (card.Cost == 1)
                {
                    if (Attribute.IsDefined(card.GetType(), typeof(UpgradeAttribute)))
                    {
                        ((Upgrade)card).creatureData.attack++;
                        ((Upgrade)card).creatureData.health++;
                    }
                }
            }            

            return base.Battlecry(gameHandler, curPlayer, enemy);
        }
    }

    [UpgradeAttribute]
    [Set(UpgradeSet.TinyInventions)]
    public class NanoDuplicatorv10 : Upgrade
    {
        public NanoDuplicatorv10()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Nano-Duplicator v10";
            this.cardText = this.writtenEffect = "1-Cost Upgrades in your shop have Binary.";
            this.SetStats(1, 1, 1);
        }

        public override void OnBuyingAMech(Upgrade m, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (m.Cost == 1)
            {
                m.creatureData.staticKeywords[StaticKeyword.Binary] = Math.Max(m.creatureData.staticKeywords[StaticKeyword.Binary], 1);
            }
        }
    }
}
