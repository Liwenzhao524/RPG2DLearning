using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyStunState
{
    public SlimeStunState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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
    }

    public override void Update ()
    {
        base.Update();

        if(rb.velocity.y < 0.1f && enemyBase.IsGroundDetected())
        {
            enemyBase.anim.SetTrigger("StunTrigger");
            enemyBase.fx.Invoke(nameof(enemyBase.fx.CancelColorChange), 0);
        }
    }
}
