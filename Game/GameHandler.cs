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
        public PairsHandler pairsHandler;
        public int maxMana = 10;

        public CombatOutputCollector combatOutputCollector;

        public int currentRound = 1;
        public int startingLives = 3;
        public int maxManaCap = -1;

        public GameHandler()
        {
            this.players = new List<Player>();
            this.pool = new MinionPool();
            this.pairsHandler = new PairsHandler();
            this.combatOutputCollector = new CombatOutputCollector();
        }

        public void AddPlayer(string name)
        {
            this.players.Add(new Player(name));
            this.pairsHandler.AddPlayer();

            //do some other stuff later
        }
        public void RemovePlayer(int index)
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
                this.players[i].lives = this.startingLives;

                this.players[i].ready = false;
            }

            //do other stuff like matching later
        }        

        public int AlivePlayers()
        {
            int ret = 0;
            for (int i=0; i<this.players.Count(); i++)
            {
                if (this.players[i].lives > 0) ret++;
            }
            return ret;
        }
    }

    public class GameHandlerMethods
    {
        public static void StartBattle(GameHandler gameHandler, int mech1, int mech2)
        {
            //check for any exceptions
            
            if (Math.Max(mech1, mech2) >= gameHandler.players.Count()) return;
            //if (mech1 == mech2) { Console.WriteLine($"{gameHandler.players[mech1].name} tried to fight itself."); return; }
            
            gameHandler.combatOutputCollector.Clear();
            
            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;
            
            //-introductionHeader output
            //gameHandler.combatOutputCollector.introductionHeader.Add($"[{gameHandler.players[mech1].name} vs {gameHandler.players[mech2].name}]");

            //gameHandler.combatOutputCollector.introductionHeader1.Add($"{gameHandler.players[mech1].name} upgraded with:");
            
            if (gameHandler.players[mech1].specificEffects.hideUpgradesInLog)
            {
                gameHandler.combatOutputCollector.introductionHeader1.Add("(Hidden)");
            }
            else 
            {
                for (int i = 0; i < gameHandler.players[mech1].attachedMechs.Count(); i++) 
                    gameHandler.combatOutputCollector.introductionHeader1.Add($"{gameHandler.players[mech1].attachedMechs[i].name}");

                for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count() - gameHandler.players[mech1].attachedMechs.Count(); i++)
                    gameHandler.combatOutputCollector.introductionHeader1.Add(string.Empty);
            }
            
            //gameHandler.combatOutputCollector.introductionHeader2.Add($"\n{gameHandler.players[mech2].name} upgraded with:");

            if (gameHandler.players[mech2].specificEffects.hideUpgradesInLog)
            {
                gameHandler.combatOutputCollector.introductionHeader2.Add("(Hidden)");
            }
            else 
            {
                for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count(); i++)
                    gameHandler.combatOutputCollector.introductionHeader2.Add($"{gameHandler.players[mech2].attachedMechs[i].name}");

                for (int i = 0; i < gameHandler.players[mech1].attachedMechs.Count() - gameHandler.players[mech2].attachedMechs.Count(); i++)
                    gameHandler.combatOutputCollector.introductionHeader2.Add(string.Empty);
            }
            //-introductionHeader output
            
            //save the data so it reverts after combat
            CreatureData crData1 = gameHandler.players[mech1].creatureData.DeepCopy();
            CreatureData crData2 = gameHandler.players[mech2].creatureData.DeepCopy();
            
            gameHandler.combatOutputCollector.introductionHeader1.Add("\n" + gameHandler.players[mech1].GetInfoForCombat(gameHandler));
            gameHandler.combatOutputCollector.introductionHeader2.Add("\n" + gameHandler.players[mech2].GetInfoForCombat(gameHandler));

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

            //-preCombat header               

            if (!coinflip)
            {             
                gameHandler.combatOutputCollector.preCombatHeader.Add($"{gameHandler.players[mech1].name} has Attack Priority.");
                if (gameHandler.players[mech1].specificEffects.invertAttackPriority || gameHandler.players[mech2].specificEffects.invertAttackPriority)
                {
                    gameHandler.combatOutputCollector.preCombatHeader.Add($"Because of a Trick Roomster, {gameHandler.players[mech2].name} has Attack Priority instead.");
                    GeneralFunctions.Swap<int>(ref mech1, ref mech2);
                    CreatureData midCrData = crData1.DeepCopy();
                    crData1 = crData2.DeepCopy();
                    crData2 = midCrData.DeepCopy();
                }
            }
            else gameHandler.combatOutputCollector.preCombatHeader.Add($"{gameHandler.players[mech1].name} wins the coinflip for Attack Priority.");

            gameHandler.combatOutputCollector.goingFirst = mech1;

            //trigger Start of Combat effects
            for (int multiplier = 0; multiplier < gameHandler.players[mech1].specificEffects.multiplierStartOfCombat; multiplier++)
                for (int i = 0; i < gameHandler.players[mech1].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
                {
                    gameHandler.players[mech1].attachedMechs[i].StartOfCombat(gameHandler, mech1, mech2);
                }

            for (int multiplier = 0; multiplier < gameHandler.players[mech2].specificEffects.multiplierStartOfCombat; multiplier++)
                for (int i = 0; i < gameHandler.players[mech2].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
                {
                    gameHandler.players[mech2].attachedMechs[i].StartOfCombat(gameHandler, mech2, mech1);
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

                int dmg = gameHandler.players[attacker].AttackMech(gameHandler, attacker, defender);

                if (!gameHandler.players[mech1].IsAlive() || !gameHandler.players[mech2].IsAlive()) break;

                for (int i = 0; i < gameHandler.players[attacker].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
                {
                    gameHandler.players[attacker].attachedMechs[i].AfterThisAttacks(dmg, gameHandler, attacker, defender);
                }

                for (int i = 0; i < gameHandler.players[defender].attachedMechs.Count() && gameHandler.players[mech1].IsAlive() && gameHandler.players[mech2].IsAlive(); i++)
                {
                    gameHandler.players[defender].attachedMechs[i].AfterTheEnemyAttacks(dmg, gameHandler, attacker, defender);
                }
            }

            if (gameHandler.players[mech1].IsAlive())
            {
                gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[mech1].name} has won!");                
                gameHandler.players[mech2].lives--;

                gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count-1][mech1] = FightResult.WIN;
                gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count-1][mech2] = FightResult.LOSS;
            }
            else
            {
                gameHandler.combatOutputCollector.combatHeader.Add($"{gameHandler.players[mech2].name} has won!");
                gameHandler.players[mech1].lives--;

                gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count-1][mech1] = FightResult.LOSS;
                gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count-1][mech2] = FightResult.WIN;
            }

            //-combat header


            Console.WriteLine("Frog2");


            //revert to before the fight
            gameHandler.players[mech1].creatureData = crData1.DeepCopy();
            gameHandler.players[mech2].creatureData = crData2.DeepCopy();

            gameHandler.players[mech1].destroyed = false;
            gameHandler.players[mech2].destroyed = false;
        }

        public static void NextRound(GameHandler gameHandler)
        {
            gameHandler.maxMana += 5;
            if (gameHandler.maxManaCap > 0) gameHandler.maxMana = Math.Min(gameHandler.maxMana, gameHandler.maxManaCap);
            //delete aftermath msgs which haven't been implemented yet Lol!

            for (int i = 0; i < gameHandler.players.Count(); i++)
            { 
                //add to history

                gameHandler.players[i].aftermathMessages.Clear();

                gameHandler.players[i].shop.Refresh(gameHandler.pool, gameHandler.maxMana);

                gameHandler.players[i].overloaded = gameHandler.players[i].creatureData.staticKeywords[StaticKeyword.Overload];
                gameHandler.players[i].curMana = gameHandler.maxMana - gameHandler.players[i].overloaded;

                gameHandler.players[i].ready = false;

                gameHandler.players[i].playHistory.Add(new List<Cards.Card>());
                gameHandler.players[i].boughtThisTurn.Clear();

                gameHandler.players[i].creatureData.InitStaticKeywordsDictionary();
                
            }

            for (int i = 0; i < gameHandler.players.Count(); i++)
            {
                for (int j = 0; j < gameHandler.players[i].attachedMechs.Count(); j++)
                {
                    gameHandler.players[i].attachedMechs[j].AftermathMe(gameHandler, i, gameHandler.pairsHandler.opponents[i]);
                }
            }

            for (int i = 0; i < gameHandler.players.Count(); i++)
            {
                for (int j = 0; j < gameHandler.players[i].attachedMechs.Count(); j++)
                {
                    gameHandler.players[i].attachedMechs[j].AftermathEnemy(gameHandler, i, gameHandler.pairsHandler.opponents[i]);
                }
            }

            for (int i=0; i<gameHandler.players.Count(); i++)
            {
                gameHandler.players[i].attachedMechs.Clear();
                gameHandler.players[i].hand.RemoveAllBlankUpgrades();
                gameHandler.players[i].specificEffects = new SpecificEffects();
            }
        }
    }
}
