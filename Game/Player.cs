using DSharpPlus.CommandsNext;
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

        public List<Upgrade> attachedMechs;
        public SpecificEffects specificEffects;

        public List<List<Card>> playHistory;
        public List<Upgrade> boughtThisTurn;
        public List<Upgrade> extraUpgradeEffects;

        public bool destroyed;

        public List<string> aftermathMessages;
        public List<Upgrade> nextRoundEffects;

        public bool ready;

        public int lives;

        public CommandContext ctx;

        public Player()
        {
            this.creatureData = new CreatureData();
            this.shop = new Shop();
            this.hand = new Hand();
            this.name = "No name";
            this.attachedMechs = new List<Upgrade>();
            this.curMana = 10;
            this.destroyed = false;
            this.aftermathMessages = new List<string>();
            this.overloaded = 0;
            this.ready = false;
            this.lives = 0;
            this.playHistory = new List<List<Card>>();
            this.playHistory.Add(new List<Card>());
            this.boughtThisTurn = new List<Upgrade>();
            this.specificEffects = new SpecificEffects();
            this.extraUpgradeEffects = new List<Upgrade>();
            this.nextRoundEffects = new List<Upgrade>();
        }
        public Player(string name) : this()
        {
            this.name = name;
        }        

        public string PrintInfoGeneral(GameHandler gameHandler, int curPlayer)
        {
            string ret = string.Empty;

            ret += $"**{this.creatureData.attack}/{this.creatureData.health}**";
            ret += $"\nMana: {this.curMana}/{gameHandler.maxMana}";
            if (this.overloaded > 0) ret += $"\n ({this.overloaded} Overloaded)";
            if (this.lives > 1) ret += $"\nLives: {this.lives}";
            else ret += "\nLives: **1** (!)";
            ret += "\nOpponent: ";
            if (gameHandler.pairsHandler.opponents[curPlayer] != curPlayer) ret += $"{gameHandler.players[gameHandler.pairsHandler.opponents[curPlayer]].name}";
            else ret += "None";

            return ret;
        }
        public string PrintInfoKeywords(GameHandler gameHandler)
        {
            string ret = string.Empty;

            foreach (var kw in this.creatureData.staticKeywords)
            {
                if (kw.Key == StaticKeyword.Echo) continue;
                if (kw.Key == StaticKeyword.Binary) continue;

                if (kw.Value != 0)
                {
                    if (ret.Equals(string.Empty)) ret += $"{kw.Key}: {kw.Value}";
                    else ret += $"\n{kw.Key}: {kw.Value}";
                }
            }

            if (ret.Equals(string.Empty)) return "(none)";
            return ret;
        }
        public string PrintInfoEffects(GameHandler gameHandler)
        {
            string ret = string.Empty;

            for (int i = 0; i < this.attachedMechs.Count(); i++)
            {
                if (!this.attachedMechs[i].writtenEffect.Equals(string.Empty))
                {
                    if (ret.Equals(string.Empty)) ret += $"{this.attachedMechs[i].writtenEffect}";
                    else ret += $"\n{this.attachedMechs[i].writtenEffect}";
                }
            }

            for (int i = 0; i < this.extraUpgradeEffects.Count(); i++)
            {
                if (!this.extraUpgradeEffects[i].writtenEffect.Equals(string.Empty))
                {
                    if (ret.Equals(string.Empty)) ret += $"{this.extraUpgradeEffects[i].writtenEffect}";
                    else ret += $"\n{this.extraUpgradeEffects[i].writtenEffect}";
                }
            }

            if (ret.Equals(string.Empty)) return "(none)";
            return ret;
        }
        public string PrintInfoUpgrades(GameHandler gameHandler)
        {
            int filler = 0;
            string ret = this.GetUpgradesList(out filler, false);

            if (ret.Equals(string.Empty) || filler == 0) return "(none)";
            return ret;
        }

        public string PrintInfo(GameHandler gameHandler)
        {   
            string ret = string.Empty;

            ret += $"**{this.creatureData.attack}/{this.creatureData.health}**\n";
            ret += $"Mana: {this.curMana}/{gameHandler.maxMana}";
            if (this.overloaded > 0) ret += $" ({this.overloaded} Overloaded)";
            ret += "\n";

            ret += $"\nKeywords:\n";
            foreach (var kw in this.creatureData.staticKeywords)
            {
                if (kw.Key == StaticKeyword.Echo) continue;
                if (kw.Key == StaticKeyword.Binary) continue;                

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

        public void AttachMech(Upgrade mech, GameHandler gameHandler, int curPlayer, int enemy)
        {
            mech.OnPlay(gameHandler, curPlayer, enemy);

            foreach (var extraEffect in mech.extraUpgradeEffects)
            {
                extraEffect.OnPlay(gameHandler, curPlayer, enemy);
            }

            if (mech.creatureData.staticKeywords[StaticKeyword.Magnetic] > 0)
            {
                for (int i = 0; i < mech.creatureData.staticKeywords[StaticKeyword.Magnetic]; i++)
                {
                    PlayerInteraction.ActivateMagnetic(gameHandler, curPlayer, enemy);
                }
            }

            if (mech.creatureData.staticKeywords[StaticKeyword.Echo] > 0)
            {
                gameHandler.players[curPlayer].shop.AddUpgrade(mech.BasicCopy());
            }

            if (mech.creatureData.staticKeywords[StaticKeyword.Binary] > 0)
            {
                mech.creatureData.staticKeywords[StaticKeyword.Binary]--;

                Upgrade binaryLessCopy = mech.BasicCopy();                
                binaryLessCopy.cardText += " (No Binary)";
                binaryLessCopy.creatureData.staticKeywords[StaticKeyword.Binary]--;

                //the copy should have basic stats
                gameHandler.players[curPlayer].hand.AddCard(binaryLessCopy);

            }
            mech.creatureData.staticKeywords[StaticKeyword.Binary] = 0;

            this.creatureData += mech.creatureData;

            this.creatureData.staticKeywords[StaticKeyword.Echo] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Magnetic] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
            this.creatureData.staticKeywords[StaticKeyword.Binary] = 0;

            mech.Battlecry(gameHandler, curPlayer, enemy);

            foreach (var extraEffect in mech.extraUpgradeEffects)
            {
                extraEffect.Battlecry(gameHandler, curPlayer, enemy);                
            }

            if (gameHandler.players[curPlayer].playHistory[gameHandler.players[curPlayer].playHistory.Count()-1].Count() > 0)
            {
                mech.Combo(gameHandler, curPlayer, enemy);

                foreach (var extraEffect in mech.extraUpgradeEffects)
                {
                    extraEffect.Combo(gameHandler, curPlayer, enemy);                    
                }
            }

            this.attachedMechs.Add((Upgrade)mech.DeepCopy());

            foreach (var extraEffect in mech.extraUpgradeEffects)
            {
                Console.WriteLine("sadge'");
                this.extraUpgradeEffects.Add((Upgrade)extraEffect.DeepCopy());
            }
            Console.WriteLine("a");
        }

        public bool BuyCard(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            bool result = this.shop.At(shopPos).CanBeBought(shopPos, gameHandler, curPlayer, enemy);
            if (!result) return false;

            Card card = this.shop.At(shopPos).DeepCopy();

            this.shop.RemoveUpgrade(shopPos);

            this.shop.At(shopPos).inLimbo = true;
            ((Upgrade)card).BuyCard(shopPos, gameHandler, curPlayer, enemy);
            this.shop.At(shopPos).inLimbo = false;

            this.playHistory[this.playHistory.Count() - 1].Add(card.DeepCopy());
            this.boughtThisTurn.Add((Upgrade)card.DeepCopy());
            
            return result;
        }
        public bool PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (handPos >= this.hand.totalSize) return false;
            if (this.hand.At(handPos).name == BlankUpgrade.name) return false;
            Card card = this.hand.At(handPos).DeepCopy();

            bool result = this.hand.At(handPos).PlayCard(handPos, gameHandler, curPlayer, enemy);

            if (result)
            {
                this.playHistory[this.playHistory.Count() - 1].Add(card.DeepCopy());

                this.hand.RemoveCard(handPos);
            }
            return result;
        }

        public int AttackMech(GameHandler gameHandler, int attacker, int defender)
        {
            string msg = $"{this.name} attacks for {this.creatureData.attack} damage, ";
            int damage = this.creatureData.attack;
            if (this.creatureData.staticKeywords[StaticKeyword.Spikes] > 0 && !gameHandler.players[defender].specificEffects.ignoreSpikes)
            {
                damage += this.creatureData.staticKeywords[StaticKeyword.Spikes];
                msg += $"increased to {damage} by Spikes, ";
            }
                
            if (gameHandler.players[defender].creatureData.staticKeywords[StaticKeyword.Shields] > 0 && !gameHandler.players[attacker].specificEffects.ignoreShields)
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

            if (!gameHandler.players[defender].specificEffects.ignoreSpikes) this.creatureData.staticKeywords[StaticKeyword.Spikes] = 0;

            return gameHandler.players[defender].TakeDamage(damage, gameHandler, attacker, defender, msg);
        }

        public int TakeDamage(int damage, GameHandler gameHandler, int attacker, int defender, string msg)
        {
            for (int i=0; i<this.attachedMechs.Count(); i++)
            {
                this.attachedMechs[i].BeforeTakingDamage(ref damage, gameHandler, defender, attacker, ref msg);                
            }
            foreach (var extraEffect in gameHandler.players[defender].extraUpgradeEffects)
            {
                extraEffect.BeforeTakingDamage(ref damage, gameHandler, defender, attacker, ref msg);
            }

            this.creatureData.health -= damage;

            if (this.creatureData.health > 0)
            {
                msg += $"reducing {this.name} to {this.creatureData.health} Health.";
            }
            else
            {
                this.destroyed = true;
                msg += $"destroying {this.name}.";
                gameHandler.combatOutputCollector.combatHeader.Add(msg);
                return damage;
            }

            //Console.WriteLine("Called: " + msg);
            gameHandler.combatOutputCollector.combatHeader.Add(msg);

            if (damage > 0)
            {
                if (gameHandler.players[attacker].creatureData.staticKeywords[StaticKeyword.Poisonous] > 0)
                {
                    this.destroyed = true;
                    gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[attacker].name}'s Poisonous destroys {this.name}.");

                    return damage;
                }

                for (int i=0; i<this.attachedMechs.Count() && gameHandler.players[attacker].IsAlive() && gameHandler.players[defender].IsAlive(); i++)
                {
                    this.attachedMechs[i].AfterThisTakesDamage(damage, gameHandler, defender, attacker);                    
                }

                foreach (var extraEffect in gameHandler.players[defender].extraUpgradeEffects)
                {
                    if (!(gameHandler.players[attacker].IsAlive() && gameHandler.players[defender].IsAlive())) break;
                    extraEffect.AfterThisTakesDamage(damage, gameHandler, defender, attacker);
                }
            }

            return damage;
        }

        public bool IsAlive()
        {
            if (this.destroyed) return false;
            if (this.creatureData.health <= 0) return false;
            return true;
        }

        public string GetInfoForCombat(GameHandler gameHandler)
        {
            string ret = string.Empty;

            bool isVanilla = true;

            string preCombatEffects = string.Empty;
            for (int i=0; i<this.attachedMechs.Count(); i++)
            {                
                if (this.attachedMechs[i].writtenEffect.Equals(string.Empty)) continue;
                if (!this.attachedMechs[i].printEffectInCombat) continue;
                if (this.attachedMechs[i].writtenEffect.StartsWith("Aftermath:")) continue;
                if (this.attachedMechs[i].writtenEffect.StartsWith("Spellburst:")) continue;

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
                ret += $"**{this.name} is a {this.creatureData.attack}/{this.creatureData.health} vanilla.**\n";
            }
            else
            {
                ret += $"**{this.name} is a {this.creatureData.attack}/{this.creatureData.health} with:**\n";
                foreach (var kw in this.creatureData.staticKeywords)
                {
                    if (kw.Value == 0) continue;
                    if (kw.Key == StaticKeyword.Overload) continue;
                    ret += $"{kw.Key}: {kw.Value}\n";
                }
            }

            if (!preCombatEffects.Equals(string.Empty)) ret += preCombatEffects + "\n";

            return ret;
        }

        public string GetUpgradesList(out int rows, bool respectHidden = true)
        {
            if (respectHidden && this.specificEffects.hideUpgradesInLog)
            {
                rows = 1;
                return "(Hidden)";
            }

            string ret = string.Empty;
            rows = 0;

            for (int i = 0; i < this.attachedMechs.Count(); i++)
            {
                if (this.attachedMechs[i].name == BlankUpgrade.name) continue;
                int mult = 1;
                ret += $"- {this.attachedMechs[i].name}";

                for (int j=i+1; j<this.attachedMechs.Count(); j++)
                {
                    if (this.attachedMechs[j].name == this.attachedMechs[i].name) mult++;
                    else break;
                }

                i += mult - 1;

                rows++;

                if (mult > 1) ret += $" x{mult}";
                if (i != this.attachedMechs.Count() - 1) ret += "\n";
            }

            return ret;
        }
        public string GetAftermathMessages()
        {
            string ret = string.Empty;
            for (int i = 0; i < this.aftermathMessages.Count(); i++) ret = ret + this.aftermathMessages[i] + "\n";
            return ret;
        }
    }    
}
