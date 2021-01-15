﻿using System;
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
            this.name = "Venture Co. Sawmblade";
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
            this.cardText = this.writtenEffect = "Start of Combat: Deal 3 damage to the enemy Mech for each Venture Co. Upgrade you've bought this game.";
            this.creatureData = new CreatureData(5, 2, 2);            
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, VentureCo.Criteria);

            gameHandler.players[enemy].TakeDamage(3*list.Count(), ref gameHandler, curPlayer, enemy, $"{gameHandler.players[curPlayer].name}'s Venture Co. Flamethrower deals {3*list.Count()} damage, ");
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