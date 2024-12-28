using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum itemType 
{
    Material,
    Equipment
}


/// <summary>
/// 道具具体信息类
/// </summary>
[CreateAssetMenu(fileName = "new item", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public itemType itemType;
    public string itemName;
    public Sprite icon;
    public string itemID;

    [Range(0, 100)]
    public float dropChance;

    public StringBuilder stringBuilder = new();

    private void OnValidate ()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription ()
    {
        return "";
    }
}
