using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class Healthbox : Mech
    {
        public Healthbox()
        {
            this.rarity = Rarity.Common;
            this.name = "Healthbox";
            this.cardText = this.writtenEffect = "Start of Combat: Give the enemy Mech +8 Health.";
            this.SetStats(0, 0, 8);
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[enemy].creatureData.health += 8;
            gameHandler.combatOutputCollector.preCombatHeader.Add(
                $"{gameHandler.players[curPlayer].name}'s Healthbox gives {gameHandler.players[enemy].name} +8 Health, leaving it with {gameHandler.players[enemy].creatureData.health} Health.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class Microchip : Mech
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
    [Package(UpgradePackage.TinyInventions)]
    public class RefurbishePlating : Mech
    {
        public RefurbishePlating()
        {
            this.rarity = Rarity.Common;
            this.name = "Refurbished Plating";
            this.cardText = "Battlecry: Gain Shields equal to twice your Taunt.";
            this.SetStats(2, 0, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt] * 2;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class BootPolisher : Mech
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
    [Package(UpgradePackage.TinyInventions)]
    public class CutleryDispencer : Mech
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
    [Package(UpgradePackage.TinyInventions)]
    public class GoldBolts : Mech
    {
        public GoldBolts()
        {
            this.rarity = Rarity.Rare;
            this.name = "Gold Bolts";
            this.cardText = "Battlecry: Transform your Shields into Health.";
            this.SetStats(3, 3, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] = 0;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class WupallSmasher : Mech
    {
        public WupallSmasher()
        {
            this.rarity = Rarity.Rare;
            this.name = "Wupall Smasher";
            this.cardText = "Battlecry: Transform your Spikes into Attack.";
            this.SetStats(5, 4, 5);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes];
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] = 0;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class BrawlersPlating : Mech
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
    [Package(UpgradePackage.TinyInventions)]
    public class PowerGlove : Mech
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
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 8;
                gameHandler.players[enemy].TakeDamage(4, gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Brawler's Plating triggers and deals 4 damage, ");
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
            this.cost = 1;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy);
            gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.TinyInventions)]
    public class CelsiorX : Mech
    {
        public CelsiorX()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Celsior X";
            this.cardText = "Battlecry: Add a 1-Cost Absolute Zero to your hand. It Freezes an Upgrade and can be played any number of times.";
            this.SetStats(2, 2, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].hand.AddCard(new AbsoluteZero());
        }
    }
}
