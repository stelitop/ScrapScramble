using ScrapScramble.Game.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public abstract class Mech : Card, IComparable<Mech>
    {
        public CreatureData creatureData;
        public Rarity rarity;

        public string name;
        public string cardText;

        public Mech()
        {
            this.creatureData = new CreatureData();
            this.rarity = Rarity.NO_RARITY;
            this.name = string.Empty;
            this.cardText = string.Empty;
        }
        public Mech(string name, string cardText, int cost, int attack, int health, Rarity rarity)
        {
            this.creatureData = new CreatureData(cost, attack, health);            
            this.rarity = rarity;
            this.name = name;
            this.cardText = cardText;
        }

        public override string GetInfo()
        {
            return $"{this.name} - {this.rarity} Mech - {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";
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

        public override bool PlayCard(int handPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].hand.cards.Count <= handPos) return false;
            if (this.creatureData.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.creatureData.cost;

            gameHandler.players[curPlayer].AttachMech(this, ref gameHandler, curPlayer, enemy);
            return true;
        }

        public override bool BuyCard(int shopPos, ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.players[curPlayer].shop.options.Count() <= shopPos) return false;
            if (this.creatureData.cost > gameHandler.players[curPlayer].curMana) return false;

            gameHandler.players[curPlayer].curMana -= this.creatureData.cost;

            //TODO: add this to memory

            //TODO: OnBuyingMech condition

            gameHandler.players[curPlayer].AttachMech(this, ref gameHandler, curPlayer, enemy);
            return true;
        }

        //public void PrintInfo()
        //{
        //    Console.WriteLine($"{this.name}: {this.creatureData.cost}/{this.creatureData.attack}/{this.creatureData.health} {} - \"{this.cardText}\"");
        //}

        public virtual void Battlecry(ref GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy) { }
        public virtual void StartOfCombat(ref GameHandler gameHandler, int curPlayer, int enemy) { }

        public int CompareTo(Mech other)
        {
            if (this.rarity > other.rarity) return -1;
            else if (this.rarity < other.rarity) return 1;

            return this.name.CompareTo(other.name);
        }
    }
}
