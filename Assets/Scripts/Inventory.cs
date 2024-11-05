using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> start = new();

    // 装备库存
    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new();

    // 材料库存
    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new();

    // 装备槽
    public List<InventoryItem> equipment = new();
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary = new();

    [Header("Slot UI")]
    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlotParent;
    [SerializeField] Transform equipmentSlotParent;
    [SerializeField] Transform statsSlotParent;

    UI_ItemSlot[] _inventorySlots;
    UI_ItemSlot[] _stashSlots;
    UI_EquipmentSlot[] _equipmentSlots;
    UI_StatSlot[] _statSlots;

    UI _mainUI;

    float _lastTimeUseFlask;
    float _lastTimeUseArmor;
    public float flaskCoolDown { get; private set; }
    public bool flaskUse { get; set; }
    float _armorCoolDown;

    private void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    private void Start ()
    {
        _inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        _equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        _statSlots = statsSlotParent.GetComponentsInChildren<UI_StatSlot>();

        _mainUI = inventorySlotParent.GetComponentInParent<UI>();

        foreach (var item in start)
        {
            while (item.stackSize > 0)
            {
                AddItem(item.itemData);
                item.RemoveStack();
            }
        }

        UpdateAllSlotUI();
    }

    public void UpdateAllSlotUI ()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            _inventorySlots[i].CleanUpSlot();
        }
        for (int i = 0; i < _stashSlots.Length; i++)
        {
            _stashSlots[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            _inventorySlots[i].UpdateSlotUI(inventory[i]);
        }
        for (int i = 0; i < stash.Count; i++)
        {
            _stashSlots[i].UpdateSlotUI(stash[i]);
        }
        for (int i = 0; i < _equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> items in equipmentDictionary)
            {
                if (items.Key.equipmentType == _equipmentSlots[i].equipmentType)
                    _equipmentSlots[i].UpdateSlotUI(items.Value);
            }
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI ()
    {
        for (int i = 0; i < _statSlots.Length; i++)
        {
            _statSlots[i].UpdateStatUI();
        }
    }

    /// <summary>
    /// 根据装备类型 返回已装备的该类装备
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
    /// 装备 从inventory到equipment
    /// </summary>
    /// <param name="item"></param>
    public void EquipItem (ItemData_Equipment item)
    {
        InventoryItem newEquipment = new(item);

        // 检查装备槽有无和item一个位置的装备 并卸下
        UnequipItem(item);

        item.AddModifiers();

        equipment.Add(newEquipment);
        equipmentDictionary.Add(item, newEquipment);
        RemoveItem(item);

        UpdateAllSlotUI();
        _mainUI.itemToolTip.HideToolTip(item);
    }

    /// <summary>
    /// 如果有 卸除旧装备 从equipment到inventory
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

            // 从已装备槽移除
            equipment.Remove(equipmentDictionary[equipmentToRemove]);
            equipmentDictionary.Remove(equipmentToRemove);

            UpdateAllSlotUI();
            _mainUI.itemToolTip.HideToolTip(equipmentToRemove);
        }
    }

    #region Basic Add Remove

    /// <summary>
    /// 根据物品类型 存到对应位置
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
    /// 查找所有库存 移除物品
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

    public bool CanAddToInventory ()
    {
        return inventory.Count < _inventorySlots.Length;
    }

    /// <summary>
    /// 新物品 存到inventory
    /// </summary>
    /// <param name="item"></param>
    private void AddToInventory (ItemData item)
    {
        if (!CanAddToInventory())
        {
            Debug.Log("No Space");
            return;
        }

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
    /// 新物品 存到stash
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
    /// 如果可以 则制造装备
    /// </summary>
    /// <param name="equipToCraft">待造装备</param>
    /// <param name="requireMaterial">需求列表</param>
    /// <returns>是否制造成功</returns>
    public bool CanCraft (ItemData_Equipment equipToCraft, List<InventoryItem> requireMaterial)
    {
        // 暂存用到的材料
        List<InventoryItem> useMaterials = new();

        for (int i = 0; i < requireMaterial.Count; i++)
        {
            // 检查有无材料
            if (stashDictionary.ContainsKey(requireMaterial[i].itemData))
            {
                // 检查数量
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

        // 制造成功 扣除材料
        for (int i = 0; i < useMaterials.Count; i++)
        {
            while (useMaterials[i].stackSize > 0)
            {
                RemoveItem(useMaterials[i].itemData);
                useMaterials[i].RemoveStack();
            }
        }

        AddItem(equipToCraft);

        Debug.Log("Craft " + equipToCraft.itemName);
        return true;
    }

    #region EffectEquip Use

    /// <summary>
    /// 检查冷却 并尝试使用药瓶技能
    /// </summary>
    public void UseFlask ()
    {
        ItemData_Equipment flask;
        if (!GetEquipmentByType(EquipmentType.Flask)) return;
            
        flask = GetEquipmentByType(EquipmentType.Flask);

        if (Time.time > _lastTimeUseFlask + flaskCoolDown)
        {
            flaskUse = true;
            flaskCoolDown = flask.effectCoolDown;
            flask.ExecuteEffects(null);
            _lastTimeUseFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask In CoolDown");
        }
    }

    /// <summary>
    /// 检查盔甲技能冷却 与Flask执行逻辑不同
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
