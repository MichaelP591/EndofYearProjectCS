using System;
using System.Collections.Generic;
using BaseGame;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace BaseGame
{
    public class AlgorithmPlayer : Player 
    {
        private int totalSpent = 0;
        public Round round;
        public AlgorithmPlayer(Round round) {
            this.round = round;
        }
        private readonly System.Random random = new System.Random();
        public override void MakeBet(Game.BettingRound bettingRound) // Makes a bet based on effective hand strength, pot size, and amount of time spent 
        {
            
            if (bettingRound == Game.BettingRound.PreFlop) Check();
            if (EffectiveHS() < 0.05) Fold();
            if (true) Check();
            if (EffectiveHS() > 0.99) AllIn();
            if (true) Call(0);
            if (true) Raise(0);
        }
        double EffectiveHS() { //Returns a percentile value from 0-1 compared to all other posible hands
            return CurrHS() * (1 - Potential()[0]) + (1 - CurrHS()) * Potential()[1];
        }
        double[] Potential() 
        {
            int[,] HP = new int[3, 3]; 
            int[] HPTotal = new int[3];
            int ourrank = round.GetHighestCard(this);
            int index = 0;
            int ahead = 0;
            int behind = 0;
            int tied = 0;

            PokerCard[,] deck = MakeDeck();

            for (int i = 0; i < deck.GetLength(0); i++)
            {
                for (int j = 0; j < deck.GetLength(1); j++)
                {
                    PokerCard c = deck[i, j];
                    int opprank = Math.Max(GetHighestCard(round.knownCards), GetHighestCard(new List<PokerCard>(cards)));
                    if (ourrank > opprank) index = ahead;
                    else if (ourrank == opprank) index = tied;
                    else index = behind;
                    HPTotal[index] += 1;

                    // All possible board cards to come
                    // for each case(turn, river) {
                    //     // Final 5-card board
                    //     board = [boardcards, turn, river]
                    //     ourbest = Rank(ourcards, board)
                    //     oppbest = Rank(oppcards, board)
                    //     if (ourbest > oppbest) HP[index][ahead] += 1
                    //     else if (ourbest == oppbest) HP[index][tied] += 1
                    //     else HP[index][behind] += 1
                    // }
                }
            }
            // Ppot: were behind but moved ahead
            double Ppot = (HP[behind, ahead] + HP[behind, tied] / 2 + HP[tied, ahead] / 2) / (HPTotal[behind] + HPTotal[tied]);
            // Npot: were ahead but fell behind
            double Npot = (HP[ahead, behind] + HP[tied, behind] / 2 + HP[ahead, tied] / 2) / (HPTotal[ahead] + HPTotal[tied]);
            double[] pot = { Ppot, Npot };
            return pot;
        }
        double CurrHS()
        {
            PokerCard[,] deck = MakeDeck();
            int ahead = 0;
            int behind = 0;
            int tied = 0;
            for (int i = 0; i < deck.GetLength(0); i++)
            {
                for (int j = 0; j < deck.GetLength(1); i++)
                {
                    int opprank = Math.Max(GetHighestCard(round.knownCards), GetHighestCard(new List<PokerCard>(cards)));
                    int ourrank = round.GetHighestCard(this);
                    if (ourrank > opprank) ahead++;
                    else if (ourrank == opprank) tied++;
                    else behind++;
                }
            }
            return (ahead + tied / 2) / (ahead + tied + behind);
        }
        int GetHighestCard(List<PokerCard> cards)
        {
            int max = 0;
            foreach (PokerCard c in cards) if (c.GetCardNumber() > max) max = c.GetCardNumber();
            return max;
        }
        PokerCard[,] MakeDeck()
        {
            PokerCard[,] deck = new PokerCard[4, 13];
            string[] suits = { "diamond", "spade", "heart", "club" };

            for (int i = 0; i < deck.GetLength(0); i++)
                for (int j = 0; j < deck.GetLength(1); i++)
                    deck[i, j] = new PokerCard(j + 2, suits[i]);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    if (deck[i, j] == cards[0] || deck[i, j] == cards[1]) deck[i, j] = null;
                    foreach (PokerCard c in round.knownCards)
                        if (deck[i, j] == c) deck[i, j] = null;
                }
            }
            return deck;
        }
        public int TotalPot(Pot[] pots)
        {
            int totalPotSize = 0;
            foreach (Pot pot in pots) if (pot.EligiblePlayers.Contains(this)) totalPotSize += pot.Amount;
            return totalPotSize;
        }
        protected override int GetHighestCard() { return round.GetHighestCard(this); }
        protected override int FindPair() { return round.FindPair(this); }
        protected override int FindTwoPair() { return round.FindTwoPair(this); }
        protected override int FindThreeOfKind() { return round.FindThreeOfKind(this); }
        protected override int FindStraight(){ { return round.FindTwoPair(this); }}
        protected override string FindFlush() { return round.FindFlush(this); }
        protected override int FindFullHouse() { return round.FindFullHouse(this); }
        protected override int FindFourOfKind() { return round.FindFourOfKind(this); }
        protected override int FindStraightFlush() { return round.FindStraightFlush(this);  }
        protected override bool HasRoyalFlush() { return round.HasRoyalFlush(this); }
    }
}
