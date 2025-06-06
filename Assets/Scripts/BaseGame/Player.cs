using System;
using System.Collections.Generic;
using BaseGame;
using UnityEngine;
using UnityEngine.UI;

namespace BaseGame
{
    public abstract class Player : MonoBehaviour
    {
        int balance;
        protected bool areCardsShowing = false;
        CardsManager cardsManager = new CardsManager();
        protected bool inGame = true;
        protected PokerCard[] cards = new PokerCard[2];
        private bool isTurn;
        public enum PlayerAction
        {
            Fold,
            Check,
            Call,
            Raise,
            AllIn
        }
        public bool IsTurn { get { return isTurn; } set { isTurn = value; } }
        public int Stack { get; set; }
        public PokerCard[] Hand
        {
            get { return cards; }
            set
            {
                if (value.Length == 2) cards = value;
                foreach (PokerCard card in cards) cardsManager.AddCard(card);
            }
        }
        public int Balance { get { return balance; } set { balance = value; } }
        public PlayerAction LastAction { get; protected set; }
        public int CurrentBet { get; set; }
        public void RevealHand()
        {
            foreach (PokerCard card in Hand)
            {
                card.IsFaceUp = true;
            }
        }
        public void Fold() { LastAction = PlayerAction.Fold; }
        public void Call(int amount)
        {
            balance -= amount;
            LastAction = PlayerAction.Call;
            CurrentBet = amount;
        }
        public void Raise(int value)
        {
            if (value <= balance)
            {
                balance -= value;
                LastAction = PlayerAction.Raise;
                CurrentBet = value;
            }
            else AllIn();
        }
        public void AllIn()
        {
            LastAction = PlayerAction.AllIn;
            CurrentBet = balance;
            balance = 0;
        }
        void HandleAllIn(int amount)
        {
            amount = Math.Min(amount, Stack);
            CurrentBet += amount;
            Stack -= amount;
            if (Stack == 0) LastAction = PlayerAction.AllIn;
        }
        public void Check() { LastAction = PlayerAction.Check; CurrentBet = 0; }
        public abstract int MakeMove();
        public abstract int GetHighestCard();
        public abstract int FindPair();
        public abstract int FindTwoPair();
        public abstract int FindThreeOfKind();
        public abstract int FindStraight();
        public abstract string FindFlush();
        public abstract int FindFullHouse();
        public abstract int FindFourOfKind();
        public abstract int FindStraightFlush(); 
        public abstract bool HasRoyalFlush();
    }
}