using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastState : EnemyState
{
    Enemy_Boss _enemy;

    int castAmount;

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
        castAmount = _enemy.castAmount;

        stateTimer = 0.5f;
    }

    public override void Exit ()
    {
        base.Exit();
        _enemy.lastCastTime = Time.time;
    }

    public override void Update ()
    {
        base.Update();

        if (CanCast_Amount())
        {
            _enemy.CreateCast();
        }
        else if(castAmount <= 0)
            _enemy.stateMachine.ChangeState(_enemy.teleportState);
    }


    public bool CanCast_Amount ()
    {
        if (castAmount > 0 && stateTimer < 0)
        {
            stateTimer = _enemy.castInterval;
            castAmount--;
            return true;
        }

        return false;
    }
}
