using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : EnemyIdleState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX("SkeletonIdle", enemyBase.transform);
    }

    public override void Update()
    {
        base.Update();
    }
}
    

