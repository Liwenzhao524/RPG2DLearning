using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    protected Transform player;
    protected int _moveDir;
    public EnemyBattleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        if (player.GetComponent<PlayerStats>().isDead)
            enemyBase.stateMachine.ChangeState(enemyBase.moveState);
    }

    public override void Update ()
    {
        base.Update();
        if (player.position.x > rb.position.x)
            _moveDir = 1;
        else if (player.position.x < rb.position.x)
            _moveDir = -1;

        enemyBase.SetVelocity(enemyBase.moveSpeed * _moveDir, rb.velocity.y);

        if (enemyBase.IsPlayerDetected())
        {
            stateTimer = enemyBase.battleTime;
            if (enemyBase.IsPlayerDetected().distance < enemyBase.attackDistance)
            {
                enemyBase.SetZeroVelocity();
                if (enemyBase.CanAttack())
                    enemyBase.stateMachine.ChangeState(enemyBase.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.position, enemyBase.transform.position) > enemyBase.attackDistance * 3f)
                enemyBase.stateMachine.ChangeState(enemyBase.idleState);
        }
    }

    public override void Exit ()
    {
        base.Exit();
    }
}
