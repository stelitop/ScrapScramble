using ScrapScramble.BotRelated;
using ScrapScramble.Game.Cards;
using ScrapScramble.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class Player
    {
        public CreatureData creatureData;
        public Shop shop;
        public Hand hand;
        public int curMana;
        public int overloaded;

        public string name;

        public List<Mech> attachedMechs;

        public bool destroyed;

        public List<string> aftermathMessages;

        public bool submitted;

        public int lives;

        public Player()
        {
            this.creatureData = new CreatureData();
            this.shop = new Shop();
            this.hand = new Hand();
            this.name = "No name";
            this.attachedMechs = new List<Mech>();
            this.curMana = 10;
            this.destroyed = false;
            this.aftermathMessages = new List<string>();
            this.overloaded = 0;
            this.submitted = false;
            this.lives = 0;
        }
        public Player(string name) : this()
        {
            this.name = name;
        }

        public string PrintInfo(ref GameHandler gameHandler)
        {   
            string ret = string.Empty;

            ret += $"**{this.creatureData.attack}/{this.creatureData.health}**\n";
            ret += $"Mana: {this.curMana}/{gameHandler.maxMana}";
            if (this.overloaded > 0) ret += $" ({this.overloaded} Overloaded)";
            ret += "\n";

            ret += $"\nKeywords:\n";
            foreach (var kw in this.creatureData.staticKeywords)
            {
                if (kw.Value != 0) ret += $"{kw.Key}: {kw.Value}\n";
            }
            ret += $"\nPlayer Effects:\n"; 
            for (int i=0; i<this.attachedMechs.Count(); i++)
            {
                if (!this.attachedMechs[i].writtenEffect.Equals(string.Empty)) ret += $"{this.attachedMechs[i].writtenEffect}\n";
            }

            ret += $"\nPlayer Upgrades:\n";
            foreach (var upgrade in this.attachedMechs)
            {
                ret += $"{upgrade.name}\n";
            }

            return ret;
        }

        public void AttachMech(Mech mech, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            this.attachedMechs.Add((Mech)mech.DeepCopy());

            if (mech.creatureData.staticKeywords[StaticKeyword.Binary] > 0)
            {
                mech.creatureData.staticKeywords[StaticKeyword.Binary]--;
                mech.cardText += " (No Binary)";

                //the copy should have basic stats
                gameHandler.players[curPlayer].hand.cards.Add(mech.DeepCopy());

            }
            mech.creatureData.staticKeywords[StaticKeyword.Binary] = 0;

            this.creatureData += mech.creatureData;

            mech.Battlecry(ref gameHandler, curPlayer, enemy);
        }

        public bool BuyCard(int shopPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            bool result = this.shop.options[shopPos].BuyCard(shopPos, ref gameHandler, curPlayer, enemy);

            if (result)
            {
                this.shop.options.RemoveAt(shopPos);
            }
            return result;
        }

        public bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            bool result = this.hand.cards[handPos].PlayCard(handPos, ref gameHandler, curPlayer, enemy);

            if (result)
            {
                this.hand.cards.RemoveAt(handPos);
            }
            return result;
        }

        public int AttackMech(ref GameHandler gameHandler, int attacker, int defender)
        {
            string msg = $"{this.name} attacks for {this.creatureData.attack} damage, ";
            int damage = this.creatureData.attack;
            if (this.creatureData.staticKeywords[StaticKeyword.Spikes] > 0)
            {
                damage += this.creatureData.staticKeywords[StaticKeyword.Spikes];
                msg += $"increased to {damage} by Spikes, ";
            }

            if (gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields] > 0)
            {
                damage -= gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields];
                if (damage < 0) damage = 0;
                gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields] = 0;

                if (this.creatureData.staticKeywords[StaticKeyword.Spikes] > 0)
                {
                    msg = $"{this.name} attacks for {this.creatureData.attack} damage, adjusted to {damage} by Spikes and Shields, ";
                }
                else
                {
                    msg += $"reduced to {damage} by Shields, ";
                }
            }

            this.creatureData.staticKeywords[StaticKeyword.Spikes] = 0;

            return gameHandler.players[defender].TakeDamage(damage, ref gameHandler, attacker, defender, msg);
        }

        public int TakeDamage(int damage, ref GameHandler gameHandler, int attacker, int defender, string msg)
        {
            this.creatureData.health -= damage;

            if (this.creatureData.health > 0)
            {
                msg += $"reducing {this.name} to {this.creatureData.health} Health.";
            }
            else
            {
                this.destroyed = true;
                msg += $"destroying {this.name}.";
                //output a msg about getting destroyed
            }

            //Console.WriteLine("Called: " + msg);
            gameHandler.combatOutputCollector.combatHeader.Add(msg);

            if (damage > 0)
            {
                //TODO: AfterThisTakesDamage kw
            }

            return damage;
        }

        public bool IsAlive()
        {
            if (this.destroyed) return false;
            if (this.creatureData.health <= 0) return false;
            return true;
        }

        public void GetInfoForCombat(ref GameHandler gameHandler)
        {
            bool isVanilla = true;

            string preCombatEffects = string.Empty;
            for (int i=0; i<this.attachedMechs.Count(); i++)
            {                
                if (this.attachedMechs[i].writtenEffect.Equals(string.Empty)) continue;
                if (!this.attachedMechs[i].printEffectInCombat) continue;
                if (this.attachedMechs[i].writtenEffect.StartsWith("Aftermath:")) continue;

                if (isVanilla) preCombatEffects = this.attachedMechs[i].writtenEffect;
                else preCombatEffects = preCombatEffects + $"\n{this.attachedMechs[i].writtenEffect}";
                isVanilla = false;
            }

            if (isVanilla) foreach (var kw in this.creatureData.staticKeywords)
            {
                if (kw.Value > 0)
                {
                    isVanilla = false;
                    break;
                }
            }
            


            if (isVanilla)
            {
                gameHandler.combatOutputCollector.statsHeader.Add($"{this.name} is a {this.creatureData.attack}/{this.creatureData.health} vanilla.");
            }
            else
            {
                gameHandler.combatOutputCollector.statsHeader.Add($"{this.name} is a {this.creatureData.attack}/{this.creatureData.health} with:");
                foreach (var kw in this.creatureData.staticKeywords)
                {
                    if (kw.Value == 0) continue;
                    if (kw.Key == StaticKeyword.Overload) continue;
                    gameHandler.combatOutputCollector.statsHeader.Add($"{kw.Key}: {kw.Value}");
                }
            }

            if (!preCombatEffects.Equals(string.Empty)) gameHandler.combatOutputCollector.statsHeader.Add(preCombatEffects);
        }
        public string GetAftermathMessages()
        {
            string ret = string.Empty;
            for (int i = 0; i < this.aftermathMessages.Count(); i++) ret = ret + this.aftermathMessages[i] + "\n";
            return ret;
        }
    }    
}
