using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardHolder : MonoBehaviour,
                          IPointerEnterHandler,
                          IPointerExitHandler
{
    public CardsManager cardsManager;
    public bool available;
    public bool completed;
    public bool hasToHaveSameNumberOrColor;
    public int maxAmount;
    public int amountToComplete;

    public HolderType holderType;

    public enum HolderType
    {
        Play,
        Discard,
        CardTrader,
        MainHolder
    }

    private void Update()
    {
        HandleCardHolderFunctionality();

        
    }

    public void HandleCardHolderFunctionality()
    {
       

        if (holderType == HolderType.MainHolder)
        {
            available = true;
            completed = transform.childCount == amountToComplete;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (available)
            cardsManager.HoveringMenu = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (available)
            cardsManager.HoveringMenu = null;
    }
}
