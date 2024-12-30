using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMoveState : EnemyMoveState
{
    public SlimeMoveState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        AudioManager.instance.PlaySFX("SlimeWalk", enemyBase.transform);
    }

    public override void Exit ()
    {
        base.Exit();
        AudioManager.instance.StopSFX("SlimeWalk");
    }

    public override void Update ()
    {
        base.Update();
    }
}
