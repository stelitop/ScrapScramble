using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards
{
    public enum Rarity
    {
        Token = -1,
        NO_RARITY = 0,
        Common = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
        Mythic = 5
    }

    public enum SpellRarity
    {
        Spell = 0,
        Spare_Part = 1,
    }
}
