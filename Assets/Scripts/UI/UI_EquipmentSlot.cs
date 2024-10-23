using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate ()
    {
        gameObject.name = "Equipment - " + equipmentType.ToString();
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        //Inventory.instance.AddItem(item.item as ItemData_Equipment);
        CleanUpSlot();
    }
}
