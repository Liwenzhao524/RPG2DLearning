using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX("SkeletonIdle", enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0f)
            enemyStateMachine.ChangeState(enemy.moveState);
    }
}
    

