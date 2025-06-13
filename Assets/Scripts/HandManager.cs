using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize = 7;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Transform spawnPoint; // Position this in canvas space
    
    // Define your hand curve directly in canvas coordinates
    [SerializeField] private AnimationCurve handCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 0f);
    [SerializeField] private float handWidth = 600f; // Width of hand in canvas units
    [SerializeField] private float handHeight = 100f; // Height of curve in canvas units
    [SerializeField] private Vector2 handCenter = new Vector2(0, -300); // Center position of hand
    
    private List<GameObject> handCards = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveAllCards();
        }
    }

    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize) return;

        // Spawn card at spawn point (in canvas space)
        GameObject card = Instantiate(cardPrefab, mainCanvas.transform);
        RectTransform cardRect = card.GetComponent<RectTransform>();
        
        // Set initial position to spawn point
        if (spawnPoint != null)
        {
            cardRect.position = spawnPoint.position;
        }
        
        handCards.Add(card);
        UpdateCardPositions();
    }

    private void UpdateCardPositions()
    {
        if (handCards.Count == 0) return;

        for (int i = 0; i < handCards.Count; i++)
        {
            // Calculate position along the hand (0 to 1)
            float t = handCards.Count == 1 ? 0.5f : i / (float)(handCards.Count - 1);
            
            // Get position on our custom curve
            Vector2 cardPosition = GetHandPosition(t);
            
            // Get rotation for card fan effect
            float cardRotation = GetHandRotation(t);
            
            // Animate to position
            RectTransform cardRect = handCards[i].GetComponent<RectTransform>();
            cardRect.DOAnchorPos(cardPosition, 0.3f);
            cardRect.DORotate(new Vector3(0, 0, cardRotation), 0.3f);
            
            // Set card order (optional - for visual layering)
            cardRect.SetSiblingIndex(i);
        }
    }

    private Vector2 GetHandPosition(float t)
    {
        // Calculate X position across the hand width
        float x = Mathf.Lerp(-handWidth / 2f, handWidth / 2f, t);
        
        // Calculate Y position using the curve
        float curveValue = handCurve.Evaluate(t);
        float y = curveValue * handHeight;
        
        // Add to hand center position
        return handCenter + new Vector2(x, y);
    }

    private float GetHandRotation(float t)
    {
        // Create a fan effect - cards rotate based on their position
        float maxRotation = 15f; // Maximum rotation in degrees
        return Mathf.Lerp(-maxRotation, maxRotation, t);
    }

    private void RemoveAllCards()
    {
        foreach (GameObject card in handCards)
        {
            if (card != null) DestroyImmediate(card);
        }
        handCards.Clear();
    }

    // Visualize the hand curve in the scene view
    private void OnDrawGizmos()
    {
        if (mainCanvas == null) return;
        
        Gizmos.color = Color.yellow;
        Vector3 lastPoint = Vector3.zero;
        
        for (int i = 0; i <= 20; i++)
        {
            float t = i / 20f;
            Vector2 canvasPos = GetHandPosition(t);
            
            // Convert canvas position to world position for gizmo drawing
            Vector3 worldPos = mainCanvas.transform.TransformPoint(canvasPos);
            
            if (i > 0)
            {
                Gizmos.DrawLine(lastPoint, worldPos);
            }
            lastPoint = worldPos;
        }
    }
}