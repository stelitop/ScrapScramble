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
            this.Cost = 1;
            this.name = "Whirling Blades";
            this.cardText = "Give your Upgrade +2 Attack and +4 Spikes.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.attack += 2;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Spikes] += 4;
            gameHandler.players[curPlayer].shop.At(shopPos).cardText += " (+4 Spikes)";
        }
    }

    [SparePartAttribute]
    public class SPArmorPlating : Spell
    {
        public SPArmorPlating()
        {
            this.Cost = 1;
            this.name = "Armor Plating";
            this.cardText = "Give your Upgrade +2 Health and +4 Shields.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.health += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 4;
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.health += 2;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Shields] += 4;
            gameHandler.players[curPlayer].shop.At(shopPos).cardText += " (+4 Shields)";
        }
    }

    [SparePartAttribute]
    public class SPReversingSwitch : Spell
    {
        public SPReversingSwitch()
        {
            this.Cost = 1;
            this.name = "Reversing Switch";
            this.cardText = "Swap your Upgrade's Attack and Health.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int mid = gameHandler.players[curPlayer].creatureData.attack;
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].creatureData.health = mid;
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            Upgrade m = gameHandler.players[curPlayer].shop.At(shopPos);
            GeneralFunctions.Swap<int>(ref m.creatureData.attack, ref m.creatureData.health);
        }
    }

    [SparePartAttribute]
    public class SPTimeAccelerator : Spell
    {
        public SPTimeAccelerator()
        {
            this.Cost = 1;
            this.name = "Time Accelerator";
            this.cardText = "Give your Upgrade Rush.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush]++;
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {            
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Rush] += 1;
            gameHandler.players[curPlayer].shop.At(shopPos).cardText += " (Rush)";
        }
    }

    [SparePartAttribute]
    public class SPRustyHorn : Spell
    {
        public SPRustyHorn()
        {
            this.Cost = 1;
            this.name = "Rusty Horn";
            this.cardText = "Give your Upgrade +3/+3 and Taunt.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Taunt]++;
            gameHandler.players[curPlayer].creatureData.attack += 3;
            gameHandler.players[curPlayer].creatureData.health += 3;
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.attack += 3;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.health += 3;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Taunt] += 1;
            gameHandler.players[curPlayer].shop.At(shopPos).cardText += " (Taunt)";
        }
    }

    [SparePartAttribute]
    public class SPManaCapsule : Spell
    {
        public SPManaCapsule()
        {
            this.Cost = 1;
            this.name = "Mana Capsule";
            this.cardText = "Gain 2 Mana this turn only.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana += 2;
        }
    }

    [SparePartAttribute]
    public class SPEmergencyCoolant : Spell
    {
        public SPEmergencyCoolant()
        {
            this.Cost = 1;
            this.name = "Emergency Coolant";
            this.cardText = "Freeze an Upgrade. Give it +4/+4.";
            this.rarity = SpellRarity.Spare_Part;
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int pos = PlayerInteraction.FreezeUpgradeInShop(gameHandler, curPlayer, enemy, 1);

            if (pos != -1)
            {
                gameHandler.players[curPlayer].shop.At(pos).creatureData.attack += 4;
                gameHandler.players[curPlayer].shop.At(pos).creatureData.health += 4;
            }
        }

        public override void CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.attack += 4;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.health += 4;
            gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Freeze] =
                Math.Max(1, gameHandler.players[curPlayer].shop.At(shopPos).creatureData.staticKeywords[StaticKeyword.Freeze]);

            gameHandler.players[curPlayer].shop.At(shopPos).OnBeingFrozen(gameHandler, curPlayer, enemy);
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

    public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
    {
        base.OnPlay(gameHandler, curPlayer, enemy);
    }
}
 
 */
