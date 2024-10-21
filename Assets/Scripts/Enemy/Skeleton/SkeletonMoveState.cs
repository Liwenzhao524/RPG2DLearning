using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.faceDir, rb.velocity.y);

        if(!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.FlipController(-enemy.faceDir);
            enemyStateMachine.ChangeState(enemy.idleState);
        }
    }
}
