using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class GameHandler
    {
        public static Random randomGenerator = new Random();

        public MinionPool pool;

        public List<Player> players;
        public int maxMana = 10;

        public GameHandler()
        {
            this.players = new List<Player>();
            this.pool = new MinionPool();
        }

        public void AddMech(string name)
        {
            this.players.Add(new Player(name));

            //do some other stuff later
        }
        public void RemoveMech(int index)
        {
            if (index >= this.players.Count()) return;

            this.players.RemoveAt(index);
            
            //do some other stuff later
        }
        public void StartNewGame()
        {
            this.maxMana = 10;
            this.pool = new MinionPool();
            this.pool.FillGenericMinionPool();

            for (int i=0; i<this.players.Count(); i++)
            {                
                this.players[i] = new Player(this.players[i].name);                

                this.players[i].shop.Refresh(ref this.pool, this.maxMana);
                this.players[i].curMana = this.maxMana;
            }

            //do other stuff like matching later
        }
    }

    public class MechFighting
    {
        public static void StartBattle(ref GameHandler gameHandler, int mech1, int mech2)
        {
            //check for any exceptions
            if (Math.Max(mech1, mech2) >= gameHandler.players.Count()) return;
            if (mech1 == mech2) { Console.WriteLine($"{gameHandler.players[mech1].name} tried to fight itself."); return; }

            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;

            //TODO: Output the information somewhere, somehow :)

            //save the data so it reverts after combat
            CreatureData crData1 = gameHandler.players[mech1].creatureData;
            CreatureData crData2 = gameHandler.players[mech2].creatureData;

            int prStat1 = crData1.staticKeywords[StaticKeyword.Rush] - crData1.staticKeywords[StaticKeyword.Taunt];
            int prStat2 = crData2.staticKeywords[StaticKeyword.Rush] - crData2.staticKeywords[StaticKeyword.Taunt];

            //false = mech1 wins, true = mech2 wins
            bool result;
            //for output purposes

            bool coinflip = false;
            Console.WriteLine($"Coinflip: {coinflip}");


            //see who has bigger priority
            if (prStat1 > prStat2) result = false;
            else if (prStat1 < prStat2) result = true;
            //if tied, check the tiebreaker
            else if (crData1.staticKeywords[StaticKeyword.Tiebreaker] > crData2.staticKeywords[StaticKeyword.Tiebreaker]) result = false;
            else if (crData1.staticKeywords[StaticKeyword.Tiebreaker] < crData2.staticKeywords[StaticKeyword.Tiebreaker]) result = true;
            //roll random
            else
            {
                coinflip = true;
                if (GameHandler.randomGenerator.Next(0, 2) == 0) result = false;
                else result = true;
            }

            if (result == true)
            {
                GeneralFunctions.Swap<int>(ref mech1, ref mech2);
                GeneralFunctions.Swap<CreatureData>(ref crData1, ref crData2);
            }

            //output attack priority somewhere, somehow :)

            //trigger Start of Combat effects
            for (int i = 0; i < gameHandler.players[mech1].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
            {
                gameHandler.players[mech1].attachedMechs[i].StartOfCombat(ref gameHandler, mech1, mech2);
            }
            for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
            {
                gameHandler.players[mech2].attachedMechs[i].StartOfCombat(ref gameHandler, mech2, mech1);
            }

            for (int curAttacker = 0; gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); curAttacker++)
            {
                int attacker, defender;
                if (curAttacker % 2 == 0)
                {
                    attacker = mech1;
                    defender = mech2;
                }
                else
                {
                    attacker = mech2;
                    defender = mech1;
                }

                gameHandler.players[mech1].AttackMech(ref gameHandler, attacker, defender);

                if (!gameHandler.players[mech1].IsAlive() || !gameHandler.players[mech2].IsAlive()) break;

                //add an AfterThisAttacks
            }

            if (gameHandler.players[mech1].IsAlive()) Console.WriteLine($"{gameHandler.players[mech1].name} has won!");
            else Console.WriteLine($"{gameHandler.players[mech2].name} has won!");

            //revert to before the fight
            gameHandler.players[mech1].creatureData = crData1;
            gameHandler.players[mech2].creatureData = crData2;

            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;
        }
    }
}
