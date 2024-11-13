using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂在animator物体上，只负责动画事件触发
/// </summary>
public class PlayerAnimTriggers : MonoBehaviour
{
    Player _player => PlayerManager.instance.player;

    private void AnimTrigger()
    {
        _player.AnimTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX("PlayerPrimeAttack");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackRadius);
        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>(); 
                _player.GetComponent<CharacterStats>().DoDamageTo(target);
                //_player.GetComponent<CharacterStats>().DoMagicDamageTo(target);

                Inventory.instance.GetEquipmentByType(EquipmentType.Weapon)?.ExecuteEffects(hit.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
