using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> start = new();

    // װ�����
    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new();

    // ���Ͽ��
    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new();

    // װ����
    public List<InventoryItem> equipment = new();
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary = new();

    [Header("Slot UI")]
    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlotParent;
    [SerializeField] Transform equipmentSlotParent;

    UI_ItemSlot[] inventorySlots;
    UI_ItemSlot[] stashSlots;
    UI_EquipmentSlot[] equipmentSlots;


    float _lastTimeUseFlask;
    float _lastTimeUseArmor;
    float _flaskCoolDown;
    float _armorCoolDown;

    private void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    private void Start ()
    {
        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        foreach (var item in start)
        {
            while (item.stackSize > 0)
            {
                AddItem(item.itemData);
                item.RemoveStack();
            }
        }
    }

    void UpdateAllSlotUI ()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashSlots.Length; i++)
        {
            stashSlots[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventorySlots[i].UpdateSlotUI(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            stashSlots[i].UpdateSlotUI(stash[i]);
        }
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> items in equipmentDictionary)
            {
                if (items.Key.equipmentType == equipmentSlots[i].equipmentType)
                    equipmentSlots[i].UpdateSlotUI(items.Value);
            }
        }
    }

    /// <summary>
    /// ����װ������ ������װ���ĸ���װ��
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ItemData_Equipment GetEquipmentByType (EquipmentType type)
    {
        ItemData_Equipment newEquip = null;
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> items in equipmentDictionary)
        {
            if (items.Key.equipmentType == type)
                newEquip = items.Key;
        }
        return newEquip;
    }

    /// <summary>
    /// װ�� ��inventory��equipment
    /// </summary>
    /// <param name="item"></param>
    public void EquipItem (ItemData_Equipment item)
    {
        InventoryItem newEquipment = new(item);

        // ���װ�������޺�itemһ��λ�õ�װ�� ��ж��
        UnequipItem(item);

        item.AddModifiers();

        equipment.Add(newEquipment);
        equipmentDictionary.Add(item, newEquipment);
        RemoveItem(item);

        UpdateAllSlotUI();
    }

    /// <summary>
    /// ����� ж����װ�� ��equipment��inventory
    /// </summary>
    /// <param name="item"></param>
    public void UnequipItem (ItemData_Equipment item)
    {
        ItemData_Equipment equipmentToRemove = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> items in equipmentDictionary)
        {
            if (items.Key.equipmentType == item.equipmentType)
                equipmentToRemove = items.Key;
        }

        if (equipmentToRemove != null)
        {
            AddItem(equipmentToRemove);

            equipmentToRemove.RemoveModifiers();

            // ��װ�����Ƴ�
            equipment.Remove(equipmentDictionary[equipmentToRemove]);
            equipmentDictionary.Remove(equipmentToRemove);
        }
    }

    #region Basic Add Remove

    /// <summary>
    /// ������Ʒ���� �浽��Ӧλ��
    /// </summary>
    /// <param name="item"></param>
    public void AddItem (ItemData item)
    {
        if (item.itemType == itemType.Equipment)
        {
            AddToInventory(item);

        }
        else if (item.itemType == itemType.Material)
        {
            AddToStash(item);
        }
        UpdateAllSlotUI();
    }

    /// <summary>
    /// �������п�� �Ƴ���Ʒ
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem (ItemData item)
    {
        if (inventoryDictionary.ContainsKey(item))
        {
            inventoryDictionary[item].RemoveStack();
            if (inventoryDictionary[item].stackSize <= 0)
            {
                inventory.Remove(inventoryDictionary[item]);
                inventoryDictionary.Remove(item);
            }
        }
        else if (stashDictionary.ContainsKey(item))
        {

            stashDictionary[item].RemoveStack();
            if (stashDictionary[item].stackSize <= 0)
            {

                stash.Remove(stashDictionary[item]);
                stashDictionary.Remove(item);
            }
        }

        UpdateAllSlotUI();
    }

    /// <summary>
    /// ����Ʒ �浽inventory
    /// </summary>
    /// <param name="item"></param>
    private void AddToInventory (ItemData item)
    {
        if (inventoryDictionary.ContainsKey(item))
        {
            inventoryDictionary[item].AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            inventoryDictionary.Add(item, newItem);
        }
    }

    /// <summary>
    /// ����Ʒ �浽stash
    /// </summary>
    /// <param name="item"></param>
    private void AddToStash (ItemData item)
    {
        if (stashDictionary.ContainsKey(item))
        {
            stashDictionary[item].AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stash.Add(newItem);
            stashDictionary.Add(item, newItem);
        }
    }

    #endregion

    /// <summary>
    /// ������� ������װ��
    /// </summary>
    /// <param name="equipToCraft">����װ��</param>
    /// <param name="requireMaterial">�����б�</param>
    /// <returns>�Ƿ�����ɹ�</returns>
    public bool CanCraft (ItemData_Equipment equipToCraft, List<InventoryItem> requireMaterial)
    {
        // �ݴ��õ��Ĳ���
        List<InventoryItem> useMaterials = new();

        for (int i = 0; i < requireMaterial.Count; i++)
        {
            // ������޲���
            if (stashDictionary.ContainsKey(requireMaterial[i].itemData))
            {
                // �������
                InventoryItem value = stashDictionary[requireMaterial[i].itemData];
                if (value.stackSize >= requireMaterial[i].stackSize)
                {
                    useMaterials.Add(value);
                }
                else
                {
                    Debug.Log("No Enough Amount");
                    return false;
                }
            }
            else
            {
                Debug.Log("No Enough Type");
                return false;
            }
        }

        // ����ɹ� �۳�����
        for (int i = 0; i < useMaterials.Count; i++)
        {
            while (useMaterials[i].stackSize > 0)
            {
                RemoveItem(useMaterials[i].itemData);
                useMaterials[i].RemoveStack();
            }
        }

        AddItem(equipToCraft);

        Debug.Log("Craft" + equipToCraft.itemName);
        return true;
    }

    #region EffectEquip Use

    /// <summary>
    /// �����ȴ ������ʹ��ҩƿ����
    /// </summary>
    public void UseFlask ()
    {
        ItemData_Equipment flask;
        if (!GetEquipmentByType(EquipmentType.Flask)) return;
            
        flask = GetEquipmentByType(EquipmentType.Flask);

        if (Time.time > _lastTimeUseFlask + _flaskCoolDown)
        {
            _flaskCoolDown = flask.effectCoolDown;
            flask.ExecuteEffects(null);
            _lastTimeUseFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask In CoolDown");
        }
    }

    /// <summary>
    /// �����׼�����ȴ ��Flaskִ���߼���ͬ
    /// </summary>
    /// <returns></returns>
    public bool CanUseArmor ()
    {
        ItemData_Equipment armor;
        if (!GetEquipmentByType(EquipmentType.Armor)) return false;

        armor = GetEquipmentByType(EquipmentType.Armor);

        if(Time.time > _lastTimeUseArmor + _armorCoolDown)
        {
            _armorCoolDown = armor.effectCoolDown;
            //armor.ExecuteEffects(PlayerManager.instance.player.transform);
            _lastTimeUseArmor = Time.time;
            return true;
        }
        else
        {
            Debug.Log("Armor In CoolDown");
        }
        return false;
    }

    #endregion
}
