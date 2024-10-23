using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µÀ¾ßGameObject
/// </summary>
public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    Rigidbody2D _rb => GetComponent<Rigidbody2D>();

    Vector2 _dropVelocity;

    public void SetUpItem(ItemData itemData, Vector2 dropVelocity)
    {
        _dropVelocity = dropVelocity;
        this.itemData = itemData;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "item - " + itemData.itemName;
        _rb.velocity = _dropVelocity;
    }

    public void PickupItem ()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
