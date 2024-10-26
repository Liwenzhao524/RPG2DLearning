using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 点击一类装备时 显示下属可制作装备的列表UI
/// </summary>
public class UI_CraftList : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Transform craftSlotParent;
    [SerializeField] GameObject craftSlotPrefab;

    [SerializeField] List<ItemData_Equipment> craftEquipList;

    [SerializeField] UI_CraftWindow craftWindow;

    void Start ()
    {
        
    }

    /// <summary>
    /// 显示能制作的装备列表
    /// </summary>
    public void SetUpCraftList ()
    {
        CleanUpLastList();

        for (int i = 0; i < craftEquipList.Count; i++)
        {
            GameObject newEquip = Instantiate(craftSlotPrefab, craftSlotParent);
            newEquip.GetComponent<UI_CraftSlot>().SetUpCraftSlot(craftEquipList[i]);
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        SetUpCraftList();
    }

    /// <summary>
    /// 删除上一次残留
    /// </summary>
    private void CleanUpLastList ()
    {
        craftWindow.gameObject.SetActive(false);
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }
    }
}
