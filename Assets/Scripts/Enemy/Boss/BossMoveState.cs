using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : EnemyMoveState
{
    public BossMoveState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        AudioManager.instance.PlaySFX("BossWalk", enemyBase.transform);
    }

    public override void Exit ()
    {
        base.Exit();
        AudioManager.instance.StopSFX("BossWalk");
    }

    public override void Update ()
    {
        base.Update();
    }
}
