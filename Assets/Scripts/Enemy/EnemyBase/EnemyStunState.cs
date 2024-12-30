using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunState : EnemyState
{
    public EnemyStunState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void AnimFinishTrigger ()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter ()
    {
        base.Enter();

        stateTimer = enemyBase.stunDuration;
        rb.velocity = new Vector2(enemyBase.stunDirection.x * -enemyBase.faceDir, enemyBase.stunDirection.y);
        enemyBase.fx.InvokeRepeating(nameof(enemyBase.fx.RedBlink), 0, 0.1f);
    }

    public override void Exit ()
    {
        base.Exit();
        enemyBase.fx.Invoke(nameof(enemyBase.fx.CancelColorChange), 0);
    }

    public override void Update ()
    {
        base.Update();
        if (stateTimer < 0)
            enemyBase.stateMachine.ChangeState(enemyBase.idleState);
    }
}
