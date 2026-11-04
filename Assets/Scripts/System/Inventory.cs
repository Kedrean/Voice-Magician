
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private HashSet<string> items = new HashSet<string>();

    public bool HasKey => items.Contains("Key");

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Inventory: Added " + itemName);
    }
}
