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
}
