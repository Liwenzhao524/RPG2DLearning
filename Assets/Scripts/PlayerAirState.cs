using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
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
        
        if(_player.IsWallDetected())
        {
            _player.stateMachine.ChangeState(_player.wallSlideState);
        }

        if(_xinput != 0)
        {
            _player.SetVelocity(_player.moveSpeed * _xinput * 0.8f, _rb.velocity.y);
        }

        if(_rb.velocity.y == 0)
        {
            _player.stateMachine.ChangeState(_player.idleState);
        }
    }
}
