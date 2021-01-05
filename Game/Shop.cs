using ScrapScramble.Game.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Effects
{
    public class Shop
    {
        public List<Mech> options;

        public Shop()
        {
            this.options = new List<Mech>();
        }

        public void Refresh(ref MinionPool pool, int maxMana)
        {
            this.options.Clear();

            int commons = 4, rares = 3, epics = 2, legendaries = 1;

            List<Mech> subList = new List<Mech>();            

            foreach (var x in pool.mechs) if (x.rarity == Rarity.Legendary && x.creatureData.cost <= maxMana - 5) subList.Add(x);
            for (int i = 0; i < legendaries; i++) this.options.Add(subList[GameHandler.randomGenerator.Next(0, subList.Count() )]);

            subList.Clear();
            foreach (var x in pool.mechs) if (x.rarity == Rarity.Epic && x.creatureData.cost <= maxMana - 5) subList.Add(x);
            for (int i = 0; i < epics; i++) this.options.Add(subList[GameHandler.randomGenerator.Next(0, subList.Count() )]);

            subList.Clear();
            foreach (var x in pool.mechs) if (x.rarity == Rarity.Rare && x.creatureData.cost <= maxMana - 5) subList.Add(x);
            for (int i = 0; i < rares; i++) this.options.Add(subList[GameHandler.randomGenerator.Next(0, subList.Count() )]);

            subList.Clear();
            foreach (var x in pool.mechs) if (x.rarity == Rarity.Common && x.creatureData.cost <= maxMana - 5) subList.Add(x);
            for (int i=0; i<commons; i++) this.options.Add(subList[GameHandler.randomGenerator.Next(0, subList.Count())] );
        }

        public string GetShopInfo()
        {
            if (this.options.Count() == 0) return "Your shop is empty.";
            string ret = string.Empty;
            for (int i = 0; i < this.options.Count(); i++)
            {
                ret += $"{i+1}) " + this.options[i].GetInfo();
                if (i != this.options.Count() - 1) ret += '\n';
            }
            return ret;
        }
    }
}
