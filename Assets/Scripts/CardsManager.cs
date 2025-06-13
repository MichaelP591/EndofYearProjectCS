using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    // Used by Card.cs and CardHolder.cs
    [HideInInspector] public GameObject SelectedCard;
    [HideInInspector] public GameObject HoveringMenu;

    [Header("Prefabs & UI References")]
    public GameObject CardParent;               // Your CardParent prefab
    public GameObject CardFace;                 // Your CardFace prefab
    public Canvas canvas;                       // Your Canvas (for dragging)

    [Header("Hands Layouts")]
    public HorizontalLayoutGroup PlayerLayoutGroup; // Drag your PlayerLayoutGroup here
    public HorizontalLayoutGroup EnemyLayoutGroup;  // Drag your EnemyLayoutGroup here

    [Header("Optional Play Area")]
    public CardHolder DefaultPlayArea;          // If you use a “play/discard” holder

    [Header("Settings")]
    [Range(1, 12)] public int MaxCards = 5;         // Max cards per hand
    public int StartingAmountPlayer = 5;            // How many to deal Player on Start()
    public int StartingAmountEnemy  = 5;            // How many to deal Enemy on Start()

    [Header("Deck Definition")]
    public List<CardType> CardTypes = new List<CardType>();

    // Runtime list of all active card wrappers
    [HideInInspector] public List<GameObject> Cards = new List<GameObject>();

    private void Start()
    {
        // Deal each hand at startup
        if (PlayerLayoutGroup != null && StartingAmountPlayer > 0)
            DealToPlayer(StartingAmountPlayer);

        if (EnemyLayoutGroup != null && StartingAmountEnemy > 0)
            DealToEnemy(StartingAmountEnemy);
    }

    private void Update()
    {
        HandleCardMovements();
    }

    // ──────────────────────────────────────────────────────────
    //  Drag-and-swap logic (exactly as your tutorial used it)
    // ──────────────────────────────────────────────────────────
    private void HandleCardMovements()
    {
        if (SelectedCard == null) return;

        for (int i = 0; i < Cards.Count; i++)
        {
            var other = Cards[i];
            float selX = SelectedCard.transform.position.x;
            float othX = other.transform.position.x;

            int selIdx = SelectedCard.transform.parent.GetSiblingIndex();
            int othIdx = other.transform.parent.GetSiblingIndex();

            if (selX > othX && selIdx < othIdx)
            {
                SwapCards(SelectedCard, other);
                break;
            }
            if (selX < othX && selIdx > othIdx)
            {
                SwapCards(other,  SelectedCard);
                break;
            }
        }
    }

    public void SwapCards(GameObject a, GameObject b)
    {
        Transform pa = a.transform.parent;
        Transform pb = b.transform.parent;

        a.transform.SetParent(pb);
        b.transform.SetParent(pa);

        if (a.GetComponent<Card>()._CardState != Card.CardState.IsDragging)
            a.transform.localPosition = Vector2.zero;
            b.transform.localPosition = Vector2.zero;
    }

    public void PlayCard()
    {
        if (SelectedCard == null || DefaultPlayArea == null || !DefaultPlayArea.available)
            return;

        Transform old = SelectedCard.transform.parent;
        SelectedCard.transform.position = DefaultPlayArea.transform.position;
        SelectedCard.transform.SetParent(DefaultPlayArea.transform);
        Destroy(old.gameObject);
        SelectedCard = null;
    }

    // ──────────────────────────────────────────────────────────
    //  Original “AddCard” from tutorial (deals random cards into Player)
    // ──────────────────────────────────────────────────────────
    public void AddCard(int amount)
    {
        if (PlayerLayoutGroup == null) return;

        for (int i = 0; i < amount; i++)
        {
            if (PlayerLayoutGroup.transform.childCount >= MaxCards) break;

            var wrapper = Instantiate(CardParent, PlayerLayoutGroup.transform);
            int idx = Random.Range(0, CardTypes.Count);
            var cardComp = wrapper.GetComponentInChildren<Card>();
            cardComp.cardType = CardTypes[idx];

            var face = Instantiate(CardFace,
                GameObject.Find("CardVisuals").transform);
            face.GetComponent<CardFace>().target = cardComp.gameObject;
        }
    }

    // ──────────────────────────────────────────────────────────
    //  NEW: Multi‐Hand Dealing Helpers
    // ──────────────────────────────────────────────────────────
    private void DealIntoHand(HorizontalLayoutGroup layout, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (layout.transform.childCount >= MaxCards) break;

            var wrapper = Instantiate(CardParent, layout.transform);
            int idx = Random.Range(0, CardTypes.Count);
            var cardComp = wrapper.GetComponentInChildren<Card>();
            cardComp.cardType = CardTypes[idx];

            var face = Instantiate(CardFace,
                GameObject.Find("CardVisuals").transform);
            face.GetComponent<CardFace>().target = cardComp.gameObject;
        }
    }

    /// <summary>Deal exactly <paramref name="count"/> cards to the Player hand.</summary>
    public void DealToPlayer(int count)
    {
        if (PlayerLayoutGroup != null)
            DealIntoHand(PlayerLayoutGroup, count);
    }

    /// <summary>Deal exactly <paramref name="count"/> cards to the Enemy hand.</summary>
    public void DealToEnemy(int count)
    {
        if (EnemyLayoutGroup != null)
            DealIntoHand(EnemyLayoutGroup, count);
    }

    /// <summary>Draw one card at runtime into the Player hand (e.g. from a UI button).</summary>
    public void DrawToPlayer()
    {
        DealToPlayer(1);
    }

    /// <summary>Draw one card at runtime into the Enemy hand.</summary>
    public void DrawToEnemy()
    {
        DealToEnemy(1);
    }
}
