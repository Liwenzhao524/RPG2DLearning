using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAniTrigger : EnemyAniTrigger
{
    Enemy_Skeleton enemy; 
    
    protected override void Start()
    {
        base.Start();
        enemy = _enemy as Enemy_Skeleton;
    }

    protected override void AniTrigger()
    {
        base.AniTrigger();
    }

    protected override void AttackTrigger()
    {
        base.AttackTrigger();
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
