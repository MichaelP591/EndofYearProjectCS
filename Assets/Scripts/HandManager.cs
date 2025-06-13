using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween - you'll need to import this package

public class HandManager : MonoBehaviour
{
    [Header("Card Settings")]
    [SerializeField] private RectTransform fromTransform; // Where cards originate from (UI element)
    [SerializeField] private GameObject cardPrefab; // Card prefab (must have RectTransform)
    [SerializeField] private Sprite cardSprite; // Alternative: assign sprite directly
    [SerializeField] private Vector2 cardSize = new Vector2(100, 140); // Card dimensions
    [SerializeField] private float cardOffsetX = 20f; // Horizontal spacing between cards
    [SerializeField] private float rotMaxDegrees = 10f; // Maximum rotation in degrees
    [SerializeField] private float animOffsetY = 0.3f; // Floating animation intensity
    [SerializeField] private float timeMultiplier = 2f; // Speed of floating animation
    
    [Header("Animation Settings")]
    [SerializeField] private float drawDuration = 0.3f; // Base animation duration
    [SerializeField] private float cardDelay = 0.075f; // Delay between each card animation
    [SerializeField] private float floatFadeDuration = 1.5f; // How long to fade in floating effect
    
    private List<RectTransform> cards = new List<RectTransform>();
    private List<Vector2> cardBasePositions = new List<Vector2>(); // Store base positions for floating
    private float time = 0f;
    private float sineOffsetMultiplier = 0f;
    private bool drawn = false;
    private bool isFloating = false;
    
    // This component must be on a Canvas or child of Canvas
    private RectTransform rectTransform;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("CardHandManager must be attached to a GameObject with RectTransform (UI element)");
        }
    }
    
    private void Start()
    {
        // Wait 2 seconds like in the original Godot code
        StartCoroutine(DelayedStart());
    }
    
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2f);
        // You can call DrawCards here if you want to test
        // DrawCards(fromTransform.anchoredPosition, 10);
    }
    
    private void Update()
    {
        if (isFloating && cards.Count > 0)
        {
            time += Time.deltaTime;
            
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i] != null)
                {
                    float sineValue = Mathf.Sin(i + (time * timeMultiplier));
                    Vector2 newPos = cardBasePositions[i];
                    newPos.y += sineValue * sineOffsetMultiplier;
                    cards[i].anchoredPosition = newPos;
                }
            }
        }
    }
    
    public void DrawCards(Vector2 fromPosition, int numberOfCards)
    {
        drawn = true;
        
        // Kill any existing tweens
        DOTween.Kill(this);
        
        // Clear previous data
        cards.Clear();
        cardBasePositions.Clear();
        
        // Create cards
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject cardInstance;
            RectTransform cardRect;
            Image cardImage;
            if (cardPrefab != null)
            {
                // Use prefab
                cardInstance = Instantiate(cardPrefab, transform);
                cardRect = cardInstance.GetComponent<RectTransform>();
            }
            else if (cardSprite != null)
            {
                // Create card from sprite
                cardInstance = new GameObject($"Card_{i}");
                cardInstance.transform.SetParent(transform);
                
                cardRect = cardInstance.AddComponent<RectTransform>();
                cardImage = cardInstance.AddComponent<Image>();
                cardImage.sprite = cardSprite;
                cardImage.color = Color.white;
                
                // Set card size
                cardRect.sizeDelta = cardSize;
            }
            else
            {
                Debug.LogError("Either cardPrefab or cardSprite must be assigned!");
                return;
            }
            
            if (cardRect == null)
            {
                Debug.LogError("Card must have a RectTransform component!");
                Destroy(cardInstance);
                continue;
            }
            
            // Debug: Check if Image component exists and is configured
            cardImage = cardInstance.GetComponent<Image>();
            if (cardImage == null)
            {
                Debug.LogWarning($"Card {i} has no Image component!");
            }
            else if (cardImage.sprite == null)
            {
                Debug.LogWarning($"Card {i} Image has no sprite assigned!");
            }
            else
            {
                Debug.Log($"Card {i} created successfully with sprite: {cardImage.sprite.name}");
                // Ensure the card is fully opaque
                cardImage.color = Color.white;
            }
            
            // Set initial position to origin
            cardRect.anchoredPosition = fromPosition;
            
            // Ensure card stays in UI plane (Z = 0)
            Vector3 localPos = cardRect.localPosition;
            localPos.z = 0f;
            cardRect.localPosition = localPos;
            
            // Calculate final position
            Vector2 finalPosition = CalculateCardPosition(i, numberOfCards, cardRect);
            
            // Calculate rotation
            float rotationAngle = CalculateCardRotation(i, numberOfCards);
            
            // Add to cards list and store base position
            cards.Add(cardRect);
            cardBasePositions.Add(finalPosition);
            
            // Animate to final position with staggered timing
            float animDelay = i * cardDelay;
            cardRect.DOAnchorPos(finalPosition, drawDuration + animDelay)
                .SetDelay(animDelay)
                .SetEase(Ease.InOutCubic);
                
            cardRect.DORotate(new Vector3(0, 0, rotationAngle), drawDuration + animDelay)
                .SetDelay(animDelay)
                .SetEase(Ease.InOutCubic);
        }
        
        // Start floating animation after cards are drawn
        float totalAnimTime = drawDuration + (numberOfCards - 1) * cardDelay;
        DOVirtual.DelayedCall(totalAnimTime, () => {
            isFloating = true;
            DOTween.To(() => sineOffsetMultiplier, x => sineOffsetMultiplier = x, animOffsetY, floatFadeDuration)
                .SetEase(Ease.InOutCubic);
        });
    }
    
    public void UndrawCards(Vector2 toPosition)
    {
        drawn = false;
        isFloating = false;
        
        // Kill any existing tweens
        DOTween.Kill(this);
        
        // Fade out floating effect
        DOTween.To(() => sineOffsetMultiplier, x => sineOffsetMultiplier = x, 0f, 0.9f)
            .SetEase(Ease.InOutCubic);
        
        // Animate cards back to origin in reverse order
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            if (cards[i] != null)
            {
                float animDelay = (cards.Count - i) * cardDelay;
                
                cards[i].DOAnchorPos(toPosition, drawDuration + animDelay)
                    .SetDelay(animDelay)
                    .SetEase(Ease.InOutCubic);
                    
                cards[i].DORotate(Vector3.zero, drawDuration + animDelay)
                    .SetDelay(animDelay)
                    .SetEase(Ease.InOutCubic);
            }
        }
        
        // Clean up cards after animation
        float totalAnimTime = drawDuration + cards.Count * cardDelay;
        DOVirtual.DelayedCall(totalAnimTime, () => {
            foreach (RectTransform card in cards)
            {
                if (card != null)
                    Destroy(card.gameObject);
            }
            cards.Clear();
            cardBasePositions.Clear();
        });
    }
    
    private Vector2 CalculateCardPosition(int cardIndex, int totalCards, RectTransform cardRect)
    {
        // Get card size from RectTransform
        Vector2 cardSize = cardRect.sizeDelta;
        
        // Center the card on its position (equivalent to -(instance.size / 2.0) in Godot)
        Vector2 finalPosition = new Vector2(-cardSize.x / 2f, -cardSize.y / 2f);
        
        // Add horizontal offset for fan formation
        finalPosition.x -= cardOffsetX * (totalCards - 1 - cardIndex);
        
        // Center the entire hand
        finalPosition.x += (cardOffsetX * (totalCards - 1)) / 2f;
        
        return finalPosition;
    }
    
    private float CalculateCardRotation(int cardIndex, int totalCards)
    {
        if (totalCards == 1) return 0f;
        
        float normalizedPosition = (float)cardIndex / (float)(totalCards - 1);
        return Mathf.Lerp(-rotMaxDegrees, rotMaxDegrees, normalizedPosition);
    }
    
    // Button callback - attach this to a UI button
    public void OnDrawButtonPressed()
    {
        if (drawn)
        {
            UndrawCards(fromTransform.anchoredPosition);
        }
        else
        {
            DrawCards(fromTransform.anchoredPosition, 10);
        }
    }
    
    // Alternative method if you want to use it from code
    public void ToggleCards()
    {
        OnDrawButtonPressed();
    }
}