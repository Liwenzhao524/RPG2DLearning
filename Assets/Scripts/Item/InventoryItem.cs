using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据封装类 inventory内所有一类物体
/// </summary>
public class InventoryItem  
{
    public ItemData itemData;
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
        AddStack();
    }

    public void AddStack () => stackSize++;
    public void RemoveStack () => stackSize--;
}
