using Newtonsoft.Json;
using ScrapScramble.Game.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{    
    public class MinionPool
    {
        public List<Mech> mechs;
        
        public MinionPool()
        {
            this.mechs = new List<Mech>();
            this.FillGenericMinionPool();
        }        

        public void GenericMinionPollSort()
        {
            this.mechs.Sort();
        }

        public void FillGenericMinionPool()
        {
            this.mechs = new List<Mech>();

            var allMechClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(MechAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<MechAttribute>() };

            foreach (var x in allMechClasses)
            {
                this.mechs.Add((Mech)(Activator.CreateInstance(x.Type)));
            }

            this.GenericMinionPollSort();
        }

        public void PrintMechNames()
        {
            foreach (var x in this.mechs) Console.WriteLine(x.name);
        }

        //public bool LoadMinion(string jsonPath)
        //{
        //    var json = string.Empty;

        //    using (var fs = File.OpenRead(jsonPath))
        //    using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
        //        json = sr.ReadToEnd();

        //    var mechJson = JsonConvert.DeserializeObject<MechJsonTemplate>(json);            

        //    Mech mech = new Mech(mechJson);

        //    this.mechs.Add(mech);

        //    return true;
        //}
    }
}
