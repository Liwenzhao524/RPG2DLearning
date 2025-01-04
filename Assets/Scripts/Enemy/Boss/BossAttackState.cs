using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : EnemyAttackState
{
    Enemy_Boss _enemy;
    public BossAttackState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Boss;
    }

    public override void AnimFinishTrigger ()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter ()
    {
        base.Enter();
        _enemy.teleportChance += 10;
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        _enemy.SetZeroVelocity();
        if (anitrigger)
        {
            if (_enemy.CanTeleport())
                _enemy.stateMachine.ChangeState(_enemy.teleportState);
            else
                _enemy.stateMachine.ChangeState(_enemy.battleState);
        }
    }
}
