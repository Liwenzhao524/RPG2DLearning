using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人 动画事件 基类
/// </summary>
public class EnemyAniTrigger : MonoBehaviour
{
    protected Enemy enemy;

    protected virtual void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    protected virtual void AniTrigger()
    {
        enemy.AnimTrigger();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.GetComponent<CharacterStats>().DoDamageTo(target);
                //enemy.GetComponent<CharacterStats>().DoMagicDamageTo(target);
            }
        }
    }

    public void OpenCounterAttackWindow () => enemy.OpenCounterAttackWindow();
    public void CloseCounterAttackWindow () => enemy.CloseCounterAttackWindow();
}
