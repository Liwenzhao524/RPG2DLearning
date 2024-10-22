using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
/// <summary>
/// 数据封装类 根据Itemdata创建的可以放进inventory内的物体
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
