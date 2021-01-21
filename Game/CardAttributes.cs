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
}
