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

        public void Refresh(MinionPool pool, int maxMana)
        {
            this.options.Clear();

            int commons = 4, rares = 3, epics = 2, legendaries = 1;

            List<Mech> subList = new List<Mech>();            

            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Legendary && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < legendaries; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add(m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Epic && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < epics; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];                
                this.options.Add(m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Rare && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < rares; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add(m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Common && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < commons; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add(m.DeepCopy());
            }
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
