using UnityEngine;
using System.Collections.Generic;

public class ChipBalance : MonoBehaviour
{
    Dictionary<int, int> chips = new Dictionary<int, int>();
    void Start()
    {
        chips.Add(5, 0);
        chips.Add(10, 0);
        chips.Add(25, 0);
        chips.Add(50, 0);
        chips.Add(100, 0);
    }
    private int balance = 0;
    
}
