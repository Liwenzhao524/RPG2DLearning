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

    public DictionarySerialized<string, bool> checkpoints;
    public string closestCheckpointID;

    public float bgmValue;
    public float sfxValue;

    public GameData ()
    {
        money = 0;
        saveSkillTree = new();
        saveInventory = new();
        saveEquipment = new(); 

        closestCheckpointID = string.Empty;
        checkpoints = new();

        bgmValue = 1; 
        sfxValue = 1;
    }
}
