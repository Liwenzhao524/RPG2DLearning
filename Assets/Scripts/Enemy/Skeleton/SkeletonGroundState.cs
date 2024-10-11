using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected Enemy_Skeleton _enemy;
    protected Transform _player;
    public SkeletonGroundState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        _player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(_enemy.IsPlayerDetected() || Vector2.Distance(_player.position, _enemy.transform.position) < _enemy.attackDistance)
            _enemy.stateMachine.ChangeState(_enemy.battleState);
    }
}
