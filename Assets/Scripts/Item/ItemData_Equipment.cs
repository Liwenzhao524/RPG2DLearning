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

    [Header("Unique Effect")]
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
    [Range(0f, 1f)]
    public float critChance;
    public float critDamage;

    [Header("Magic Stats")]
    public float fireATK;
    public float iceATK;
    public float lightningATK; 

    [Header("Craft")]
    [Tooltip("������4��")]
    public List<InventoryItem> craftMaterials;

    int descriptionLineCount;

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
        playerStats.magicResist.AddModifier (magicResistence);

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
        playerStats.magicResist.RemoveModifier(magicResistence);
        playerStats.evasion.RemoveModifier (evasion);

        playerStats.ATK.RemoveModifier(ATK);
        playerStats.critChance.RemoveModifier (critChance);
        playerStats.critDamage.RemoveModifier (critDamage);

        playerStats.fireATK.RemoveModifier (fireATK);
        playerStats.iceATK.RemoveModifier(iceATK);
        playerStats.lightningATK.RemoveModifier(lightningATK);

    }

    /// <summary>
    /// ��ToolTip�������
    /// </summary>
    /// <returns></returns>
    public override string GetDescription ()
    {

        AddDescription(strength, "Strength");
        AddDescription(agility, "Agility");
        AddDescription(intelligence, "Intelligence");
        AddDescription(vitality, "Vitality");
        AddDescription(maxHP, "MaxHP");
        AddDescription(armor, "Armor");
        AddDescription(evasion, "Evasion");
        AddDescription(magicResistence, "Magic Resist");
        AddDescription(ATK, "ATK");
        AddDescription(critChance, "Crit Chance");
        AddDescription(critDamage, "Crit Damage");
        AddDescription(fireATK, "Fire ATK");
        AddDescription(iceATK, "Ice ATK");
        AddDescription(lightningATK, "Lightning ATK");

        stringBuilder.AppendLine();

        for(int i = 0; i < equipEffects.Count; i ++)
        {
            if (equipEffects[i].effectDescription.Length > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("Unique Effect: " + equipEffects[i].effectDescription);
                descriptionLineCount++; 
            }
        }

        if(descriptionLineCount < 4)
        {
            for (int i = 0; i < 4 - descriptionLineCount; i++)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(" ");
            }
        }

        string str = stringBuilder.ToString();
        stringBuilder.Clear();

        return str;
    }

    /// <summary>
    /// �Բ�Ϊ0�������������
    /// </summary>
    /// <param name="value"></param>
    /// <param name="name"></param>
    void AddDescription(float value, string name)
    {
        if(value != 0)
        {
            if(stringBuilder.Length > 0)
                stringBuilder.AppendLine();

            if(value > 0)
            {
                if(name == "Crit Chance")
                    stringBuilder.Append( "+ " + value * 100 + "% " + name);
                else if(name == "Crit Damage")
                    stringBuilder.Append( "+ " + value * 100 + "% " + name);
                else
                    stringBuilder.Append( "+ " + value + " " + name);

                descriptionLineCount++;
            }
        }
    }
}
