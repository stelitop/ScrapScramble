using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game
{
    public class PairsHandler
    {
        public List<int> opponents { get; }

        public PairsHandler()
        {
            this.opponents = new List<int>();        
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

        public void NextRoundPairs(ref GameHandler gameHandler)
        {
            List<int> players = new List<int>();            

            if (players.Count() == 1)
            {
                this.opponents[0] = 0;
            }
            else if (players.Count() == 2)
            {
                this.opponents[0] = 1;
                this.opponents[1] = 0;
            }
            else
            {
                for (int i = 0; i < gameHandler.players.Count(); i++) players.Add(i);
                players = players.OrderBy(x => GameHandler.randomGenerator.Next()).ToList();

                if (players.Count()%2 == 1)
                {
                    if (players[players.Count()-1] == opponents[players.Count()-1])
                    {
                        NextRoundPairs(ref gameHandler);
                        return;
                    }
                    this.opponents[players[players.Count() - 1]] = players[players.Count() - 1];
                    for (int i=0; i<players.Count()-1; i+=2)
                    {
                        if (this.opponents[players[i]] == players[i+1])
                        {
                            NextRoundPairs(ref gameHandler);
                            return;
                        }
                        this.opponents[players[i]] = players[i+1];
                        this.opponents[players[i+1]] = players[i];
                    }
                }
                else
                {
                    for (int i = 0; i < players.Count(); i += 2)
                    {
                        if (this.opponents[players[i]] == players[i+1])
                        {
                            NextRoundPairs(ref gameHandler);
                            return;
                        }
                        this.opponents[players[i]] = players[i+1];
                        this.opponents[players[i+1]] = players[i];
                    }
                }
            }
        }
    }
}
