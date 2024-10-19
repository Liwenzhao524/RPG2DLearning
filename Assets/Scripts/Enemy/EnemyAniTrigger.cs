using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人 动画事件 基类
/// </summary>
public class EnemyAniTrigger : MonoBehaviour
{
    protected Enemy _enemy;

    protected virtual void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    protected virtual void AniTrigger()
    {
        _enemy.AnimTrigger();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.attackCheck.position, _enemy.attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                _enemy.GetComponent<CharacterStats>().DoDamageTo(target);
                _enemy.GetComponent<CharacterStats>().DoMagicDamageTo(target);
            }
        }
    }
}
