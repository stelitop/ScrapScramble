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
            int commons = 4, rares = 3, epics = 2, legendaries = 1;

            List<Mech> kept = new List<Mech>();

            for (int i=0; i<this.options.Count(); i++)
            {
                if (this.options[i].creatureData.staticKeywords[StaticKeyword.Freeze] > 0)
                {                    
                    this.options[i].creatureData.staticKeywords[StaticKeyword.Freeze]--;
                    Console.WriteLine("Frog");
                    kept.Add((Mech)this.options[i].DeepCopy());
                    
                    if (this.options[i].rarity == Rarity.Common) commons--;
                    else if (this.options[i].rarity == Rarity.Rare) rares--;
                    else if (this.options[i].rarity == Rarity.Epic) epics--;
                    else if (this.options[i].rarity == Rarity.Legendary) legendaries--;
                }
            }

            this.options.Clear();
            this.options = kept;

            List<Mech> subList = new List<Mech>();            

            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Legendary && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < legendaries; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add((Mech)m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Epic && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < epics; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];                
                this.options.Add((Mech)m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Rare && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < rares; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add((Mech)m.DeepCopy());
            }

            subList.Clear();
            for (int i = 0; i < pool.mechs.Count(); i++) if (pool.mechs[i].rarity == Rarity.Common && pool.mechs[i].creatureData.cost <= maxMana - 5) subList.Add(pool.mechs[i]);
            for (int i = 0; i < commons; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.options.Add((Mech)m.DeepCopy());
            }

            this.options.Sort();
        }

        public List<string> GetShopInfo(ref GameHandler gameHandler, int player)
        {
            List<string> retList = new List<string>();

            if (this.options.Count() == 0)
            {
                retList.Add("Your shop is empty.");
                return retList;
            }

            string ret = string.Empty;
            for (int i = 0; i < this.options.Count(); i++)
            {
                string newBit = $"{i + 1}) " + this.options[i].GetInfo(ref gameHandler, player);
                //ret += $"{i+1}) " + this.options[i].GetInfo();
                if (ret.Length + newBit.Length > 1020)
                {
                    retList.Add(ret);
                    ret = string.Empty;
                }

                ret += newBit;
                if (i != this.options.Count() - 1) ret += '\n';
            }
            retList.Add(ret);
            return retList;
        }
    }
}
