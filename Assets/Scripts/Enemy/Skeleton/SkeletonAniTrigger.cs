using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAniTrigger : EnemyAniTrigger
{
    Enemy_Skeleton _enemy;
    
    protected override void Start()
    {
        base.Start();
        _enemy = enemy as Enemy_Skeleton;
    }

    protected override void AniTrigger()
    {
        base.AniTrigger();
    }

    protected override void AttackTrigger()
    {
        base.AttackTrigger();
    }

    private void OpenCounterAttackWindow() => _enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => _enemy.CloseCounterAttackWindow();
}
