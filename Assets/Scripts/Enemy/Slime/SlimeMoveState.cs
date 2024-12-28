using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : SlimeGroundState
{
    public SlimeMoveState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Slime;
    }

    public override void Enter ()
    {
        base.Enter();
        AudioManager.instance.PlaySFX("SlimeWalk", enemy.transform);
    }

    public override void Exit ()
    {
        base.Exit();
        AudioManager.instance.StopSFX("SlimeWalk");
    }

    public override void Update ()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.faceDir, rb.velocity.y);

        if (!enemy.IsGroundDetected() || enemy.IsWallDetected())
        {
            enemy.FlipController(-enemy.faceDir);
            enemyStateMachine.ChangeState(enemy.idleState);
        }
    }
}
