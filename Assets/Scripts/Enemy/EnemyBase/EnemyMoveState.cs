using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyGroundState
{
    public EnemyMoveState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();

        enemyBase.SetVelocity(enemyBase.moveSpeed * enemyBase.faceDir, rb.velocity.y);

        if (!enemyBase.IsGroundDetected() || enemyBase.IsWallDetected())
        {
            enemyBase.FlipController(-enemyBase.faceDir);
            enemyStateMachine.ChangeState(enemyBase.idleState);
        }
    }
}
