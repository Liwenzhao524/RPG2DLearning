using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleportState : EnemyState
{
    Enemy_Boss _enemy;
    public BossTeleportState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();

        if (anitrigger)
        {
            if (_enemy.CanCast())
                _enemy.stateMachine.ChangeState(_enemy.castState);
            else
                _enemy.stateMachine.ChangeState(_enemy.battleState);
        }
    }
}
