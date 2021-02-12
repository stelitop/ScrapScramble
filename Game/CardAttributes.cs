using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class UpgradeAttribute : Attribute
    {
        public UpgradeAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SpellAttribute : Attribute
    {
        public SpellAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TokenAttribute : Attribute
    {
        public TokenAttribute()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SparePartAttribute : Attribute
    {
        public SparePartAttribute()
        {

        }
    }

    public enum UpgradePackage
    {
        Classic = 0,
        VentureCo = 1,
        JunkAndTreasures = 2,
        IronmoonFaire = 3,
        EdgeOfScience = 4,
        ScholomanceAcademy = 5,
        MonstersReanimated = 6,
        WarMachines = 7,
        TinyInventions = 8,
    }


    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PackageAttribute : Attribute
    {
        public UpgradePackage Package { get; }

        public PackageAttribute(UpgradePackage package = UpgradePackage.Classic)
        {
            Package = package;
        }
    }
}
