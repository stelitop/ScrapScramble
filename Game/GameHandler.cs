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
        public List<int> opponents;
        public int maxMana = 10;

        public CombatOutputCollector combatOutputCollector;

        public GameHandler()
        {
            this.players = new List<Player>();
            this.pool = new MinionPool();
            this.opponents = new List<int>();
            this.combatOutputCollector = new CombatOutputCollector();
        }

        public void AddMech(string name)
        {
            this.players.Add(new Player(name));
            this.opponents.Add(this.players.Count() - 1);

            //do some other stuff later
        }
        public void RemoveMech(int index)
        {
            if (index >= this.players.Count()) return;

            this.players.RemoveAt(index);
            
            //do some other stuff later
            //including fixing relative values
        }
        public void StartNewGame()
        {
            this.maxMana = 10;
            this.pool = new MinionPool();
            this.pool.FillGenericMinionPool();

            for (int i=0; i<this.players.Count(); i++)
            {                
                this.players[i] = new Player(this.players[i].name);                

                this.players[i].shop.Refresh(this.pool, this.maxMana);
                this.players[i].curMana = this.maxMana;

                this.players[i].submitted = false;
            }

            //do other stuff like matching later
        }        
    }

    public class GameHandlerMethods
    {
        public static void StartBattle(ref GameHandler gameHandler, int mech1, int mech2)
        {
            //check for any exceptions
            
            if (Math.Max(mech1, mech2) >= gameHandler.players.Count()) return;
            //if (mech1 == mech2) { Console.WriteLine($"{gameHandler.players[mech1].name} tried to fight itself."); return; }

            gameHandler.combatOutputCollector.Clear();
            
            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;

           
            //-introductionHeader output
            gameHandler.combatOutputCollector.introductionHeader.Add($"[{gameHandler.players[mech1].name} vs {gameHandler.players[mech2].name}]");
            gameHandler.combatOutputCollector.introductionHeader.Add($"{gameHandler.players[mech1].name} upgraded with:");
            
            for (int i=0; i<gameHandler.players[mech1].attachedMechs.Count(); i++)
            {
                gameHandler.combatOutputCollector.introductionHeader.Add($"{gameHandler.players[mech1].attachedMechs[i].name}");
            }            

            gameHandler.combatOutputCollector.introductionHeader.Add($"\n{gameHandler.players[mech2].name} upgraded with:");
            for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count(); i++)
            {
                gameHandler.combatOutputCollector.introductionHeader.Add($"{gameHandler.players[mech2].attachedMechs[i].name}");
            }
            //-introductionHeader output
            
            //save the data so it reverts after combat
            CreatureData crData1 = gameHandler.players[mech1].creatureData.DeepCopy();
            CreatureData crData2 = gameHandler.players[mech2].creatureData.DeepCopy();

            gameHandler.players[mech1].GetInfoForCombat(ref gameHandler);
            gameHandler.combatOutputCollector.statsHeader.Add(string.Empty);
            gameHandler.players[mech2].GetInfoForCombat(ref gameHandler);            

            int prStat1 = crData1.staticKeywords[StaticKeyword.Rush] - crData1.staticKeywords[StaticKeyword.Taunt];
            int prStat2 = crData2.staticKeywords[StaticKeyword.Rush] - crData2.staticKeywords[StaticKeyword.Taunt];
            
            //false = mech1 wins, true = mech2 wins
            bool result;
            //for output purposes

            
            bool coinflip = false;

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
                CreatureData midCrData = crData1.DeepCopy();
                crData1 = crData2.DeepCopy();
                crData2 = midCrData.DeepCopy();
            }
           
            //output attack priority somewhere, somehow :)

            //-preCombat header                

            if (!coinflip) gameHandler.combatOutputCollector.preCombatHeader.Add($"{gameHandler.players[mech1].name} has Attack Priority.");
            else gameHandler.combatOutputCollector.preCombatHeader.Add($"{gameHandler.players[mech1].name} wins the coinflip for Attack Priority.");
            
            //trigger Start of Combat effects
            for (int i = 0; i < gameHandler.players[mech1].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
            {
                gameHandler.players[mech1].attachedMechs[i].StartOfCombat(ref gameHandler, mech1, mech2);
            }
            for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
            {
                gameHandler.players[mech2].attachedMechs[i].StartOfCombat(ref gameHandler, mech2, mech1);
            }
            //-preCombat header

            //-combat header
                //the fighting
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

                gameHandler.players[attacker].AttackMech(ref gameHandler, attacker, defender);

                if (!gameHandler.players[mech1].IsAlive() || !gameHandler.players[mech2].IsAlive()) break;

                //add an AfterThisAttacks
            }

            if (gameHandler.players[mech1].IsAlive()) gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[mech1].name} has won!");
            else gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[mech2].name} has won!");

            //-combat header

            //revert to before the fight
            gameHandler.players[mech1].creatureData = crData1.DeepCopy();
            gameHandler.players[mech2].creatureData = crData2.DeepCopy();

            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;
        }

        public static void NextRound(ref GameHandler gameHandler)
        {
            gameHandler.maxMana += 5;
            //delete aftermath msgs which haven't been implemented yet Lol!

            for (int i = 0; i < gameHandler.players.Count(); i++)
            {
                //add to history

                gameHandler.players[i].aftermathMessages.Clear();

                gameHandler.players[i].shop.Refresh(gameHandler.pool, gameHandler.maxMana);

                gameHandler.players[i].overloaded = gameHandler.players[i].creatureData.staticKeywords[StaticKeyword.Overload];
                gameHandler.players[i].curMana = gameHandler.maxMana - gameHandler.players[i].overloaded;

                gameHandler.players[i].submitted = false;

                gameHandler.players[i].creatureData.InitStaticKeywordsDictionary();
                
            }

            for (int i = 0; i < gameHandler.players.Count(); i++)
            {
                for (int j = 0; j < gameHandler.players[i].attachedMechs.Count(); j++)
                {
                    gameHandler.players[i].attachedMechs[j].Aftermath(ref gameHandler, i, gameHandler.opponents[i]);
                }
            }

            for (int i=0; i<gameHandler.players.Count(); i++)
            {
                gameHandler.players[i].attachedMechs.Clear();
            }
        }
    }
}
