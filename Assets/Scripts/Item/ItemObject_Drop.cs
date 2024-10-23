 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������Ʒ����
/// </summary>
public class ItemObject_Drop : MonoBehaviour
{

    [SerializeField] GameObject dropPrefab;

    Vector2 _dropVelocity;

    public void DropItem(ItemData itemData)
    {
        GameObject newItem = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        _dropVelocity = new Vector2(Random.Range(-3, 3), Random.Range(10, 15));

        newItem.GetComponent<ItemObject>().SetUpItem(itemData, _dropVelocity);
    }
}
