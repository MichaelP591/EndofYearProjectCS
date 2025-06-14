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
            Fold, Check, Call, Raise, AllIn
        }
        public bool IsTurn { get { return isTurn; } set { isTurn = value; } }
        public int Stack { get; set; }
        public PokerCard[] Hand
        {
            get { return cards; }
            set
            {
                if (value.Length == 2) cards = value;
                ///foreach (PokerCard card in cards) cardsManager.AddCard(card);
            }
        }
        public int Balance { get { return balance; } set { balance = value; } }
        public PlayerAction LastAction { get; set; }
        public int CurrentBet { get; set; }
        public void RevealHand()
        {
            foreach (PokerCard card in Hand)
            {
                card.IsFaceUp = true;
            }
        }
        public void Fold()
        {
            LastAction = PlayerAction.Fold;
            CurrentBet = 0;
            isTurn = false;
        }
        public void Call(int amount)
        {
            balance -= amount;
            LastAction = PlayerAction.Call;
            CurrentBet = amount;
            Stack += amount;
        }
        public void Raise(int value)
        {
            if (value < balance)
            {
                balance -= value;
                LastAction = PlayerAction.Raise;
                CurrentBet = value;
                Stack += value;
            }
            else AllIn();
        }
        public void AllIn()
        {
            CurrentBet = balance;
            Stack += balance;
            balance = 0;
            LastAction = PlayerAction.AllIn;
            isTurn = false;
        }
        
        public void Check() { LastAction = PlayerAction.Check; CurrentBet = 0; }
        public abstract void MakeBet(PokerGame.BettingRound bettingRound);
        protected abstract int GetHighestCard();
        protected abstract int FindPair();
        protected abstract int FindTwoPair();
        protected abstract int FindThreeOfKind();
        protected abstract int FindStraight();
        protected abstract string FindFlush();
        protected abstract int FindFullHouse();
        protected abstract int FindFourOfKind();
        protected abstract int FindStraightFlush(); 
        protected abstract bool HasRoyalFlush();
    }
}