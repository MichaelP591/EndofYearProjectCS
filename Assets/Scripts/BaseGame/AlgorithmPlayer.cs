using System.Collections.Generic;
using BaseGame;
using Unity.VisualScripting;
using UnityEngine;

namespace BaseGame
{
    public class AlgorithmPlayer : Player 
    {
        private int totalSpent = 0;
        [SerializeField] GameObject round = new GameObject();
        private readonly System.Random random = new System.Random();
        public override void MakeMove()
        {
            
        }
        double HandPotential() 
        {
            return 0.0;
        }
        double HandStrength(Card[] cards, List<Card> boardcards) {
            return 0.0;
        }
        int PotSize()
        {
            return -1;
        }
        public override int GetHighestCard()
        {
            return -1;
        }
        public override int FindPair()
        {
            throw new System.NotImplementedException();
        }

        public override int FindTwoPair()
        {
            throw new System.NotImplementedException();
        }

        public override int FindThreeOfKind()
        {
            throw new System.NotImplementedException();
        }

        public override int FindStraight()
        {
            throw new System.NotImplementedException();
        }

        public override string FindFlush()
        {
            throw new System.NotImplementedException();
        }

        public override int FindFullHouse()
        {
            throw new System.NotImplementedException();
        }

        public override int FindFourOfKind()
        {
            throw new System.NotImplementedException();
        }

        public override int FindStraightFlush()
        {
            throw new System.NotImplementedException();
        }

        public override bool HasRoyalFlush()
        {
            throw new System.NotImplementedException();
        }
    }
}
