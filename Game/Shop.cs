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
        private List<Mech> options;
        public int totalSize { get { return options.Count(); } }

        public Shop()
        {
            this.options = new List<Mech>();
        }

        public int AddUpgrade(Mech m)
        {
            this.options.Add((Mech)m.DeepCopy());

            return this.options.Count() - 1;
        }
        public Mech At(int index)
        {
            if (index < 0 || index >= this.options.Count()) return new BlankUpgrade();
            else if (this.options[index].name == BlankUpgrade.name) return new BlankUpgrade();
            return this.options[index];
        }
        public void RemoveUpgrade(int index)
        {
            if (index < 0 || index >= this.options.Count()) return;

            this.options[index] = new BlankUpgrade();
            this.RemoveLeadingBlankUpgrades();
        }
        public int OptionsCount()
        {
            int ret = 0;
            for (int i=0; i<this.options.Count(); i++)
            {
                if (this.options[i].name != BlankUpgrade.name) ret++;
            }
            return ret;
        }
        public Mech GetRandomUpgrade()
        {
            List<int> indexes = new List<int>();
            for (int i=0; i<options.Count(); i++)
            {
                if (options[i].name != BlankUpgrade.name) indexes.Add(i);
            }

            if (indexes.Count() == 0) return new BlankUpgrade();
            return options[indexes[GameHandler.randomGenerator.Next(0, indexes.Count())]];
        }
        public int GetRandomUpgradeIndex()
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < options.Count(); i++)
            {
                if (options[i].name != BlankUpgrade.name) indexes.Add(i);
            }

            if (indexes.Count() == 0) return -1;
            return indexes[GameHandler.randomGenerator.Next(0, indexes.Count())];
        }
        public void TransformUpgrade(int index, Mech m)
        {
            if (index < 0 || index >= this.options.Count()) return;
            else if (this.options[index].name == BlankUpgrade.name) return;

            this.options[index] = (Mech)m.DeepCopy();
        }
        public List<Mech> GetAllUpgrades()
        {
            List<Mech> ret = new List<Mech>();
            for (int i=0; i<options.Count(); i++)
            {
                if (options[i].name != BlankUpgrade.name) ret.Add(options[i]);
            }
            return ret;
        }
        public List<int> GetAllUpgradeIndexes()
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < options.Count(); i++)
            {
                if (options[i].name != BlankUpgrade.name) ret.Add(i);
            }
            return ret;
        }
        public void Clear()
        {
            this.options.Clear();
        } 
        private void RemoveLeadingBlankUpgrades()
        {
            for (int i=totalSize-1; i>=0; i--)
            {
                if (options[i].name == BlankUpgrade.name)
                {
                    options.RemoveAt(i);
                }
                else return;
            }
        }


        public void Refresh(GameHandler gameHandler, int maxMana, bool decreaseFreeze = true)
        {
            int commons = gameHandler.shopRarities.common, rares = gameHandler.shopRarities.rare, epics = gameHandler.shopRarities.epic, legendaries = gameHandler.shopRarities.legendary;

            List<Mech> kept = new List<Mech>();

            for (int i=0; i<this.options.Count(); i++)
            {
                if (this.options[i].creatureData.staticKeywords[StaticKeyword.Freeze] > 0)
                {                    
                    if (decreaseFreeze) this.options[i].creatureData.staticKeywords[StaticKeyword.Freeze]--;
                    
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

            for (int i = 0; i < gameHandler.pool.mechs.Count(); i++) if (gameHandler.pool.mechs[i].rarity == Rarity.Legendary && gameHandler.pool.mechs[i].cost <= maxMana - 5) subList.Add(gameHandler.pool.mechs[i]);
            for (int i = 0; i < legendaries; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.AddUpgrade(m);
            }

            subList.Clear();
            for (int i = 0; i < gameHandler.pool.mechs.Count(); i++) if (gameHandler.pool.mechs[i].rarity == Rarity.Epic && gameHandler.pool.mechs[i].cost <= maxMana - 5) subList.Add(gameHandler.pool.mechs[i]);
            for (int i = 0; i < epics; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.AddUpgrade(m);
            }

            subList.Clear();
            for (int i = 0; i < gameHandler.pool.mechs.Count(); i++) if (gameHandler.pool.mechs[i].rarity == Rarity.Rare && gameHandler.pool.mechs[i].cost <= maxMana - 5) subList.Add(gameHandler.pool.mechs[i]);
            for (int i = 0; i < rares; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.AddUpgrade(m);
            }

            subList.Clear();
            for (int i = 0; i < gameHandler.pool.mechs.Count(); i++) if (gameHandler.pool.mechs[i].rarity == Rarity.Common && gameHandler.pool.mechs[i].cost <= maxMana - 5) subList.Add(gameHandler.pool.mechs[i]);
            for (int i = 0; i < commons; i++)
            {
                Mech m = subList[GameHandler.randomGenerator.Next(0, subList.Count())];
                this.AddUpgrade(m);
            }

            this.options.Sort();
        }

        public List<string> GetShopInfo(GameHandler gameHandler, int player)
        {
            List<string> retList = new List<string>();

            if (this.options.Count() == 0)
            {
                retList.Add("Your shop is empty.");
                return retList;
            }

            string ret = string.Empty;
            bool lastBlank = false;

            for (int i = 0; i < this.totalSize; i++)
            {
                string newBit = $"{i + 1}) " + this.options[i].GetInfo(gameHandler, player);
                if (this.At(i).name == BlankUpgrade.name) newBit = string.Empty;
                
                if (ret.Length + newBit.Length > 1020)
                {
                    retList.Add(ret);
                    ret = string.Empty;
                }

                ret += newBit;
                if (i != this.totalSize - 1 && !(lastBlank && newBit == string.Empty)) ret += '\n';

                lastBlank = (this.At(i).name == BlankUpgrade.name);
            }
            retList.Add(ret);
            return retList;
        }       
    }

    public class BlankUpgrade : Mech
    {
        public new const string name = "Blank";

        public BlankUpgrade()
        {
            base.name = BlankUpgrade.name;

            this.inLimbo = true;
            this.cardText = string.Empty;
            this.rarity = Rarity.NO_RARITY;
            this.SetStats(0, 0, 0);
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            return string.Empty;
        }
    }
}
