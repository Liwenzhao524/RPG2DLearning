using UnityEngine;

public enum itemType 
{
    Material,
    Equipment
}


/// <summary>
/// 道具具体信息类
/// </summary>
[CreateAssetMenu(fileName = "new item", menuName = "Data/item")]
public class ItemData : ScriptableObject
{
    public itemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropChance;
}
