using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.Game.Cards.Mechs.Packages
{
    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class ArmOfExotron : Mech
    {
        public ArmOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Arm of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes.";
            this.SetStats(2, 2, 1);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class LegOfExotron : Mech
    {
        public LegOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Leg of Exotron";
            this.cardText = "Battlecry: Gain +2 Shields.";
            this.SetStats(2, 1, 2);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class MotherboardOfExotron : Mech
    {
        public MotherboardOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Motherboard of Exotron";
            this.cardText = "Tiebreaker. Overload: (1)";
            this.SetStats(2, 2, 2);
            this.creatureData.staticKeywords[StaticKeyword.Tiebreaker] = 1;
            this.creatureData.staticKeywords[StaticKeyword.Overload] = 1;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class WheelOfExotron : Mech
    {
        public WheelOfExotron()
        {
            this.rarity = Rarity.Common;
            this.name = "Wheel of Exotron";
            this.cardText = "Battlecry: Gain +2 Spikes and +2 Shields.";
            this.SetStats(2, 1, 1);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Spikes] += 2;
            gameHandler.players[curPlayer].creatureData.staticKeywords[StaticKeyword.Shields] += 2;
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, delegate (Card m) { return m.name.Equals(this.name); });

            string ret = base.GetInfo(gameHandler, player);

            if (list.Count() > 0) ret += " *(played before)*";

            return ret;
        }
    }


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

            for (int i = 0; i < gameHandler.players[player].shop.totalSize; i++)
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
    [Package(UpgradePackage.MonstersReanimated)]
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
    [Package(UpgradePackage.MonstersReanimated)]
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
    [Package(UpgradePackage.MonstersReanimated)]
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

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] -= 2;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
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

            gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] -= 3;
            if (gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] < 0)
                gameHandler.players[curPlayer].shop.At(index).creatureData.staticKeywords[StaticKeyword.Freeze] = 0;
        }
    }

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
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
    [Package(UpgradePackage.MonstersReanimated)]
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
    [Package(UpgradePackage.MonstersReanimated)]
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
    [Package(UpgradePackage.MonstersReanimated)]
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

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class DungeonDragonling : Mech
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

    [UpgradeAttribute]
    [Package(UpgradePackage.MonstersReanimated)]
    public class ExotronTheForbidden : Mech
    {
        public ExotronTheForbidden()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Exotron the Forbidden";
            this.cardText = this.writtenEffect = "Start of Combat: If you've bought all 5 parts of Exotron this game, destroy the enemy Mech.";
            this.SetStats(15, 15, 15);
        }

        private bool Criteria(Card m)
        {
            if (m.name.Equals("Arm of Exotron")) return true;
            if (m.name.Equals("Leg of Exotron")) return true;
            if (m.name.Equals("Motherboard of Exotron")) return true;
            if (m.name.Equals("Wheel of Exotron")) return true;
            return false;
        }

        public override void StartOfCombat(GameHandler gameHandler, int curPlayer, int enemy)
        {
            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[curPlayer].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4)
            {
                gameHandler.players[enemy].destroyed = true;
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden triggers, destroying {gameHandler.players[enemy].name}.");
            }
            else
            {
                gameHandler.combatOutputCollector.preCombatHeader.Add(
                    $"{gameHandler.players[curPlayer].name}'s Exotron the Forbidden fails to trigger.");
            }
        }

        public override string GetInfo(GameHandler gameHandler, int player)
        {
            string ret = base.GetInfo(gameHandler, player);

            List<Card> list = CardsFilter.FilterList<Card>(gameHandler.players[player].playHistory, this.Criteria);

            int arm = 0, leg = 0, mb = 0, wheel = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].name.Equals("Arm of Exotron")) arm = 1;
                else if (list[i].name.Equals("Leg of Exotron")) leg = 1;
                else if (list[i].name.Equals("Motherboard of Exotron")) mb = 1;
                else if (list[i].name.Equals("Wheel of Exotron")) wheel = 1;
            }

            if (arm + leg + mb + wheel == 4) return ret += " *(Ready!)*";
            else if (arm + leg + mb + wheel == 0) return ret += " *(Missing: All)*";
            else
            {
                ret += " *(Missing:";

                if (arm == 0) ret += " Arm,";
                if (leg == 0) ret += " Leg,";
                if (mb == 0) ret += " Motherboard,";
                if (wheel == 0) ret += " Wheel,";

                ret = ret.Remove(ret.Length - 1, 1);
                ret += ")*";
            }

            return ret;
        }
    }


    [TokenAttribute]
    public class HackathaAmalgam : Mech
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
    //[Package(UpgradePackage.MonstersReanimated)]
    public class Hackatha : Mech
    {
        public Hackatha()
        {
            this.rarity = Rarity.Legendary;
            this.name = "Hackatha";
            this.cardText = "Battlecry: Add a 5-Cost 5/5 Amalgam to your hand with the effects of all Upgrades your previous opponent applied last round.";
            this.SetStats(8, 5, 5);
        }

        public override void Battlecry(GameHandler gameHandler, int curPlayer, int enemy)
        {
            if (gameHandler.currentRound > 1 && gameHandler.pairsHandler.opponents[curPlayer] != curPlayer)
            {
                int index = gameHandler.players[curPlayer].hand.AddCard(new HackathaAmalgam());
                HackathaAmalgam amalgam = (HackathaAmalgam)gameHandler.players[curPlayer].hand.At(index);

                try
                {
                    amalgam.cardText = "Has the effects of";
                    int extraTextLength = 0;
                    int extraCards = 0;

                    for (int i = 0; i < gameHandler.players[enemy].playHistory[gameHandler.currentRound - 2].Count(); i++)
                    {
                        Card upgrade = (Mech)gameHandler.players[enemy].playHistory[gameHandler.currentRound - 2][i];

                        if ((upgrade.GetType().IsSubclassOf(typeof(Mech)) || upgrade.GetType() == typeof(Mech)) && upgrade.name != "Hackatha")
                        {
                            amalgam.extraUpgradeEffects.Add((Mech)upgrade.DeepCopy());

                            if (extraTextLength < 40)
                            {
                                amalgam.cardText += $" {upgrade.name},";
                                extraTextLength += upgrade.cardText.Length;
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
        }
    }
}
