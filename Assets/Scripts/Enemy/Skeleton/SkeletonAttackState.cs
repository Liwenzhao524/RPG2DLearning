using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    Enemy_Skeleton _enemy;
    public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();
        _enemy.lastAttackTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        _enemy.SetZeroVelocity();
        if(anitrigger)
            _enemy.stateMachine.ChangeState(_enemy.battleState);

    }
}
