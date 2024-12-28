using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundState
{
    public SlimeIdleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Slime;
    }

    public override void Enter ()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        stateTimer = enemy.idleTime;
    }

    public override void Exit ()
    {
        base.Exit();
        AudioManager.instance.PlaySFX("SlimeIdle", enemy.transform);
    }

    public override void Update ()
    {
        base.Update();
        if (stateTimer < 0f)
            enemyStateMachine.ChangeState(enemy.moveState);
    }
}
