using System.Diagnostics;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{

    public void SetUpCraftSlot (ItemData_Equipment equip)
    {
        if (equip == null) return;

        item.itemData = equip;
        itemIcon.sprite = equip.icon;
        itemUI.text = equip.name;
    }

    public override void OnPointerDown (PointerEventData eventData)
    {
        UI_CraftWindow window = mainUI.craftWindow;

        window?.SetUpCraftWindow(item.itemData as ItemData_Equipment);
    }
}

