using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble
{
    public class GeneralFunctions
    {
        static public void Swap<T>(ref T x, ref T y)
        {
            T t = x;
            x = y;
            y = t; 
        }
    }
}
