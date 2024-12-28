using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBattleState : EnemyBattleState
{
    Enemy_Slime _enemy;
    public SlimeBattleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Slime;
    }

    public override void Enter ()
    {
        base.Enter();

        if (_player.GetComponent<PlayerStats>().isDead)
            _enemy.stateMachine.ChangeState(_enemy.moveState);
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            stateTimer = _enemy.battleTime;
            if (_enemy.IsPlayerDetected().distance < _enemy.attackDistance)
            {
                _enemy.SetZeroVelocity();
                //if (_enemy.CanAttack())
                    //_enemy.stateMachine.ChangeState(_enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(_player.position, _enemy.transform.position) > _enemy.attackDistance * 3f)
                _enemy.stateMachine.ChangeState(_enemy.idleState);
        }

    }
}
