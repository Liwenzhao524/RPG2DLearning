using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() || collision.GetComponent<Player>())
            collision.GetComponent<CharacterStats>().KillEntity();
        else
            Destroy(collision);
    }
}
