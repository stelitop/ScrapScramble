using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{       
    public class VentureCo
    {
        public static bool Criteria(Card m)
        {
            return m.name.Contains("Venture Co.");
        }
    }

    [UpgradeAttribute]
    public class VentureCoSticker : Mech
    {
        public VentureCoSticker()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Sticker";
            this.cardText = string.Empty;
            this.creatureData = new CreatureData(1, 0, 2);
        }
    }

    [UpgradeAttribute]
    public class VentureCoSawblade : Mech
    {
        public VentureCoSawblade()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Sawblade";
            this.cardText = "Battlecry: Gain +1 Attack for each Venture Co. Upgrade you've bought this game.";
            this.creatureData = new CreatureData(2, 1, 1);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();
        }
    }

    [UpgradeAttribute]
    public class VentureCoPauldrons : Mech
    {
        public VentureCoPauldrons()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Pauldrons";
            this.cardText = "Taunt. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.";
            this.creatureData = new CreatureData(3, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();
            gameHandler.players[curPlayer].creatureData.health += list.Count();
        }
    }

    [UpgradeAttribute]
    public class VentureCoThrusters : Mech
    {
        public VentureCoThrusters()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Thrusters";
            this.cardText = "Rush. Battlecry: Gain +1/+1 for each Venture Co. Upgrade you've bought this game.";
            this.creatureData = new CreatureData(5, 1, 1);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);
            gameHandler.players[curPlayer].creatureData.attack += list.Count();
            gameHandler.players[curPlayer].creatureData.health += list.Count();
        }
    }

    [UpgradeAttribute]
    public class VentureCoFlamethrower : Mech
    {
        public VentureCoFlamethrower()
        {
            this.rarity = Rarity.Epic;
            this.name = "Venture Co. Flamethrower";
            this.cardText = this.writtenEffect = "Start of Combat: Deal 3 damage to the enemy Mech for each Venture Co. Upgrade you've played this game.";
            this.creatureData = new CreatureData(5, 2, 2);            
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);

            gameHandler.players[enemy].TakeDamage(3*list.Count(), ref gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Venture Co. Flamethrower deals {3*list.Count()} damage, ");
        }
    }

    [UpgradeAttribute]
    public class VentureCoVault : Mech
    {
        public VentureCoVault()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Vault";
            this.cardText = "Taunt. Aftermath: Add 3 other random Venture Co. Upgrades to your shop.";
            this.writtenEffect = "Aftermath: Add 3 other random Venture Co. Upgrades to your shop.";
            this.creatureData = new CreatureData(3, 0, 5);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(ref gameHandler.pool.mechs, VentureCo.Criteria);

            for (int i=list.Count()-1; i>=0; i--)
            {
                if (list[i].name == this.name)
                {
                    list.RemoveAt(i);
                    break;
                }
            }

            for (int i=0; i<3; i++)
            {
                int card = GameHandler.randomGenerator.Next(0, list.Count());

                gameHandler.players[curPlayer].shop.options.Add((Mech)list[card].DeepCopy());
            }
        }
    }

    [UpgradeAttribute]
    public class VentureCoCoolant : Mech
    {
        public VentureCoCoolant()
        {
            this.rarity = Rarity.Common;
            this.name = "Venture Co. Coolant";
            this.cardText = "Battlecry: Freeze an Upgrade. Give it -4 Attack. Overload: (1).";
            this.creatureData = new CreatureData(2, 2, 3);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;

            int shopIndex = PlayerInteraction.FreezeUpgradeInShop(ref gameHandler, curPlayer, enemy);

            gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.attack -= 4;
            if (gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.attack < 0)
                gameHandler.players[curPlayer].shop.options[shopIndex].creatureData.attack = 0;
        }
    }

    [UpgradeAttribute]
    public class SponsorshipScrubber : Mech
    {
        public SponsorshipScrubber()
        {
            this.rarity = Rarity.Epic;
            this.name = "Sponsorship Scrubber";
            this.cardText = this.writtenEffect = "Start of Combat: If your opponent has purchased a Venture Co. Upgrade this game, steal 6 Attack from their Mech.";
            this.creatureData = new CreatureData(3, 1, 2);
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[enemy].playHistory, VentureCo.Criteria);

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
}

/*
 
[UpgradeAttribute]
public class VentureCo : Mech
{
    public VentureCo()
    {
        this.rarity = Rarity.;
        this.name = "Venture Co.";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

 */