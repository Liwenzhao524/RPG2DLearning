using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    // inventory存Equipemnt stash存Material
    public static Inventory instance;

    public List<InventoryItem> inventory = new();
    public Dictionary<ItemData, InventoryItem> inventoryDictionary = new();

    public List<InventoryItem> stash = new();
    public Dictionary<ItemData, InventoryItem> stashDictionary = new();

    public List<InventoryItem> equipment = new();
    public Dictionary<ItemData_Equipment, InventoryItem > equipmentDictionary = new();

    [Header("Slot UI")]
    [SerializeField] Transform inventorySlotParent;
    [SerializeField] Transform stashSlotParent;
    [SerializeField] Transform equipmentSlotParent;
    //[SerializeField] GameObject itemSlotPrefab;
    UI_ItemSlot[] inventorySlots;
    UI_ItemSlot[] stashSlots;
    UI_EquipmentSlot[] equipmentSlots;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start ()
    {
        inventorySlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>(); 
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
        for(int i = 0; i < stash.Count; i++)
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

    public void EquipItem(ItemData_Equipment item)
    {
        InventoryItem newEquipment = new(item);

        // 检查装备槽有无和item一个位置的装备 并卸下
        UnequipItem(item);

        item.AddModifiers();

        equipment.Add(newEquipment);
        equipmentDictionary.Add(item, newEquipment);
        RemoveItem(item);

        UpdateAllSlotUI();
    }

    /// <summary>
    /// 如果有 卸除旧装备
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
            equipment.Remove(equipmentDictionary[equipmentToRemove]);
            equipmentDictionary.Remove(equipmentToRemove);
        }
    }

    public void AddItem(ItemData item)
    {
        if(item.itemType == itemType.Equipment)
        {
            AddToInventory(item);

        }
        else if(item.itemType == itemType.Material)
        {
             AddToStash(item);
        }
        UpdateAllSlotUI();
    }

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

    public void RemoveItem(ItemData item)
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
}
