using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class CombatOutputCollector
    {
        public List<string> introductionHeader;
        //priority, startofcombat
        public List<string> preCombatHeader;
        public List<string> combatHeader;

        public CombatOutputCollector()
        {
            this.introductionHeader = new List<string>();
            this.preCombatHeader = new List<string>();
            this.combatHeader = new List<string>();
        }

        public void Clear()
        {
            this.introductionHeader.Clear();
            this.preCombatHeader.Clear();
            this.combatHeader.Clear();
        }
    }
}
