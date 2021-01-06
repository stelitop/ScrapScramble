using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [MechAttribute]
    public class Naptron : Mech
    {
        public Naptron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Naptron";
            this.cardText = "Taunt x2. Aftermath: Give your Mech Rush x2.";
            this.creatureData = new CreatureData(4, 1, 10);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] += 2;
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            Console.WriteLine("1");
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Rush] += 2;
        }
    }

    [MechAttribute]
    public class HighRoller : Mech
    {
        public HighRoller()
        {
            this.rarity = Rarity.Epic;
            this.name = "High Roller";
            this.cardText = "Aftermath: Reduce the Cost of a random Upgrade in your shop by (4).";
            this.creatureData = new CreatureData(4, 3, 3);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            Console.WriteLine("1");
            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;
            Console.WriteLine("2");
            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[shop].creatureData.cost -= 4;
            if (gameHandler.players[curPlayer].shop.options[shop].creatureData.cost < 0) gameHandler.players[curPlayer].shop.options[shop].creatureData.cost = 0;   
        }
    }

    [MechAttribute]
    public class FallenReaver : Mech
    {
        public FallenReaver()
        {
            this.rarity = Rarity.Epic;
            this.name = "Fallen Reaver";
            this.cardText = "Aftermath: Destroy 6 random Upgrades in your shop.";
            this.creatureData = new CreatureData(5, 8, 8);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            Console.WriteLine("1");
            for (int i=0; i<6; i++)
            {
                if (gameHandler.players[curPlayer].shop.options.Count() == 0) break;                

                int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
                gameHandler.players[curPlayer].shop.options.RemoveAt(shop);
            }
        }
    }

    [MechAttribute]
    public class Investrotron : Mech
    {
        public Investrotron()
        {
            this.rarity = Rarity.Epic;
            this.name = "Investotron";
            this.cardText = "Aftermath: Transform a random Upgrade in your shop into an Investotron. Give your Mech +4/+4.";
            this.creatureData = new CreatureData(5, 4, 4);
        }

        public override void Aftermath(ref GameHandler gameHandler, int curPlayer, int enemy)
        {
            Console.WriteLine("1");

            if (gameHandler.players[curPlayer].shop.options.Count() == 0) return;
            Console.WriteLine("2");

            int shop = GameHandler.randomGenerator.Next(0, gameHandler.players[curPlayer].shop.options.Count() );
            gameHandler.players[curPlayer].shop.options[shop] = new Investrotron();
            gameHandler.players[curPlayer].creatureData.attack += 4;
            gameHandler.players[curPlayer].creatureData.health += 4;
        }
    }
}

/*

[MechAttribute]
public class NextMech : Mech
{
    public NextMech()
    {
        this.rarity = Rarity.Epic;
        this.name = "";
        this.cardText = "";
        this.creatureData = new CreatureData(0, 0, 0);
    }
}

*/