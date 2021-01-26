using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    //[UpgradeAttribute]
    public class CheapFillerLegendary : Mech
    {
        public CheapFillerLegendary()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Cheap Filler Legendary";
            this.cardText = "Binary. This card is for testing purposes only";
            this.creatureData = new CreatureData(5, 5, 5);
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
        }
    }

    [UpgradeAttribute]
    public class LadyInByte : Mech
    {
        public LadyInByte()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lady in Byte";
            this.cardText = this.writtenEffect = "Aftermath: Set your Mech's Attack equal to its Health.";
            this.creatureData = new CreatureData(6, 5, 5);
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.attack = gameHandler.players[curPlayer].creatureData.health;
            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Lady in Byte sets your Attack equal to Health, leaving you as a {gameHandler.players[curPlayer].creatureData.Stats()}");
        }
    }
    
    [UpgradeAttribute]
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
            this.creatureData = new CreatureData(4, 2, 2);
            this.triggered = false;
        }

        public override void OnBuyingAMech(Mech m, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (triggered == false)
            {
                triggered = true;
                this.writtenEffect = string.Empty;
                if (m.creatureData.staticKeywords[StaticKeyword.Binary] < 1) m.creatureData.staticKeywords[StaticKeyword.Binary] = 1;
            }
        }
    }

    [UpgradeAttribute]
    public class ExotronTheForbidden : Mech
    {
        public ExotronTheForbidden()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Exotron the Forbidden";
            this.cardText = this.writtenEffect = "Start of Combat: If you've bought all 5 parts of Exotron this game, destroy the enemy Mech.";
            this.creatureData = new CreatureData(15, 15, 15);
        }

        private bool Criteria(Card m)
        {
            if (m.name.Equals("Arm of Exotron")) return true;
            if (m.name.Equals("Leg of Exotron")) return true;
            if (m.name.Equals("Motherboard of Exotron")) return true;
            if (m.name.Equals("Wheel of Exotron")) return true;
            return false;
        }

        public override void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy)
        {            
            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[curPlayer].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i=0; i<list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4)
            {
                gameHandler.players[enemy].destroyed = true;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden triggers, destroying {gameHandler.players[enemy].name}.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden fails to trigger.");
            }    
        }

        public override string GetInfo(ref GameHandler gameHandler, int player)
        {
            string ret = base.GetInfo(ref gameHandler, player);

            List<Card> list = CardsFilter.FilterList<Card>(ref gameHandler.players[player].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4) return ret += " *(Ready!)*";
            else if (arm + leg + mb + wheel == 0) return ret += " *(Missing: All)*";
            else
            {
                ret += " *(Missing:";

                if (arm == 0) ret += " Arm,";
                if (leg == 0) ret += " Leg,";
                if (mb == 0) ret += " Motherboard,";
                if (wheel == 0) ret += " Wheel,";

                ret = ret.Remove(ret.Length - 1, 1);
                ret += ")*";
            }

            return ret;
        }
    }

    [UpgradeAttribute]
    public class LordBarox : Mech
    {
        int bet = -1;

        public LordBarox()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Lord Barox";
            this.cardText = "Battlecry: Name ANY other Mech. Aftermath: If it won last round, gain 5 Mana this turn only.";
            this.creatureData = new CreatureData(5, 4, 4);
            this.printEffectInCombat = false;
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
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

                for (int i=0; i<gameHandler.players.Count(); i++)
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
        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (this.bet == -1) return;

            if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count() - 2][this.bet] == FightResult.WIN)
            {
                gameHandler.players[curPlayer].curMana += 5;
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    "The bet on your Lord Barox was correct! You gain 5 Mana this turn only.");
            }
            else
            {
                gameHandler.players[curPlayer].aftermathMessages.Add(
                    "The bet on your Lord Barox was incorrect.");
            }
        }

        public override Card DeepCopy()
        {
            LordBarox ret = (LordBarox)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.creatureData = this.creatureData.DeepCopy();
            ret.writtenEffect = this.writtenEffect;
            ret.bet = this.bet;            
            return ret;
        }
    }

    [UpgradeAttribute]
    public class HatChucker8000 : Mech
    {
        private Rarity chosenRarity = Rarity.NO_RARITY;
        public HatChucker8000()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Hat Chucker 8000";
            this.cardText = "Battlecry: Name a Rarity. Aftermath: Give all players' Upgrades of that rarity +2/+2.";
            this.creatureData = new CreatureData(3, 3, 3);
        }

        public override void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            var prompt = new PlayerInteraction("Name a Rarity", "Common, Rare, Epic or Legendary", "Capitalisation is ignored", AnswerType.StringAnswer);

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

                if (res.Equals("common", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Common;
                else if (res.Equals("rare", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Rare;
                else if (res.Equals("epic", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Epic;
                else if (res.Equals("legendary", StringComparison.OrdinalIgnoreCase)) this.chosenRarity = Rarity.Legendary;
                else continue;

                this.writtenEffect = $"Aftermath: Give all players' Upgrades of rarity {this.chosenRarity} +2/+2.";
                this.printEffectInCombat = false;

                break;
            }
        }

        public override void AftermathMe(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgrades();

            for (int i=0; i<upgrades.Count(); i++)
            {
                if (upgrades[i].rarity == this.chosenRarity)
                {
                    upgrades[i].creatureData.attack += 2;
                    upgrades[i].creatureData.health += 2;
                }
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                $"Your Hat Chucker 8000 gave your {this.chosenRarity} Upgrades +2/+2.");
        }

        public override void AftermathEnemy(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            for (int j=0; j<gameHandler.players.Count(); j++)
            {
                if (j == curPlayer) continue;
                if (gameHandler.players[j].lives <= 0) continue;

                List<Mech> upgrades = gameHandler.players[j].shop.GetAllUpgrades();

                for (int i = 0; i < upgrades.Count(); i++)
                {
                    if (upgrades[i].rarity == this.chosenRarity)
                    {
                        upgrades[i].creatureData.attack += 2;
                        upgrades[i].creatureData.health += 2;
                    }
                }

                gameHandler.players[j].aftermathMessages.Add(
                    $"{gameHandler.players[curPlayer].name}'s Hat Chucker 8000 gave your {this.chosenRarity} Upgrades +2/+2.");
            }
        }

        public override Card DeepCopy()
        {
            HatChucker8000 ret = (HatChucker8000)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.creatureData = this.creatureData.DeepCopy();
            ret.writtenEffect = this.writtenEffect;
            ret.chosenRarity = this.chosenRarity;
            return ret;
        }
    }

    [UpgradeAttribute]
    public class ChaosPrism : Mech
    {
        private int spellbursts = 3;

        public ChaosPrism()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Chaos Prism";
            this.cardText = "Taunt x3. Spellburst: Gain \"Spellburst: Gain 'Spellburst: Gain Poisonous.'\"";
            this.writtenEffect = "Spellburst: Gain \"Spellburst: Gain 'Spellburst: Gain Poisonous.'\"";
            this.creatureData = new CreatureData(6, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 3;
        }

        public override void OnSpellCast(Card spell, ref GameHandler gameHandler, int curPlayer, int enemy)
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
    public class NeatoMagnetMagneto : Mech
    {
        private bool spellburst = true;

        public NeatoMagnetMagneto()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Neato Magnet Magneto";
            this.cardText = this.writtenEffect = "Spellburst: If the spell is a Spare Part other than Mana Capsule, apply its effect to all Upgrades in your shop.";
            this.creatureData = new CreatureData(9, 8, 6);
        }

        public override void OnSpellCast(Card spell, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (!spellburst) return;           

            spellburst = false;
            this.writtenEffect = string.Empty;

            if(Attribute.IsDefined(spell.GetType(), typeof(SparePartAttribute)))
            {
                Spell sparePart = (Spell)spell;

                if (sparePart.name.Equals("Mana Capsule")) return;

                List<int> upgrades = gameHandler.players[curPlayer].shop.GetAllUpgradeIndexes();

                for (int i=0; i<upgrades.Count(); i++)
                {
                    sparePart.CastOnUpgradeInShop(upgrades[i], ref gameHandler, curPlayer, enemy);
                }
            }            
        }
    }
}

/*

[UpgradeAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Legendary;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/