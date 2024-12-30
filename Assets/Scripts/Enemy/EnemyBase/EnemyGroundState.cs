using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundState : EnemyState
{
    protected Transform player;
    public EnemyGroundState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();
        if (enemyBase.IsPlayerDetected() || Vector2.Distance(player.position, enemyBase.transform.position) < enemyBase.attackDistance)
            enemyBase.stateMachine.ChangeState(enemyBase.battleState);
    }
}
