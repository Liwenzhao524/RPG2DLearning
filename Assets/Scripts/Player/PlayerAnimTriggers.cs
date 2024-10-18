using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在animator物体上，只负责动画事件触发
/// </summary>
public class PlayerAnimTriggers : MonoBehaviour
{
    Player _player => GetComponentInParent<Player>();

    private void AnimTrigger()
    {
        _player.AnimTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>(); 
                _player.GetComponent<CharacterStats>().DoDamageTo(target);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager._instance.sword.CreateSword();
    }
}
