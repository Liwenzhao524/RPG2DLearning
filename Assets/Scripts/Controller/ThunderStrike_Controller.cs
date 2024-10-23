using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrike_Controller : Skill_Controller
{
    protected virtual void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            PlayerStats stats = player.stats as PlayerStats;
            EnemyStats target = collision.GetComponent<EnemyStats>();

            stats.DoMagicDamageTo(target);

            Destroy(gameObject, 0.5f);
        }
    }
}
