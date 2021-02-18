﻿using Newtonsoft.Json;
using ScrapScramble.Game.Cards;
using ScrapScramble.Game.Cards.Mechs;
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
        public List<Upgrade> mechs;
        public List<Spell> spareparts;
        
        public MinionPool()
        {
            this.mechs = new List<Upgrade>();
            this.spareparts = new List<Spell>();
            this.FillGenericMinionPool();
        }        

        public MinionPool(List<Upgrade> mechs)
        {
            this.mechs = new List<Upgrade>(mechs);
        }

        public void GenericMinionPollSort()
        {
            this.mechs.Sort();
        }

        public void FillGenericMinionPool()
        {
            this.mechs = new List<Upgrade>();

            var allMechClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(UpgradeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<UpgradeAttribute>() };

            foreach (var x in allMechClasses)
            {
                this.mechs.Add((Upgrade)(Activator.CreateInstance(x.Type)));
            }

            this.FillSpareParts();

            this.GenericMinionPollSort();
        }

        public void FillMinionPoolWithPackages(int packageAmount, PackageHandler packageHandler)
        {
            this.mechs = new List<Upgrade>();

            List<string> packagesList = new List<string>();
            foreach (var package in packageHandler.Packages)
            {
                packagesList.Add(package.Key);
            }

            packagesList.Sort((x, y) => GameHandler.randomGenerator.Next(-2, 0)*2+3);

            for (int i=0; i<packageAmount && i <packagesList.Count(); i++)
            {
                for (int j=0; j < packageHandler.Packages[packagesList[i]].Count(); j++)
                {
                    this.mechs.Add((Upgrade)packageHandler.Packages[packagesList[i]][j].DeepCopy());
                }
            }

            this.FillSpareParts();

            this.GenericMinionPollSort();
        }

        public void FillSpareParts()
        {
            this.spareparts = new List<Spell>();

            var allSparePartClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(SparePartAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<SparePartAttribute>() };

            foreach (var x in allSparePartClasses)
            {
                this.spareparts.Add((Spell)(Activator.CreateInstance(x.Type)));
            }
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

        //    Upgrade mech = new Upgrade(mechJson);

        //    this.mechs.Add(mech);

        //    return true;
        //}
    }
}
