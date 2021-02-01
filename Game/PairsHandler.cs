using ScrapScramble.BotRelated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public enum FightResult
    {
        WIN,
        LOSS,
        BYE
    }

    public class PairsHandler
    {
        public List<int> opponents { get; }
        public List<List<FightResult>> playerResults;
        public PairsHandler()
        {
            this.opponents = new List<int>();
            this.playerResults = new List<List<FightResult>>();
        }

        public void AddPlayer()
        {
            this.opponents.Add(this.opponents.Count());
        }
        public void RemovePlayer(int index)
        {
            this.opponents.RemoveAt(index);

            for (int i=0; i<this.opponents.Count(); i++)
            {
                if (this.opponents[i] == index) this.opponents[i] = i;
                else if (this.opponents[i] > index) this.opponents[i]--;
            }
        }
        public void SetPair(int a, int b)
        {
            if (a < 0 || a >= this.opponents.Count() || b < 0 || b >= this.opponents.Count()) return;

            this.opponents[this.opponents[a]] = this.opponents[a];
            this.opponents[this.opponents[b]] = this.opponents[b];

            this.opponents[a] = b;
            this.opponents[b] = a;
        }

        public void NextRoundPairs(GameHandler gameHandler, int times = 0)
        {
            this.playerResults.Add(new List<FightResult>());
            for (int i=0; i<gameHandler.players.Count(); i++)
            {
                this.playerResults[this.playerResults.Count() - 1].Add(FightResult.BYE);
            }
             
            List<int> players = new List<int>();
            List<int> newOpponents = new List<int>();
            
            for (int i = 0; i < gameHandler.players.Count(); i++)
            {
                newOpponents.Add(i);
                if (gameHandler.players[i].lives > 0) players.Add(i);
            }
                        
            if (players.Count() == 1)
            {
                newOpponents[players[0]] = players[0];
            }
            else if (players.Count() == 2)
            {
                newOpponents[players[0]] = players[1];
                newOpponents[players[1]] = players[0];
            }
            else
            {
                if (players.Count()%2 == 1)
                {
                    players.Sort((x, y) => gameHandler.players[x].lives.CompareTo(gameHandler.players[y].lives));

                    for (int i=0; i<players.Count(); i++)
                    {
                        if (opponents[players[i]] == players[i]) continue;
                        newOpponents[i] = i;
                        players.RemoveAt(i);
                        break;
                    }

                    players = players.OrderBy(x => GameHandler.randomGenerator.Next()).ToList();
                    
                    for (int i=0; i<players.Count()-1; i+=2)
                    {
                        if (this.opponents[players[i]] == players[i+1] && times < 8)
                        {
                            this.playerResults.RemoveAt(this.playerResults.Count() - 1);
                            NextRoundPairs(gameHandler, times+1);
                            return;
                        }
                        newOpponents[players[i]] = players[i+1];
                        newOpponents[players[i+1]] = players[i];
                    }
                }
                else
                {
                    players = players.OrderBy(x => GameHandler.randomGenerator.Next()).ToList();

                    for (int i = 0; i < players.Count(); i += 2)
                    {
                        if (this.opponents[players[i]] == players[i+1] && times < 8)
                        {
                            this.playerResults.RemoveAt(this.playerResults.Count() - 1);
                            NextRoundPairs(gameHandler, times+1);
                            return;
                        }
                        newOpponents[players[i]] = players[i+1];
                        newOpponents[players[i+1]] = players[i];
                    }
                }
            }
            
            this.opponents.Clear();
            for (int i = 0; i < newOpponents.Count(); i++) this.opponents.Add(newOpponents[i]);
            BotInfoHandler.pairsReady = true;
        }
    }
}
