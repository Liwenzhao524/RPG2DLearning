using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    protected Transform _player;
    protected int _moveDir;
    public EnemyBattleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void Enter ()
    {
        base.Enter();
        _player = PlayerManager.instance.player.transform;
        //if (_player.GetComponent<PlayerStats>().isDead)
            //enemyBase.stateMachine.ChangeState(enemyBase.moveState);
    }

    public override void Update ()
    {
        base.Update();
        if (_player.position.x > rb.position.x)
            _moveDir = 1;
        else if (_player.position.x < rb.position.x)
            _moveDir = -1;

        enemyBase.SetVelocity(enemyBase.moveSpeed * _moveDir, rb.velocity.y);
    }

    public override void Exit ()
    {
        base.Exit();
    }
}
