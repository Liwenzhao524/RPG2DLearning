using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform _sword;
    public PlayerCatchSwordState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _sword = _player.sword.transform;

        _rb.velocity = new Vector2(_player.swordReturnForce * -_player.faceDir, _rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        _player.StartCoroutine("BusyFor", 0.05f);
    }

    public override void Update()
    {
        base.Update();

        if(_aniTrigger)
            _stateMachine.ChangeState(_player.idleState);
    }
}
