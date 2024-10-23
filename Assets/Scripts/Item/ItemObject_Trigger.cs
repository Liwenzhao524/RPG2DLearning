using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    
    ItemObject _itemObject => GetComponentInParent<ItemObject>();
    Collider2D _colTrigger => GetComponent<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !collision.GetComponent<CharacterStats>().isDead)
        {
            _itemObject.PickupItem();
            //_colTrigger.enabled = false;
        }
    }
}
