using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardTypeScriptibleObject", menuName = "CardType/CardTypeScriptibleObject")]
public class CardType : ScriptableObject
{
    [Header("Info")]
    public Sprite CardIcon;
    public String Suit;
    public int setAmount = 13; // Default to 13 for a standard deck
    public String Value;
    public bool IsFaceDown = true; // Default to face down
    public int MaxCardNumber;
}