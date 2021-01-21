using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class SpecificEffects
    {
        public bool hideUpgradesInLog;
        public bool invertAttackPriority;
        public bool ignoreSpikes, ignoreShields;
        public int multiplierStartOfCombat;

        public SpecificEffects()
        {
            hideUpgradesInLog = false;
            invertAttackPriority = false;
            ignoreShields = false;
            ignoreSpikes = false;
            multiplierStartOfCombat = 1;
        }
    }
}
