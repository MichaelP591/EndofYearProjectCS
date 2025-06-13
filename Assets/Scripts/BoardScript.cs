using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using BaseGame;

public class BoardScript : MonoBehaviour
{
    [Header("References")]
    public GameObject CardParent;
    [SerializeField] private GameObject CardFace;
    public HorizontalLayoutGroup CommunityCardsLayout;
    [SerializeField] private GameObject CardVisualsParent;

    [Header("Card Management")]
    private bool[] cardFlipped = new bool[5];
    private List<PokerCard> CommunityCards = new List<PokerCard>();
    
    private string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    private string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    private List<(string suit, string value)> usedCards = new List<(string suit, string value)>();

    private void Start()
    {
        InitializeCards();
        DealCommunityCards();
    }

    private void InitializeCards()
    {
        for (int i = 0; i < 5; i++)
        {
            cardFlipped[i] = false;
        }

        if (CardVisualsParent == null)
        {
            CardVisualsParent = GameObject.Find("CardVisuals");
        }
    }

    public void DealCommunityCards()
    {
        // Clear existing cards
        foreach (Transform child in CommunityCardsLayout.transform)
        {
            Destroy(child.gameObject);
        }
        CommunityCards.Clear();
        usedCards.Clear();

        // Create 5 new cards
        for (int i = 0; i < 5; i++)
        {
            var wrapper = Instantiate(CardParent, CommunityCardsLayout.transform);
            var cardComp = wrapper.GetComponentInChildren<PokerCard>();

            if (cardComp != null)
            {
                // Generate random unique card
                string randomSuit;
                string randomValue;
                do
                {
                    randomSuit = suits[Random.Range(0, suits.Length)];
                    randomValue = values[Random.Range(0, values.Length)];
                } while (usedCards.Contains((randomSuit, randomValue)));

                usedCards.Add((randomSuit, randomValue));

                // Set up card
                cardComp.SetSuit(randomSuit);
                cardComp.SetCardNumber(System.Array.IndexOf(values, randomValue) + 2);
                cardComp.IsFaceUp = false;

                // Create visual
                if (CardVisualsParent != null)
                {
                    var face = Instantiate(CardFace, CardVisualsParent.transform);
                    face.GetComponent<CardFace>().target = cardComp.gameObject;
                }

                CommunityCards.Add(cardComp);
            }
        }
    }

    public void FlipCard(int index)
    {
        if (index < 0 || index >= CommunityCards.Count) return;

        var card = CommunityCards[index];
        if (card != null)
        {
            card.IsFaceUp = !card.IsFaceUp;
            cardFlipped[index] = !cardFlipped[index];
        }
    }

    public void RevealFlop()
    {
        for (int i = 0; i < 3; i++)
            FlipCard(i);
    }

    public void RevealTurn()
    {
        FlipCard(3);
    }

    public void RevealRiver()
    {
        FlipCard(4);
    }
}