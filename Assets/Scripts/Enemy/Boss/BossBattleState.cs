using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleState : EnemyBattleState
{
    public BossBattleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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
        if (player.position.x > rb.position.x)
            _moveDir = 1;
        else if (player.position.x < rb.position.x)
            _moveDir = -1;

        enemyBase.SetVelocity(enemyBase.moveSpeed * _moveDir, rb.velocity.y);

        if (enemyBase.IsPlayerDetected())
        {
            if (enemyBase.IsPlayerDetected().distance < enemyBase.attackDistance)
            {
                enemyBase.SetZeroVelocity();
                if (enemyBase.CanAttack())
                    enemyBase.stateMachine.ChangeState(enemyBase.attackState);
                else
                    enemyBase.stateMachine.ChangeState(enemyBase.idleState);
            }
        }
        
    }
}
