using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FreezeNear Effect", menuName = "Data/Item Effect/FreezeNear")]
public class Effect_FreezeNearby : ItemEffect
{
    [SerializeField] float duration;
    public override void ExecuteEffect (Transform target)
    {
        base.ExecuteEffect(target);

        // 首先条件1：血线 其次条件2：冷却
        // 仅当受击时判断是否触发 不会凭空触发
        if (stats.currentHP > stats.GetMaxHP() * 0.4f || !Inventory.instance.CanUseArmor())
            return;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(target.position, 2);

        foreach(var collision in collisions)
        {
            if(collision.TryGetComponent(out Enemy enemy))
            {
                enemy.StartCoroutine(enemy.FreezeTimerFor(duration));
            }
        }
    }
}
