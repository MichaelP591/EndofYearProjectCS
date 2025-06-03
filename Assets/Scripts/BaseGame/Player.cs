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
        protected ChipBalance chipBalance = new ChipBalance();
        protected bool inGame = true;
        protected PokerCard[] cards = new PokerCard[2];
        private bool isTurn;
        void Update()
        {
            if (isTurn)
            {
                MakeMove();
            }
        }
        public bool IsTurn { get { return isTurn; }  set { isTurn = value; } }
        public PokerCard[] Hand {
            get { return cards; }
            set 
            { 
                if (value.Length == 2) cards = value; 
                foreach (PokerCard card in cards) {
                    cardsManager.AddCard(card); 
                }
            }    
        }
        public int Balance { get { return balance; } set { balance = value; } }
        public abstract void MakeMove();
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
        public void Fold() {
            inGame = false;
        }

        public int Call(int amount)
        {
            balance -= amount;
            return amount;
        }
        public int Raise(int value) {
            if (value <= balance)
            {
                balance -= value;
                return value;
            }
            return balance;
        }
        public int AllIn()
        {
            int temp = balance;
            balance = 0;
            return temp;
        }
        public int Check()
        {
            return Raise(0);
        }
        public void AddBalance(int amount)
        {
            balance += amount;
        }
        public void RemoveBalance(int amount)
        {
            balance -= amount;
        }
        public bool IsInGame() {
            return inGame;
        }
    }
}