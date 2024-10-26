using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���������װ��ʱ ��ʾ������������UI 
/// ����װ����Ϣ �������
/// </summary>
public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameUI;
    [SerializeField] TextMeshProUGUI descriptionUI;

    [SerializeField] Image[] materialImage;

    Button _craftButton => GetComponentInChildren<Button>();

    public void SetUpCraftWindow (ItemData_Equipment equip)
    {
        gameObject.SetActive(true);
        _craftButton.onClick.RemoveAllListeners();

        icon.sprite = equip.icon;
        nameUI.text = equip.itemName;
        descriptionUI.text = equip.GetDescription();

        SetMaterialInfo(equip);

        _craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(equip, equip.craftMaterials));
    }

    /// <summary>
    /// ����equip���������List ����window�ڲ���ͼ�꼰����
    /// </summary>
    /// <param name="equip"></param>
    private void SetMaterialInfo (ItemData_Equipment equip)
    {
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < equip.craftMaterials.Count; i++)
        {
            materialImage[i].color = Color.white;
            materialImage[i].sprite = equip.craftMaterials[i].itemData.icon;

            TextMeshProUGUI text = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.white;
            text.text = equip.craftMaterials[i].stackSize.ToString();
        }
    }
}
