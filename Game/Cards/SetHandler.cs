using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [Serializable]
    public class SetHandler
    {
        public static Dictionary<UpgradeSet, string> SetAttributeToString = new Dictionary<UpgradeSet, string>()
        {
            { UpgradeSet.Classic, "Classic" },
            { UpgradeSet.IronmoonFaire, "Ironmoon Faire" },
            { UpgradeSet.EdgeOfScience, "Edge of Science" },
            { UpgradeSet.JunkAndTreasures, "Junk & Treasures" },
            { UpgradeSet.MonstersReanimated, "Monsters Reanimated" },
            { UpgradeSet.ScholomanceAcademy, "Scholomance Academy" },
            { UpgradeSet.TinyInventions, "Tiny Inventions" },
            { UpgradeSet.VentureCo, "Venture Co." },
            { UpgradeSet.WarMachines, "War Machines" },
        };

        public static Dictionary<string, UpgradeSet> StringToSetAttribute = SetAttributeToString.ToDictionary((x) => x.Value, (x) => x.Key);
        public static Dictionary<string, UpgradeSet> LowercaseToSetAttribute = SetAttributeToString.ToDictionary((x) => x.Value.ToLower(), (x) => x.Key);

        public Dictionary<string, List<Upgrade> > Sets { get; private set; }

        public SetHandler()
        {
            this.LoadSets();
        }

        protected void LoadSets()
        {            
            this.Sets = new Dictionary<string, List<Upgrade>>();

            var allMechClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(UpgradeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<UpgradeAttribute>() };

            foreach (var x in allMechClasses)
            {
                Upgrade m = (Upgrade)(Activator.CreateInstance(x.Type));

                if (!Attribute.IsDefined(m.GetType(), typeof(SetAttribute))) continue;

                var package = ((SetAttribute)Attribute.GetCustomAttribute(m.GetType(), typeof(SetAttribute))).Set;

                string packageName;
                if (!SetHandler.SetAttributeToString.ContainsKey(package)) packageName = $"{package} (No Parser Found)";
                else packageName = SetHandler.SetAttributeToString[package];

                if (this.Sets.ContainsKey(packageName))
                {
                    this.Sets[packageName].Add(m);
                }
                else
                {
                    this.Sets.Add(packageName, new List<Upgrade>() { m });
                }
            }

            foreach (var package in this.Sets)
            {
                package.Value.Sort();
            }
        }
    }
}
