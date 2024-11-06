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
    [SerializeField] Sprite _backGround;

    private void OnValidate ()
    {
        gameObject.name = "Equipment - " + equipmentType.ToString();
    }

    public override void CleanUpSlot ()
    {
        item = null;
        itemIcon.sprite = _backGround;

        itemUI.text = "";
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        if (item == null || item.itemData == null) return;

        Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);

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
