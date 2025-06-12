using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class HandManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Canvas mainCanvas; // Reference to your main canvas
    private List<GameObject> handCards = new();
    public Transform canvasTransform;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
        }
    }

    private void DrawCard()
    {
        if (handCards.Count >= maxHandSize)
        {
            return;
        }
        GameObject g = Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation, canvasTransform);
        handCards.Add(g);
        UpdateCardPositions();
        g.transform.SetSiblingIndex(handCards.Count - 1); // Fixed: was using undefined 'i'
    }
    
    private void UpdateCardPositions()
    {
        if (handCards.Count == 0)
        {
            return;
        }
        
        float cardSpacing = 1f / maxHandSize;
        float firstCardPosition = 0.5f - (handCards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline; // Fixed: Use splineContainer.Spline instead
        
        for (int i = 0; i < handCards.Count; i++)
        {
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splineWorldPosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            
            // Convert world position to canvas local position
            Vector2 canvasLocalPosition = WorldToCanvasPosition(splineWorldPosition);
            
            // Calculate rotation for UI element
            Quaternion rotation = Quaternion.LookRotation(forward, up);
            
            // Get the RectTransform component
            RectTransform cardRect = handCards[i].GetComponent<RectTransform>();
            
            // Animate to canvas position instead of world position
            cardRect.DOLocalMove(canvasLocalPosition, 0.25f);
            cardRect.DOLocalRotateQuaternion(rotation, 0.25f);
        }
    }
    
    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        // Convert world position to screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        
        // Convert screen position to canvas local position
        RectTransform canvasRect = mainCanvas.GetComponent<RectTransform>();
        Vector2 localPosition;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out localPosition
        );
        
        return localPosition;
    }
}