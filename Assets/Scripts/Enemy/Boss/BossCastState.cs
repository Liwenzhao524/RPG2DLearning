using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastState : EnemyState
{
    Enemy_Boss _enemy;
    public BossCastState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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
        stateTimer = 5;
    }

    public override void Exit ()
    {
        base.Exit();
        _enemy.lastCastTime = Time.time;
    }

    public override void Update ()
    {
        base.Update();

        if(stateTimer < 0)
            _enemy.stateMachine.ChangeState(_enemy.idleState);
    }
}
