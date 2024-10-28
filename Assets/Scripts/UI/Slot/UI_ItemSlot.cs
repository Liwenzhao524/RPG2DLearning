using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI mainUI => GetComponentInParent<UI>();
    [SerializeField] protected Image itemIcon;
    protected TextMeshProUGUI itemUI => GetComponentInChildren<TextMeshProUGUI>();

    public InventoryItem item;

    public void UpdateSlotUI (InventoryItem newitem)
    {
        item = newitem;

        itemIcon.color = Color.white;

        if (item != null)
        {
            itemIcon.sprite = item.itemData.icon;

            if (item.stackSize > 1)
                itemUI.text = item.stackSize.ToString();
            else if (item.stackSize == 1)
                itemUI.text = "";
        }
    }

    public virtual void CleanUpSlot ()
    {
        item = null;
        itemIcon.color = Color.clear;
        itemIcon.sprite = null;
        itemUI.text = "";
    }

    public virtual void OnPointerDown (PointerEventData eventData)
    {
        if(item != null)
        {
            // ��ס��CTRL + ���ɾ����Ʒ
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Inventory.instance.RemoveItem(item.itemData);
                return;
            }

            if (item.itemData.itemType == itemType.Equipment)
                Inventory.instance.EquipItem(item.itemData as ItemData_Equipment);
        }
    }

    public virtual void OnPointerEnter (PointerEventData eventData)
    {
        if (item != null && item.itemData != null)
        {
            mainUI.itemToolTip.ShowToolTip(item.itemData as ItemData_Equipment);
            mainUI.itemToolTip.transform.position = SetToolTipPosition(eventData);
        }
    }

    public virtual void OnPointerExit (PointerEventData eventData)
    {
        if (item != null && item.itemData != null)
            mainUI.itemToolTip.HideToolTip(item.itemData as ItemData_Equipment);
    }

    /// <summary>
    /// ����Slotλ�� ����ToolTipλ��
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public Vector2 SetToolTipPosition (PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        float xOffset = 0;

        if (mousePos.x > Screen.width / 2) xOffset = -Screen.width / 6;
        else xOffset = Screen.width / 6;

        Vector2 newPos = new(mousePos.x + xOffset, mousePos.y + 200);
        return newPos;
    }

}
