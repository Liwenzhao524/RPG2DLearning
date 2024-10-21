using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;
    public SkeletonGroundState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        enemy = enemyBase as Enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.IsPlayerDetected() || Vector2.Distance(player.position, enemy.transform.position) < enemy.attackDistance)
            enemy.stateMachine.ChangeState(enemy.battleState);
    }
}
