using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> itemDictionary;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start ()
    {
        
    }

    public void AddItem(ItemData item)
    {
        if(itemDictionary.ContainsKey(item))
        {
            itemDictionary[item].AddStack();
        }
        else
        {
            itemDictionary.Add(item, new InventoryItem(item));
        }
    }

    public void RemoveItem(ItemData item)
    {
        if (itemDictionary.ContainsKey(item))
        {
            itemDictionary[item].RemoveStack();
            if(itemDictionary[item].stackSize <= 0) itemDictionary.Remove(item);
        }
    }
}
