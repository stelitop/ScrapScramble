using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{                          
    [UpgradeAttribute]
    public class OddsEvener : Upgrade
    {
        public OddsEvener()
        {
            this.rarity = Rarity.Epic;
            this.name = "Odds Evener";
            this.cardText = this.writtenEffect = "Aftermath: Give all Mechs who won last round Taunt and those who lost Rush.";
            this.SetStats(5, 5, 5);
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.pairsHandler.playerResults.Count() < 2) return;

            for (int i=0; i<gameHandler.players.Count(); i++)
            {
                if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count()-2][i] == FightResult.WIN)
                {
                    gameHandler.players[i].creatureData.staticKeywords[StaticKeyword.Taunt]++;
                    gameHandler.players[i].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s {this.name} gives you Taunt because you won last round.");
                }
                else if (gameHandler.pairsHandler.playerResults[gameHandler.pairsHandler.playerResults.Count() - 2][i] == FightResult.LOSS)
                {
                    gameHandler.players[i].creatureData.staticKeywords[StaticKeyword.Rush]++;
                    gameHandler.players[i].aftermathMessages.Add(
                        $"{gameHandler.players[curPlayer].name}'s {this.name} gives you Rush because you lost last round.");
                }
            }
        }
    }
}

/*

[UpgradeAttribute]
public class NextMech : Upgrade
{
    public NextMech()
    {
        this.rarity = Rarity.Epic;
        this.name = "";
        this.cardText = "";
        this.SetStats(0, 0, 0);
    }
}

*/