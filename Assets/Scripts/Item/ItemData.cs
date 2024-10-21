using UnityEngine;

/// <summary>
/// 道具具体Data
/// </summary>
[CreateAssetMenu(fileName = "new item", menuName = "Data/item")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite icon;
}
