using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    Image _itemIcon => GetComponent<Image>();
    TextMeshProUGUI _itemUI => GetComponentInChildren<TextMeshProUGUI>();

    public InventoryItem item;

    public void UpdateSlotUI (InventoryItem newitem)
    {
        item = newitem;

        _itemIcon.color = Color.white;

        if (item != null)
        {
            _itemIcon.sprite = item.itemData.icon;

            if (item.stackSize > 1)
                _itemUI.text = item.stackSize.ToString();
            else if (item.stackSize == 1)
                _itemUI.text = "";
        }
    }

    public void CleanUpSlot ()
    {
        item = null;
        _itemIcon.color = Color.clear;
        _itemIcon.sprite = null;
        _itemUI.text = "";
    }

    public virtual void OnPointerDown (PointerEventData eventData)
    {
        if(item != null)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.instance.RemoveItem(item.itemData);
                return;
            }

            if (item.itemData.itemType == itemType.Equipment)
                Inventory.instance.EquipItem(item.itemData as ItemData_Equipment);
        }
    }
}
