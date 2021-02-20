using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [TokenAttribute]
    public class Mechathun : Upgrade
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
            if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {this.rarity} - {this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {this.rarity} - {this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Thaws in 1 turn) {ret}";
            else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Thaws in {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
            return ret;
        }

        public override Upgrade BasicCopy(MinionPool pool)
        {
            Upgrade ret = base.BasicCopy(pool);
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

            for (int i = 0; i < gameHandler.players[player].shop.LastIndex; i++)
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
            Card token = new Mechathun();
            return gameHandler.players[player].shop.AddUpgrade((Upgrade)gameHandler.players[player].pool.FindBasicCard(token.name));
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsSeeker : Upgrade
    {
        public MechaThunsSeeker()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Seeker";
            this.cardText = "Battlecry: Your Mecha'thun thaws 1 turn sooner.";
            this.SetStats(2, 1, 2);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]--;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsSlayer : Upgrade
    {
        public MechaThunsSlayer()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Slayer";
            this.cardText = "Rush. Battlecry: Your Mecha'thun thaws 1 turn sooner.";
            this.SetStats(6, 3, 4);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze]--;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsHarbinger : Upgrade
    {
        public MechaThunsHarbinger()
        {
            this.rarity = Rarity.Common;
            this.name = "Mecha'thun's Harbinger";
            this.cardText = "Taunt. Battlecry: Your Mecha'thun thaws 2 turns sooner.";
            this.SetStats(10, 9, 9);
            this.creatureData.staticKeywords[StaticKeyword.Taunt] = 1;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] -= 2;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsLiege : Upgrade
    {
        public MechaThunsLiege()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mecha'thun's Liege";
            this.cardText = "Battlecry: Your Mecha'thun thaws 3 turns sooner. Overload: (3).";
            this.SetStats(12, 12, 12);
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 3;
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] -= 3;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsElder : Upgrade
    {
        public MechaThunsElder()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Elder";
            this.cardText = "Battlecry: Give your Upgrade -1/-1 for each turn your Mecha'thun has left to thaw.";
            this.SetStats(4, 12, 12);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            gameHandler.players[curPlayer].creatureData.attack -= gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze];
            gameHandler.players[curPlayer].creatureData.health -= gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze];

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsLynchpin : Upgrade
    {
        public MechaThunsLynchpin()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Lynchpin";
            this.cardText = "Battlecry: If your Mecha'thun has 5 or fewer turns left to thaw, gain +16 Shields.";
            this.SetStats(8, 8, 8);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);

            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] <= 5)
            {
                gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 16;
            }

            return Task.CompletedTask;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsGenerator : Upgrade
    {
        public MechaThunsGenerator()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mecha'thun's Generator";
            this.cardText = this.writtenEffect = "Aftermath: Add 3 other random Mecha'thun Cultists to your shop.";
            this.SetStats(5, 2, 5);
        }

        public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
        {
            int index = Mechathun.FindInShop(gameHandler, curPlayer);
            if (index == -1) index = Mechathun.AddMechaThun(gameHandler, curPlayer);
        }

        private bool Criteria(Upgrade mech)
        {
            if (mech.name == this.name) return false;

            return mech.name.StartsWith("Mecha'thun");
        }

        public override void AftermathMe(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Upgrade> list = CardsFilter.FilterList<Upgrade>(gameHandler.players[curPlayer].pool.upgrades, this.Criteria);

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
    [Package(UpgradePackage.MonstersReanimated)]
    public class MkIVSuperCobra : Upgrade
    {
        public MkIVSuperCobra()
        {
            this.rarity = Rarity.Rare;
            this.name = "Mk. IV Super Cobra";
            this.cardText = "Rush. Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.writtenEffect = "Aftermath: Destroy a random Upgrade in your opponent's shop.";
            this.SetStats(6, 5, 2);
            this.creatureData.staticKeywords[StaticKeyword.Rush] = 1;
        }

        public override void AftermathEnemy(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (curPlayer == enemy) return;

            if (gameHandler.players[enemy].shop.OptionsCount() == 0) return;

            int index = gameHandler.players[enemy].shop.GetRandomUpgradeIndex();
            gameHandler.players[enemy].shop.RemoveUpgrade(index);

            gameHandler.players[enemy].aftermathMessages.Add($"{gameHandler.players[curPlayer].name}'s Mk. IV Super Cobra destroyed a random upgrade in your shop.");
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MechaThunsLord : Upgrade
    {
        private bool spellburst = true;

        public MechaThunsLord()
        {
            this.rarity = Rarity.Epic;
            this.name = "Mecha'thun's Lord";
            this.cardText = this.writtenEffect = "Spellburst: Give your Mecha'thun +10/+10.";
            this.SetStats(10, 10, 10);
        }

        public override async Task OnPlay(GameHandler gameHandler, int curPlayer, int enemy)
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

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class DungeonDragonling : Upgrade
    {
        public DungeonDragonling()
        {
            this.rarity = Rarity.Epic;
            this.name = "Dungeon Dragonling";
            this.cardText = this.writtenEffect = "Whenever you would take damage, roll a d20. You take that much less damage.";
            this.SetStats(12, 4, 12);
        }

        public override void BeforeTakingDamage(ref int damage, GameHandler gameHandler, int curPlayer, int enemy, ref string msg)
        {
            int red = GameHandler.randomGenerator.Next(1, 21);
            damage -= red;
            if (damage < 0) damage = 0;
            msg += $"reduced to {damage} by Dungeon Dragonling(rolled {red}), ";
        }
    }    


    [TokenAttribute]
    public class HackathaAmalgam : Upgrade
    {
        public HackathaAmalgam()
        {
            this.rarity = Rarity.NO_RARITY;
            this.name = "Hackatha's Amalgam";
            this.cardText = string.Empty;
            this.SetStats(5, 5, 5);
        }
    }

    //[UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class Hackatha : Upgrade
    {
        public Hackatha()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Hackatha";
            this.cardText = "Battlecry: Add a 5-cost 5/5 Amalgam to your hand with the effects of all Upgrades your previous opponent applied last round.";
            this.SetStats(8, 5, 5);
        }

        public override Task Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.currentRound > 1 && gameHandler.pairsHandler.opponents[curPlayer] != curPlayer)
            {
                Card token = new HackathaAmalgam();
                int index = gameHandler.players[curPlayer].hand.AddCard(gameHandler.players[curPlayer].pool.FindBasicCard(token.name));
                HackathaAmalgam amalgam = (HackathaAmalgam)gameHandler.players[curPlayer].hand.At(index);

                try
                {
                    amalgam.cardText = "Has the effects of";
                    int extraTextLength = 0;
                    int extraCards = 0;

                    for (int i = 0; i < gameHandler.players[enemy].playHistory[gameHandler.currentRound - 2].Count(); i++)
                    {
                        Card upgrade = gameHandler.players[enemy].playHistory[gameHandler.currentRound - 2][i];

                        if ((upgrade.GetType().IsSubclassOf(typeof(Upgrade)) || upgrade.GetType() == typeof(Upgrade)) && upgrade.name != "Hackatha" && upgrade.name != "Hackatha's Amalgam")
                        {
                            amalgam.extraUpgradeEffects.Add((Upgrade)upgrade.DeepCopy());

                            if (extraTextLength < 50)
                            {
                                amalgam.cardText += $" {upgrade.name},";
                                extraTextLength += upgrade.name.Length;
                            }
                            else
                            {
                                extraCards++;
                            }
                        }
                    }
                    amalgam.cardText.TrimEnd(',');
                    if (extraCards == 0)
                    {
                        amalgam.cardText += ".";
                    }
                    else
                    {
                        amalgam.cardText += $" and {extraCards} more Upgrades.";
                    }
                }
                catch
                {
                    Console.WriteLine("Hackatha Fucked Up");
                }
            }

            return Task.CompletedTask;
        }
    }
}
