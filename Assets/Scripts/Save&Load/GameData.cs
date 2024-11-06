using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int money;

    public DictionarySerialized<string, bool> saveSkillTree;
    public DictionarySerialized<string, int> saveInventory;
    public List<string> saveEquipment;

    public GameData ()
    {
        money = 0;
        saveSkillTree = new();
        saveInventory = new();
        saveEquipment = new List<string>(); 
    }
}
