using UnityEngine;

/// <summary>
/// ���߾���Data
/// </summary>
[CreateAssetMenu(fileName = "new item", menuName = "Data/item")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite icon;
}
