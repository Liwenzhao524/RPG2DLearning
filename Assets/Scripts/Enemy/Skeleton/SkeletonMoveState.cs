using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : EnemyMoveState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX("SkeletonWalk", enemyBase.transform);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX("SkeletonWalk");
    }

    public override void Update()
    {
        base.Update();
    }
}
