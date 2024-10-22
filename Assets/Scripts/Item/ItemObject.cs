using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µÀ¾ßGameObject
/// </summary>
public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    //Inventory _inventory = Inventory.instance;

    private void OnValidate ()
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
