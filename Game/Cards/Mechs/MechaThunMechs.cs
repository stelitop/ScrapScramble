using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs
{
    [TokenAttribute]
    public class Mechathun : Mech
    {
        public Mechathun()
        {
            this.rarity = Rarity.Token;
            this.name = "Mecha'thun";
            this.cardText = string.Empty;
            this.SetStats(0, 50, 50);
            this.creatureData.staticKeywords[StaticKeyword.Freeze] = 10;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret = string.Empty;
            if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {this.rarity} - {this.cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {this.rarity} - {this.cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Thaws in 1 turn) {ret}";
            else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Thaws in {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
            return ret;
        }

        public override Mech BasicCopy()
        {
            Mech ret = new Mechathun();
            ret.creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
            return ret;
        }

        /// <summary>
        /// Checks whether ot not Mecha'thun is in a player's shop. Returns the index of the spot if yes. Returns -1 otherwise.
        /// </summary>
        /// <param name="gameHandler"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static int FindInShop(GameHandler gameHandler, int player)
        {
            int ret = -1;

            for (int i=0; i<gameHandler.players[player].shop.totalSize; i++)
            {
                if (gameHandler.players[player].shop.At(i).name.Equals("Mecha'thun"))
                {
                    ret = i;
                    break;
                }
            }

            return ret;            
        }    
        
        public static int AddMechaThun(GameHandler gameHandler, int player)
        {
            return gameHandler.players[player].shop.AddUpgrade(new Mechathun());            
        }
    }

    [UpgradeAttribute]
    public class MechaThunsSeeker : Mech
    {
        public MechaThunsSeeker()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Seeker";
            this.cardText = "Battlecry: Your Mecha'thun thaws 1 turn sooner.";
            this.SetStats(2, 1, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]--;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;            
        }
    }

    [UpgradeAttribute]
    public class MechaThunsSlayer : Mech
    {
        public MechaThunsSlayer()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Slayer";
            this.cardText = "Rush. Battlecry: Your Mecha'thun thaws 1 turn sooner.";
            this.SetStats(6, 3, 4);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]--;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
        }
    }

    [UpgradeAttribute]
    public class MechaThunsHarbinger : Mech
    {
        public MechaThunsHarbinger()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Harbinger";
            this.cardText = "Taunt. Battlecry: Your Mecha'thun thaws 2 turns sooner.";
            this.SetStats(10, 9, 9);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]-=2;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
        }
    }

    [UpgradeAttribute]
    public class MechaThunsLiege : Mech
    {
        public MechaThunsLiege()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mecha'thun's Liege";
            this.cardText = "Battlecry: Your Mecha'thun thaws 3 turns sooner. Overload: (3).";
            this.SetStats(12, 12, 12);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]-=3;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
        }
    }

    [UpgradeAttribute]
    public class MechaThunsElder : Mech
    {
        public MechaThunsElder()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Elder";
            this.cardText = "Battlecry: Give your Mech -1/-1 for each turn your Mecha'thun has left to thaw.";
            this.SetStats(4, 12, 12);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].creatureData.attack -= gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze];
            gameHandler.players[curPlayer].creatureData.health -= gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze];
        }
    }

    [UpgradeAttribute]
    public class MechaThunsLynchpin : Mech
    {
        public MechaThunsLynchpin()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Lynchpin";
            this.cardText = "Battlecry: If your Mecha'thun has 5 or fewer turns left to thaw, gain +16 Shields.";
            this.SetStats(8, 8, 8);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] <= 5)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 16;
            }
        }
    }

    [UpgradeAttribute]
    public class MechaThunsGenerator : Mech
    {
        public MechaThunsGenerator()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Generator";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 other random Mecha'thun Cultists to your shop.";
            this.SetStats(5, 2, 5);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);
        }

        private bool Criteria(Mech mech)
        {
            if (mech.name == this.name) return false;

            return mech.name.StartsWith("Mecha'thun");
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Mech> list = CardsFilter.FilterList<Mech>(gameHandler.pool.mechs, this.Criteria);

            for (int i = 0; i < 3; i++)
            {
                int card = GameHandler.randomGenerator.Next(0, list.Count());

                gameHandler.players[curPlayer].shop.AddUpgrade(list[card]);
            }

            gameHandler.players[curPlayer].aftermathMessages.Add(
                "Your Mecha'thun's Generator adds 3 other random Mecha'thun Cultists to your shop.");
        }
    }

    [UpgradeAttribute]
    public class MechaThunsLord : Mech
    {
        private bool spellburst = true;

        public MechaThunsLord()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mecha'thun's Lord";
            this.cardText = this.writtenEffect = "Spellburst: Give your Mecha'thun +10/+10.";
            this.SetStats(10, 10, 10);
        }

        public override void OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);
        }

        public override void OnSpellCast(Card spell, GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (spellburst)
            {
                spellburst = false;
                this.writtenEffect = string.Empty;

                int index = Mechathun.FindInShop(gameHandler, curPlayer);
                if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

                gameHandler.players[curPlayer].shop.At(index).creatureData.attack += 10;
                gameHandler.players[curPlayer].shop.At(index).creatureData.health += 10;
            }
        }
    }
    
}
