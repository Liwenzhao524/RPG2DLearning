using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    Inventory _inventory;
    SpriteRenderer _sr;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Inventory.instance;
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = itemData.icon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            _inventory.AddItem(itemData);
        }
    }
}
