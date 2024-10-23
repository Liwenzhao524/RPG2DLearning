using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable ()
    {
        UpdateSlotUI(item);
    }

    private void OnValidate ()
    {
        if (item == null || item.itemData == null) return;

        gameObject.name = "Craft - " + item.itemData.itemName;    
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.itemData as ItemData_Equipment;

        if (Inventory.instance.CanCraft(craftData, craftData.craftMaterials))
        {
            
        }
    }
}
