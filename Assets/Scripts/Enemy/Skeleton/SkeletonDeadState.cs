using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    Enemy_Skeleton _enemy;
    public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Skeleton;
    }

    public override void AnimFinishTrigger()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        // 进入死前动画 并保持不动
        _enemy.anim.SetBool(_enemy.lastAniName, true);
        _enemy.anim.speed = 0;
        _enemy.col.enabled = false;

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
