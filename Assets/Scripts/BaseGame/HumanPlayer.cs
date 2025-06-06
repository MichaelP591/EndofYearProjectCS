using System;
using System.Collections.Generic;
using System.Linq;
using BaseGame;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.XR;

namespace BaseGame
{
    public class HumanPlayer : Player
    {
        List<PokerCard> playerHand = new List<PokerCard>();
        void Start()
        {
            playerHand.Add(cards[0]);
            playerHand.Add(cards[1]);
        }
        void ChooseHand()
        {
            
        }
        public override int MakeMove()
        {
            //implement ui shit here mohit and max
            return -1;
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