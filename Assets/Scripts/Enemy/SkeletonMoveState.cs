using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        _enemy.SetVelocity(_enemy.moveSpeed * _enemy.faceDir, _rb.velocity.y);

        if(!_enemy.IsGroundDetected() || _enemy.IsWallDetected())
        {
            _enemy.FlipController(-_enemy.faceDir);
            _enemyStateMachine.ChangeState(_enemy.idleState);
        }
    }
}
