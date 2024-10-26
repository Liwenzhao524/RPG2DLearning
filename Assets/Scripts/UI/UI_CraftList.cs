using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���һ��װ��ʱ ��ʾ����������װ�����б�UI
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
    /// ��ʾ��������װ���б�
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
    /// ɾ����һ�β���
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
