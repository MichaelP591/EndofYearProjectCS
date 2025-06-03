using System;
using System.Collections.Generic;
using System.Linq;
using BaseGame;
using UnityEditor;

namespace BaseGame
{
    public class HumanPlayer : Player
    {
        List<Card> hand = new List<Card>();
        public override void MakeMove()
        {
            
        }
        public override string FindFlush()
        {
            throw new NotImplementedException();
        }

        public override int FindFourOfKind()
        {
            throw new NotImplementedException();
        }

        public override int FindFullHouse()
        {
            throw new NotImplementedException();
        }

        public override int FindPair()
        {
            return -1;
        }

        public override int FindStraight()
        {
            throw new NotImplementedException();
        }

        public override int FindStraightFlush()
        {
            throw new NotImplementedException();
        }

        public override int FindThreeOfKind()
        {
            throw new NotImplementedException();
        }

        public override int FindTwoPair()
        {
            throw new NotImplementedException();
        }

        public override int GetHighestCard()
        {
            throw new NotImplementedException();
        }

        public override bool HasRoyalFlush()
        {
            throw new NotImplementedException();
        }
    }
}