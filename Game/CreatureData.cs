using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{    
    public class CreatureData
    {
        public int attack, health;
        public Dictionary<StaticKeyword, int> staticKeywords;

        public void InitStaticKeywordsDictionary()
        {
            this.staticKeywords.Clear();
            foreach (StaticKeyword keyword in Enum.GetValues(typeof(StaticKeyword)))
            {
                this.staticKeywords.Add(keyword, 0);
            }            
        }      

        public CreatureData()
        {
            this.attack = this.health = 1;
            this.staticKeywords = new Dictionary<StaticKeyword, int>();
            this.InitStaticKeywordsDictionary();
        }

        public CreatureData(int attack, int health)
        {
            this.attack = attack;
            this.health = health;
            this.staticKeywords = new Dictionary<StaticKeyword, int>();
            this.InitStaticKeywordsDictionary();
        }
        public string Stats()
        {
            return $"{this.attack}/{this.health}";
        }

        public CreatureData DeepCopy()
        {
            CreatureData ret = new CreatureData(this.attack, this.health);
            ret.staticKeywords = new Dictionary<StaticKeyword, int>(this.staticKeywords);
            return ret;
        }

        public static CreatureData operator +(CreatureData a, CreatureData b)
        {
            CreatureData ret = new CreatureData(a.attack + b.attack, a.health + b.health)
            {
                staticKeywords = a.staticKeywords
            };

            foreach (var kw in b.staticKeywords)
            {
                if (ret.staticKeywords.ContainsKey(kw.Key))
                {
                    ret.staticKeywords[kw.Key] += kw.Value;
                }
                else
                {
                    ret.staticKeywords.Add(kw.Key, kw.Value);
                }
            }

            return ret;
        }
    }
}
