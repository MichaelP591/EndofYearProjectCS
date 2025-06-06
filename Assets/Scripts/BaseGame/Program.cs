using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UIElements;
using UnityEngine.UI;

namespace BaseGame.Tests
{
    public class RoundTest
    {
        private static Round round;
        private static HumanPlayer humanPlayer;
        private static PokerCard card1, card2, card3, card4, card5;

        static void Main(string[] args)
        {
            Setup();
            TestPotOperations();
            TestFullHandReturnsNullForNullPlayer();
            TestGetHighestCard();
            TestFindPairReturnsZeroForNoPair();
            TestFindPairDetectsPair();
            TestFindThreeOfKindDetectsThree();
            TestFindFlushDetectsFlush();
            TestGetBestHandRoyalFlush();
            TestDeclareWinnerReturnsSingleWinner();
        }
        public static void Setup()
        {
            humanPlayer = new HumanPlayer();
            round = new Round();
            round.humanPlayer = humanPlayer;

            // Setup some cards for testing
            card1 = new PokerCard(10, "spade");
            card2 = new PokerCard(11, "spade");
            card3 = new PokerCard(12, "spade");
            card4 = new PokerCard(13, "spade");
            card5 = new PokerCard(9, "spade");
        }
        public static void TestPotOperations()
        {
            Assert.AreEqual(0, round.Pot);
            round.Pot += 100;
            Assert.AreEqual(100, round.Pot);
            round.Pot = 50;
            Assert.AreEqual(50, round.Pot);
        }
        public static void TestFullHandReturnsNullForNullPlayer()
        {
            Assert.IsNull(round.FullHand(null));
        }
        public static void TestGetHighestCard()
        {
            var player = new HumanPlayer();
            player.Hand = (new PokerCard[] { new PokerCard(5, "spade"), new PokerCard(8, "heart") });
            Assert.AreEqual(8, round.GetHighestCard(player));
        }
        public static void TestFindPairReturnsZeroForNoPair()
        {
            var player = new HumanPlayer();
            player.Hand = new PokerCard[] { new PokerCard(2, "spade"), new PokerCard(3, "heart") };
            Assert.AreEqual(0, round.FindPair(player));
        }
        public static void TestFindPairDetectsPair()
        {
            var player = new HumanPlayer();
            player.Hand = new PokerCard[] { new PokerCard(4, "spade"), new PokerCard(4, "heart") };
            Assert.AreEqual(4, round.FindPair(player));
        }
        public static void TestFindThreeOfKindDetectsThree()
        {
            var player = new HumanPlayer();
            player.Hand = new PokerCard[] { new PokerCard(7, "spade"), new PokerCard(7, "heart") };
            // Add third card to knownCards
            round.GetType().GetField("knownCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(round, new List<PokerCard> { new PokerCard(7, "club") });
            Assert.AreEqual(7, round.FindThreeOfKind(player));
        }
        public static void TestFindFlushDetectsFlush()
        {
            var player = new HumanPlayer();
            player.Hand = new PokerCard[] { card1, card2 };
            round.GetType().GetField("knownCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(round, new List<PokerCard> { card3, card4, card5 });
            Assert.AreEqual("spade", round.FindFlush(player));
        }
        public static void TestGetBestHandRoyalFlush()
        {
            var player = new HumanPlayer();
            player.Hand = new PokerCard[] { new PokerCard(10, "spade"), new PokerCard(11, "spade") };
            round.GetType().GetField("knownCards", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(round, new List<PokerCard> {
                    new PokerCard(12, "spade"),
                    new PokerCard(13, "spade"),
                    new PokerCard(14, "spade")
                });
            Assert.AreEqual(9, round.GetBestHand(player));
        }
        public static void TestDeclareWinnerReturnsSingleWinner()
        {
            // Setup two players, one with higher pair
            var player1 = new HumanPlayer();
            player1.Hand = new PokerCard[] { new PokerCard(5, "spade"), new PokerCard(5, "heart") };
            var player2 = new HumanPlayer();
            player2.Hand = new PokerCard[] { new PokerCard(8, "spade"), new PokerCard(2, "heart") };

            var players = new List<Player> { player1, player2 };
            round.GetType().GetField("players", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(round, players);

            var winners = round.DeclareWinner();
            Assert.AreEqual(1, winners.Count);
            Assert.AreEqual(player1, winners[0]);
        }
    }
}