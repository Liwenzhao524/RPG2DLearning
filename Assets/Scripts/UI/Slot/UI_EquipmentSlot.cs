using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;
    Sprite _temp;

    private void OnValidate ()
    {
        gameObject.name = "Equipment - " + equipmentType.ToString();
    }

    private void Start ()
    {
        _temp = itemIcon.sprite;
    }

    public override void CleanUpSlot ()
    {
        item = null;
        itemIcon.sprite = _temp;
        itemUI.text = "";
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        if (item == null || item.itemData == null) return;

        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
        //Inventory.instance.AddItem(item.item as ItemData_Equipment);
        CleanUpSlot();
    }

    public override void OnPointerEnter (PointerEventData eventData)
    {
        base.OnPointerEnter (eventData);
    }

    public override void OnPointerExit (PointerEventData eventData)
    {
        base .OnPointerExit (eventData);
    }
}
