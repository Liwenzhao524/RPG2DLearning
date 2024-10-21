using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ݷ�װ�� inventory������һ������
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
