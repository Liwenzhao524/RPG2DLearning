using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemType;

    public override void ShowToolTip(ItemData_Equipment item)
    {
        itemName.text = item.itemName;
        itemType.text = item.equipmentType.ToString();
        description.text = item.GetDescription();
        
        base.ShowToolTip(item);
    }

    public override void HideToolTip(ItemData_Equipment item)
    {
        item.stringBuilder.Clear();
        base.HideToolTip(item);
    }
}
