using System;
using System.Collections.Generic;
using System.Linq;
using BaseGame;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;


namespace BaseGame
{
    public class HumanPlayer : Player
    {
        [SerializeField] private Button foldButton;
        [SerializeField] private Button checkButton;
        [SerializeField] private Button callButton;
        [SerializeField] private Button raiseButton;
        [SerializeField] private Button allInButton;
        PokerCard[] playerHand = new PokerCard[5];
        void Start()
        {
            playerHand[0] = cards[0];
            playerHand[1] = cards[1];

            foldButton.onClick.AddListener(Fold);
            checkButton.onClick.AddListener(Check);
        }
        void ChooseHand()
        {

        }

        public override void MakeBet(PokerGame.BettingRound bettingRound)
        {
            throw new NotImplementedException();
        }

        protected override int GetHighestCard()
        {
            List<PokerCard> hand = playerHand.ToList();
            hand.Sort((a, b) => a.GetCardNumber().CompareTo(b.GetCardNumber()));
            return hand.Last().GetCardNumber();
        }

        protected override int FindPair()
        {
            List<PokerCard> hand = playerHand.ToList();
            hand.Sort((a, b) => a.GetCardNumber().CompareTo(b.GetCardNumber()));
            for (int i = hand.Count() - 1; i > 1; i--)
            {
                if (hand.ElementAt(i).GetCardNumber() == hand.ElementAt(i - 1).GetCardNumber())
                {
                    return hand.ElementAt(i).GetCardNumber();
                }
            }
            return -1;
        }

        protected override int FindTwoPair()
        {
            List<PokerCard> hand = playerHand.ToList();
            hand.Sort((a, b) => a.GetCardNumber().CompareTo(b.GetCardNumber()));
            int firstPair = FindPair();
            int secondPair = 0;
            if (firstPair == -1) return -1;
            for (int i = 0; i < hand.Count() - 1; i++)
            {
                if (hand.ElementAt(i).GetCardNumber() == hand.ElementAt(i + 1).GetCardNumber())
                {
                    secondPair = hand.ElementAt(i).GetCardNumber();
                    continue;
                }
            }
            return -1;
        }

        protected override int FindThreeOfKind()
        {
            throw new NotImplementedException();
        }

        protected override int FindStraight()
        {
            throw new NotImplementedException();
        }

        protected override string FindFlush()
        {
            throw new NotImplementedException();
        }

        protected override int FindFullHouse()
        {
            throw new NotImplementedException();
        }

        protected override int FindFourOfKind()
        {
            throw new NotImplementedException();
        }

        protected override int FindStraightFlush()
        {
            throw new NotImplementedException();
        }

        protected override bool HasRoyalFlush()
        {
            throw new NotImplementedException();
        }
    }
}