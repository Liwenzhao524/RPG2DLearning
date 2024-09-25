using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
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

        if (_yinput < 0) _player.SetVelocity(0, _rb.velocity.y);
        else _player.SetVelocity(0, _rb.velocity.y * 0.7f);

        if(_xinput * _player.faceDir < 0 || _player.IsGroundDetected())
        {
            _player.stateMachine.ChangeState(_player.idleState);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _player.stateMachine.ChangeState(_player.wallJumpState);
            //return;
        }
    }
}
