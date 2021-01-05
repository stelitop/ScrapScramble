using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public struct MechJsonTemplate
    {
        [JsonProperty("name")]
        public string name;
        [JsonProperty("cardtext")]
        public string cardText;
        [JsonProperty("attack")]
        public int attack;
        [JsonProperty("health")]
        public int health;
        [JsonProperty("cost")]
        public int cost;
    }
}
