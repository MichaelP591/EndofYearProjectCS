using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Settings")]
    public CardState _CardState;

    public CardsManager cardsManager;
    public CardType cardType;
    public int cardNumber;

    public enum CardState
    {
        Idle, IsDragging, Played
    }

    [HideInInspector] public bool CanDrag;
    [HideInInspector] public bool Hovering;
    [HideInInspector] public Canvas canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        cardsManager = GameObject.Find("CardsManager").GetComponent<CardsManager>();
        cardsManager.Cards.Add(gameObject);
        CanDrag = true;

        cardNumber = cardType.setAmount == 0 ? Random.Range(0, cardType.MaxCardNumber) : cardType.setAmount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!CanDrag)
            return;

        _CardState = CardState.IsDragging;
        cardsManager.SelectedCard = gameObject;
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag)
            return;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, Input.mousePosition, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _CardState = CardState.Idle;
        cardsManager.SelectedCard = null;

        if (cardsManager.HoveringMenu != null)
        {
            if (cardsManager.HoveringMenu.GetComponent<CardHolder>().holderType == CardHolder.HolderType.CardTrader)
            {
                cardsManager.AddCard(cardNumber);
            }

            Transform target = transform.parent;
            transform.position = cardsManager.HoveringMenu.transform.position;
            transform.SetParent(cardsManager.HoveringMenu.transform);
            Destroy(target.gameObject); // YES: This is part of the original logic
        }
        else
        {
            transform.localPosition = Vector2.zero;
        }

        GetComponent<Image>().raycastTarget = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hovering = true || _CardState == CardState.IsDragging;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hovering = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cardsManager.SelectedCard = gameObject;
    }
}
