using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void AnimFinishTrigger ()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter ()
    {
        base.Enter();

        // 进入死前动画 并保持不动
        enemyBase.anim.SetBool(enemyBase.lastAniName, true);
        enemyBase.anim.speed = 0;
        enemyBase.col.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
