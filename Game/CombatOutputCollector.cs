using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class CombatOutputCollector
    {
        public List<string> introductionHeader1, introductionHeader2;
        public List<string> statsHeader;
        //priority, startofcombat
        public List<string> preCombatHeader;
        public List<string> combatHeader;

        public CombatOutputCollector()
        {
            this.introductionHeader1 = new List<string>();
            this.introductionHeader2 = new List<string>();
            this.preCombatHeader = new List<string>();
            this.combatHeader = new List<string>();
            this.statsHeader = new List<string>();
        }

        public void Clear()
        {
            this.introductionHeader1.Clear();
            this.introductionHeader2.Clear();
            this.preCombatHeader.Clear();
            this.combatHeader.Clear();
            this.statsHeader.Clear();
        }
    }
}
