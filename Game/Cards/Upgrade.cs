using ScrapScramble.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    [Serializable]
    public class Upgrade : Card, IComparable<Upgrade>
    {       
        public CreatureData creatureData = new CreatureData();
        public Rarity rarity;

        public bool showEffectInCombat = true;
        public string writtenEffect;

        public Upgrade()
        {
            this.rarity = Rarity.NO_RARITY;
            this.name = string.Empty;
            this.cardText = string.Empty;
            this.writtenEffect = string.Empty;
        }

        protected void SetStats(int cost, int attack, int health)
        {
            this.creatureData.attack = attack;
            this.creatureData.health = health;
            this.Cost = cost;
        }
        //public Upgrade(string name, string cardText, int cost, int attack, int health, Rarity rarity)
        //{
        //    this.SetStats(cost, attack, health);            
        //    this.rarity = rarity;
        //    this.name = name;
        //    this.cardText = cardText;
        //    this.writtenEffect = string.Empty;
        //}        

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret;
            string rarity = $"{this.rarity} - ";
            if (this.rarity == Rarity.NO_RARITY) rarity = string.Empty;

            if (this.cardText.Equals(string.Empty) ) ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Frozen for 1 turn) {ret}";
            else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Frozen for {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
            return ret;
        }

        public override string ToString()
        {
            string ret;
            string rarity = $"{this.rarity} - ";
            if (this.rarity == Rarity.NO_RARITY) rarity = string.Empty;

            if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            return ret;
        }

        //public Upgrade(MechJsonTemplate mechJson)
        //{
        //    this.SetStats();
        //    this.creatureData.cost = mechJson.cost;
        //    this.cardText = mechJson.cardText;
        //    this.creatureData.attack = mechJson.attack;
        //    this.creatureData.health = mechJson.health;
        //    this.name = mechJson.name;         
        //}

        public override async Task<bool> PlayCard(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            await gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy);
            return true;
        }

        public async Task<bool> BuyCard(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            for (int i=0; i<gameHandler.players[curPlayer].attachedMechs.Count(); i++)
            {                
                gameHandler.players[curPlayer].attachedMechs[i].OnBuyingAMech(this, gameHandler, curPlayer, enemy);                
            }

            foreach (var extraEffect in gameHandler.players[curPlayer].extraUpgradeEffects)
            {
                extraEffect.OnBuyingAMech(this, gameHandler, curPlayer, enemy);
            }

            await gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy);
            return true;
        }

        public virtual bool CanBeBought(int shopPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (shopPos >= gameHandler.players[curPlayer].shop.LastIndex) return false;
            if (this.name == BlankUpgrade.name) return false;
            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 0) return false;
            if (this.inLimbo) return false;
            if (this.Cost > gameHandler.players[curPlayer].curMana) return false;

            return true;
        }

        public override bool CanBePlayed(int handPos, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (handPos >= gameHandler.players[curPlayer].hand.LastIndex) return false;
            if (this.name == BlankUpgrade.name) return false;
            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 0) return false;
            if (this.inLimbo) return false;
            if (this.Cost > gameHandler.players[curPlayer].curMana) return false;

            return true;
        }

        //public void PrintInfo()
        //{
        //    Console.WriteLine($"{this.name}: {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health} {} - \"{this.cardText}\"");
        //}
        
        public virtual Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy) {
            return Task.CompletedTask;
        }
        public virtual void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void OnBuyingAMech(Upgrade m, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void Combo(GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void OnBeingFrozen(GameHandler gameHandler, int curPlayer, int enemy) { }

        public virtual void AfterThisTakesDamage(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AfterThisAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void AfterTheEnemyAttacks(int damage, GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg) {}

        public int CompareTo(Upgrade other)
        {
            if (this.rarity > other.rarity) return -1;
            else if (this.rarity < other.rarity) return 1;

            return this.name.CompareTo(other.name);
        }

        public override Card DeepCopy()
        {
            Upgrade ret = (Upgrade)Activator.CreateInstance(this.GetType());            
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;            
            ret.creatureData = this.creatureData.DeepCopy();
            ret.Cost = this.Cost;
            ret.writtenEffect = this.writtenEffect;
            ret.extraUpgradeEffects.Clear();

            for (int i = 0; i < this.extraUpgradeEffects.Count(); i++)
                ret.extraUpgradeEffects.Add((Upgrade)this.extraUpgradeEffects[i].DeepCopy());
            
            return ret;            
        }
        public virtual Upgrade BasicCopy(MinionPool pool)
        {
            Card newCopy = pool.FindBasicCard(this.name);

            if (newCopy.name != BlankUpgrade.name) return (Upgrade)newCopy.DeepCopy();
            return (Upgrade)Activator.CreateInstance(this.GetType());
        }        
    }
}
