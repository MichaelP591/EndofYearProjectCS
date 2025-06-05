using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using BaseGame;

public class CardsManager : MonoBehaviour
{
    [HideInInspector] public GameObject SelectedCard;
    [HideInInspector] public GameObject HoveringMenu;
    [HideInInspector] public CardType cardType;

    [Header("Scripts/Gameobjects")]
    public GameObject CardParent;
    public HorizontalLayoutGroup DefualtCardsLayoutGroup;
    public CardHolder DefaultPlayArea;
    public GameObject SingleCardsParent;
    public GameObject CardFace;
    public Canvas canvas;

    [Header("Settings")]
    [Range(0,12)] public int MaxCards = 2;
    [Range(0, 12)] public int StartingAmount = 2;

    [Header("Lists")]
    public List<CardType> CardTypes = new List<CardType>();
    public List<GameObject> Cards = new List<GameObject>();

    private void Start()
    {
        //Initiate
        if(StartingAmount > 0)
        AddCard(StartingAmount);
    }

    private void Update()
    {
        HandleCardMovments();
    }

    private void HandleCardMovments()
    {
        //Make sure all variables are set
        if (SelectedCard == null)
            return;

        for (int i = 0; i < Cards.Count; i++)
        {
            if (SelectedCard.transform.position.x > Cards[i].transform.position.x)
            {
                if (SelectedCard.transform.parent.GetSiblingIndex() < Cards[i].transform.parent.GetSiblingIndex())
                {
                    SwapCards(SelectedCard, Cards[i]);
                    break;
                }
            }

            if (SelectedCard.transform.position.x < Cards[i].transform.position.x)
            {
                if (SelectedCard.transform.parent.GetSiblingIndex() > Cards[i].transform.parent.GetSiblingIndex())
                {
                    SwapCards(SelectedCard, Cards[i]);
                    break;
                }
            }
        }

    }

    public void PlayCard()
    {
        if (SelectedCard == null)
            return;

        if (DefaultPlayArea.available)
        {
            Transform target = SelectedCard.transform.parent;
            SelectedCard.transform.position = DefaultPlayArea.transform.position;
            SelectedCard.transform.SetParent(DefaultPlayArea.transform);
            Destroy(target.gameObject);

            SelectedCard = null;
        }
    }

    public void SwapCards(GameObject currentCard, GameObject targetCard)
    {
        Transform currentCardParent = currentCard.transform.parent;
        Transform targetedCardParent = targetCard.transform.parent;

        currentCard.transform.SetParent(targetedCardParent);
        targetCard.transform.SetParent(currentCardParent);

        if (currentCard.transform.GetComponent<Card>()._CardState != Card.CardState.IsDragging)
        {
            currentCard.transform.localPosition = Vector2.zero;
        }

        targetCard.transform.localPosition = Vector2.zero;

       
    }
    public void AddCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (DefualtCardsLayoutGroup.transform.childCount < MaxCards)
            {
                GameObject card = Instantiate(CardParent, DefualtCardsLayoutGroup.transform);
                int randomCard = Random.Range(0, CardTypes.Count);

                card.GetComponentInChildren<Card>().cardType = CardTypes[randomCard];
                GameObject cardFace = Instantiate(CardFace, GameObject.Find("CardVisuals").transform);

                cardFace.GetComponent<CardFace>().target = card.GetComponentInChildren<Card>().gameObject;
            }
        }
    }
    public void AddCard(PokerCard pokerCard) 
    {
        if (DefualtCardsLayoutGroup.transform.childCount < MaxCards)
        {
            GameObject card = Instantiate(CardParent, DefualtCardsLayoutGroup.transform);
            int rank = pokerCard.GetCardNumber(); // gives values from 2-14 (2-ace)
            string suit = pokerCard.GetSuit(); // gives "club" or other
            List<string> suitArray = new List<string>{ "diamond", "spade", "heart", "club" };


            card.GetComponentInChildren<Card>().cardType = CardTypes[suitArray.IndexOf(suit) * 13 + (rank - 2)];
            GameObject cardFace = Instantiate(CardFace, GameObject.Find("CardVisuals").transform);

            cardFace.GetComponent<CardFace>().target = card.GetComponentInChildren<Card>().gameObject;
        }
    }
}