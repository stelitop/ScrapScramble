using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [SparePartAttribute]
    public class SPWhirlingBlades : Spell
    {
        public SPWhirlingBlades()
        {
            this.cost = 1;
            this.name = "Whirling Blades";
            this.cardText = "Give your Mech +2 Attack and +4 Spikes.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
        }

        public override void CastOnUpgradeInShop(int shopPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.options[shopPos].creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.options[shopPos].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
            gameHandler.players[curPlayer].shop.options[shopPos].cardText += " (+4 Spikes)";
        }
    }

    [SparePartAttribute]
    public class SPArmorPlating : Spell
    {
        public SPArmorPlating()
        {
            this.cost = 1;
            this.name = "Armor Plating";
            this.cardText = "Give your Mech +2 Health and +4 Shields.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
        }
    }

    [SparePartAttribute]
    public class SPReversingSwitch : Spell
    {
        public SPReversingSwitch()
        {
            this.cost = 1;
            this.name = "Reversing Switch";
            this.cardText = "Swap your Mech's Attack and Health.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            int mid = gameHandler.players[curPlayer].creatureData.attack;
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].creatureData.health = mid;
        }
    }

    [SparePartAttribute]
    public class SPTimeAccelerator : Spell
    {
        public SPTimeAccelerator()
        {
            this.cost = 1;
            this.name = "Time Accelerator";
            this.cardText = "Give your Mech Rush.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
        }
    }

    [SparePartAttribute]
    public class SPRustyHorn : Spell
    {
        public SPRustyHorn()
        {
            this.cost = 1;
            this.name = "Rusty Horn";
            this.cardText = "Give your Mech +3/+3 and Taunt.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt]++;
            gameHandler.players[curPlayer].creatureData.attack += 3;
            gameHandler.players[curPlayer].creatureData.health += 3;
        }
    }

    [SparePartAttribute]
    public class SPManaCapsule : Spell
    {
        public SPManaCapsule()
        {
            this.cost = 1;
            this.name = "Mana Capsule";
            this.cardText = "Gain 2 Mana this turn only.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 2;
        }
    }

    [SparePartAttribute]
    public class SPEmergencyCoolant : Spell
    {
        public SPEmergencyCoolant()
        {
            this.cost = 1;
            this.name = "Emergency Coolant";
            this.cardText = "Freeze an Upgrade. Give it +6/+6.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = PlayerInteraction.FreezeUpgradeInShop(ref gameHandler, curPlayer, enemy, 1);

            if (pos != -1)
            {
                gameHandler.players[curPlayer].shop.options[pos].creatureData.attack += 6;
                gameHandler.players[curPlayer].shop.options[pos].creatureData.health += 6;
            }
        }        
    }
}


/*
 
[SparePartAttribute]
public class SP : Spell
{
    public SP()
    {
        this.cost = 1;
        this.name = "";
        this.cardText = "";
        this.rarity = SpellRarity.Spare_Part;
    }

    public override void OnPlay(ref GameHandler gameHandler, int curPlayer, int enemy)
    {
        base.OnPlay(ref gameHandler, curPlayer, enemy);
    }
}
 
 */
