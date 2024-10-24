using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "new item", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public List<ItemEffect> equipEffects;
    public float effectCoolDown;

    [Header("Major Stats")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Defence Stats")]
    public float maxHP;
    public float armor;
    public float evasion;
    public float magicResistence;

    [Header("Offense Stats")]
    public float ATK;
    public float critChance;
    public float critDamage;

    [Header("Magic Stats")]
    public float fireATK;
    public float iceATK;
    public float lightningATK; 

    [Header("Craft")]
    public List<InventoryItem> craftMaterials;

    /// <summary>
    /// ִ��װ���ϵ�����Ч�� ����
    /// </summary>
    /// <param name="target">������������õĶ���</param>
    public void ExecuteEffects (Transform target)
    {
        foreach (var effect in equipEffects)
        {
            effect.ExecuteEffect(target);
        }
    }

    /// <summary>
    /// ���ɫ��帽���������
    /// </summary>
    public void AddModifiers ()
    {
        // ��ȡPlayerStats���  
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier (strength);
        playerStats.agility.AddModifier (agility);
        playerStats.intelligence.AddModifier (intelligence);
        playerStats.vitality.AddModifier (vitality);

        playerStats.maxHP.AddModifier (maxHP);
        playerStats.armor.AddModifier (armor);
        playerStats.evasion.AddModifier (evasion);
        playerStats.magicResistence.AddModifier (magicResistence);

        playerStats.ATK.AddModifier(ATK);
        playerStats.critChance.AddModifier (critChance);
        playerStats.critDamage.AddModifier (critDamage);

        playerStats.fireATK.AddModifier (fireATK);
        playerStats.iceATK.AddModifier (iceATK);
        playerStats.lightningATK.AddModifier (lightningATK);

    }

    /// <summary>
    /// �ӽ�ɫ����Ƴ��������
    /// </summary>
    public void RemoveModifiers ()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        
        playerStats.strength.RemoveModifier (strength);
        playerStats.agility.RemoveModifier (agility);
        playerStats.intelligence.RemoveModifier (intelligence);
        playerStats.vitality.RemoveModifier (vitality);

        playerStats.maxHP.RemoveModifier (maxHP);
        playerStats.armor.RemoveModifier (armor);
        playerStats.magicResistence.RemoveModifier(magicResistence);
        playerStats.evasion.RemoveModifier (evasion);

        playerStats.ATK.RemoveModifier(ATK);
        playerStats.critChance.RemoveModifier (critChance);
        playerStats.critDamage.RemoveModifier (critDamage);

        playerStats.fireATK.RemoveModifier (fireATK);
        playerStats.iceATK.RemoveModifier(iceATK);
        playerStats.lightningATK.RemoveModifier(lightningATK);

    }
}
