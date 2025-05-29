using BaseGame;

namespace BaseGame
{
    public abstract class Player
    {

        protected int balance;  // Added protected field for balance
        private bool inGame = true;

        private Card[] cards = new Card[2];

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
            balance = 0;
            return balance;
        }

        public void Check()
        {
            
        }

        public int GetBalance()
        {
            return balance;
        }

        public Card[] GetHand()
        {
            return cards;
        }

        public void AddBalance(int amount)
        {
            balance += amount;
        }

        public void SetBalance(int amount)
        {
            balance = amount;
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