using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : EnemyIdleState
{
    Enemy_Boss _enemy;
    public BossIdleState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Boss;
    }

    public override void Enter ()
    {
        base.Enter();
        stateTimer = _enemy.idleTime;
    }

    public override void Exit ()
    {
        base.Exit();
        AudioManager.instance.PlaySFX("BossIdle", _enemy.transform);
    }

    public override void Update ()
    {
        stateTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.V))
            _enemy.stateMachine.ChangeState(_enemy.teleportState);

        if(stateTimer < 0 && Vector2.Distance(player.position, _enemy.transform.position) < _enemy.attackDistance * 3f)
            _enemy.stateMachine.ChangeState(_enemy.battleState);

    }
}
