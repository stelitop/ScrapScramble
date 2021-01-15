using DSharpPlus.Entities;
using ScrapScramble.BotRelated;
using ScrapScramble.Game;
using ScrapScramble.Game.Cards;
using ScrapScramble.Game.Cards.Mechs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble
{
    class Program
    {        
        static void Main(string[] args)
        {   
            var bot = new Bot();            
            bot.RunAsync().GetAwaiter().GetResult();            

            Console.Read();
        }
    }
}
