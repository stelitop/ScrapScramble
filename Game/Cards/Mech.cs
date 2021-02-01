using ScrapScramble.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    [Serializable]
    public class Mech : Card, IComparable<Mech>
    {       
        public CreatureData creatureData;
        public Rarity rarity;

        public bool printEffectInCombat = true;
        public string writtenEffect;

        public Mech()
        {
            this.creatureData = new CreatureData();
            this.rarity = Rarity.NO_RARITY;
            this.name = string.Empty;
            this.cardText = string.Empty;
            this.writtenEffect = string.Empty;
        }
        //public Mech(string name, string cardText, int cost, int attack, int health, Rarity rarity)
        //{
        //    this.creatureData = new CreatureData(cost, attack, health);            
        //    this.rarity = rarity;
        //    this.name = name;
        //    this.cardText = cardText;
        //    this.writtenEffect = string.Empty;
        //}        

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret = string.Empty; 
            if (this.cardText.Equals(string.Empty) ) ret = $"{this.name} - {this.rarity} - {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {this.rarity} - {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Frozen for 1 turn) {ret}";
            else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Frozen for {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
            return ret;
        }

        //public Mech(MechJsonTemplate mechJson)
        //{
        //    this.creatureData = new CreatureData();
        //    this.creatureData.cost = mechJson.cost;
        //    this.cardText = mechJson.cardText;
        //    this.creatureData.attack = mechJson.attack;
        //    this.creatureData.health = mechJson.health;
        //    this.name = mechJson.name;         
        //}

        public override bool PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].hand.totalSize <= handPos) return false;
            if (gameHandler.players[curPlayer].hand.At(handPos).name == BlankUpgrade.name) return false;
            if (this.creatureData.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.creatureData.cost;

            gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy);
            return true;
        }

        public bool BuyCard(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.totalSize <= shopPos) return false;
            if (this.creatureData.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.creatureData.cost;

            for (int i=0; i<gameHandler.players[curPlayer].attachedMechs.Count(); i++)
            {                
                gameHandler.players[curPlayer].attachedMechs[i].OnBuyingAMech(this, gameHandler, curPlayer, enemy);
            }

            gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy);
            return true;
        }

        //public void PrintInfo()
        //{
        //    Console.WriteLine($"{this.name}: {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health} {} - \"{this.cardText}\"");
        //}

        public virtual void Battlecry(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void OnBuyingAMech(Mech m, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy) { }

        public virtual void AfterThisTakesDamage(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AfterTheEnemyAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg) {}

        public int CompareTo(Mech other)
        {
            if (this.rarity > other.rarity) return -1;
            else if (this.rarity < other.rarity) return 1;

            return this.name.CompareTo(other.name);
        }

        public override Card DeepCopy()
        {
            Mech ret = (Mech)Activator.CreateInstance(this.GetType());            
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;            
            ret.creatureData = this.creatureData.DeepCopy();            
            ret.writtenEffect = this.writtenEffect;            
            return ret;            
        }
        public virtual Mech BasicCopy()
        {
            return (Mech)Activator.CreateInstance(this.GetType());
        }        
    }
}
