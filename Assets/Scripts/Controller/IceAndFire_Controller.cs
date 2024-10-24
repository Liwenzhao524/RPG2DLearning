using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFire_Controller : Skill_Controller
{
    protected override void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats stats = player.stats as PlayerStats;
            EnemyStats target = collision.GetComponent<EnemyStats>();

            stats.DoMagicDamageTo(target);
        }
    }
}
