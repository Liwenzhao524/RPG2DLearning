using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
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
        enemyBase.lastAttackTime = Time.time;
    }

    public override void Update ()
    {
        base.Update();
        enemyBase.SetZeroVelocity();
        if (anitrigger)
            enemyBase.stateMachine.ChangeState(enemyBase.battleState);
    }
}
