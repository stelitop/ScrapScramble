using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    public class PackageHandler
    {
        public static Dictionary<UpgradePackage, string> PackageAttributeToString = new Dictionary<UpgradePackage, string>()
        {
            { UpgradePackage.Classic, "Classic" },
            { UpgradePackage.IronmoonFaire, "Ironmoon Faire" },
            { UpgradePackage.EdgeOfScience, "Edge of Science" },
            { UpgradePackage.JunkAndTreasures, "Junk & Treasures" },
            { UpgradePackage.MonstersReanimated, "Monsters Reanimated" },
            { UpgradePackage.ScholomanceAcademy, "Scholomance Academy" },
            { UpgradePackage.TinyInventions, "Tiny Inventions" },
            { UpgradePackage.VentureCo, "Venture Co." },
            { UpgradePackage.WarMachines, "War Machines" },
        };

        public static Dictionary<string, UpgradePackage> StringToPackageAttribute = PackageAttributeToString.ToDictionary((x) => x.Value, (x) => x.Key);
        public static Dictionary<string, UpgradePackage> LowercaseToPackageAttribute = PackageAttributeToString.ToDictionary((x) => x.Value.ToLower(), (x) => x.Key);

        public Dictionary<string, List<Upgrade> > Packages { get; private set; }

        public PackageHandler()
        {
            this.LoadPackages();
        }

        protected void LoadPackages()
        {            
            this.Packages = new Dictionary<string, List<Upgrade>>();

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

                if (!Attribute.IsDefined(m.GetType(), typeof(PackageAttribute))) continue;

                var package = ((PackageAttribute)Attribute.GetCustomAttribute(m.GetType(), typeof(PackageAttribute))).Package;

                string packageName;
                if (!PackageHandler.PackageAttributeToString.ContainsKey(package)) packageName = $"{package} (No Parser Found)";
                else packageName = PackageHandler.PackageAttributeToString[package];

                if (this.Packages.ContainsKey(packageName))
                {
                    this.Packages[packageName].Add(m);
                }
                else
                {
                    this.Packages.Add(packageName, new List<Upgrade>() { m });
                }
            }

            foreach (var package in this.Packages)
            {
                package.Value.Sort();
            }
        }
    }
}
