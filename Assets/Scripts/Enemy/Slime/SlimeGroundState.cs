using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeGroundState : EnemyState
{
    protected Enemy_Slime enemy;
    protected Transform player;
    public SlimeGroundState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Slime;
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
        if (enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < enemy.attackDistance)
            enemy.stateMachine.ChangeState(enemy.battleState);
    }
}
