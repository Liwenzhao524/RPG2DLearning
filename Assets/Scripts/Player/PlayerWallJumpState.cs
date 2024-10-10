using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.2f;
        
        // ÌøÔ¾·½Ïò
        if(_xinput * _player.faceDir > 0)
            _player.SetVelocity(0, 1.2f * _player.jumpForce);
        else _player.SetVelocity(5 * -_player.faceDir, _player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            _player.stateMachine.ChangeState(_player.airState);

        if (_player.IsGroundDetected())
            _player.stateMachine.ChangeState(_player.idleState);
    }
}
