using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Transform _player;
    Enemy_Skeleton _enemy;
    int _moveDir;
    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
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

        if (_player.position.x > rb.position.x)
            _moveDir = 1;
        else if(_player.position.x < rb.position.x)
            _moveDir = -1;

        _enemy.SetVelocity(_enemy.moveSpeed * _moveDir, rb.velocity.y);
        
        if (_enemy.IsPlayerDetected())
        {
            stateTimer = _enemy.battleTime;
            if(_enemy.IsPlayerDetected().distance < _enemy.attackDistance)
            {
                _enemy.SetZeroVelocity();
                if(_enemy.CanAttack())
                    _enemy.stateMachine.ChangeState(_enemy.attackState);
            } 
        }
        else
        {
            if(stateTimer < 0 || Vector2.Distance(_player.position, _enemy.transform.position) > _enemy.attackDistance * 3f)
                _enemy.stateMachine.ChangeState(_enemy.idleState);
        }
    }

    
}
