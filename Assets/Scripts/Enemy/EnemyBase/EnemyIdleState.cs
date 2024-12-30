using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyGroundState
{
    public EnemyIdleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        enemyBase.SetZeroVelocity();
        stateTimer = enemyBase.idleTime;
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();
        if (stateTimer < 0f)
            enemyStateMachine.ChangeState(enemyBase.moveState);
    }
}
