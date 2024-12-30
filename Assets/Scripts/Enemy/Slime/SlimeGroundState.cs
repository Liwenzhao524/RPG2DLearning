using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyGroundState
{
    public SlimeGroundState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
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
    }
}
