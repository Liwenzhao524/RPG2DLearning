using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = _player.dashDuraTime;
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetVelocity(0, _rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocity(_player.dashSpeed * _player.dashDir, 0);

        if(stateTimer < 0) 
            _player.stateMachine.ChangeState(_player.idleState);

        if(_player.IsWallDetected() && !_player.IsGroundDetected())
        {
            _player.stateMachine.ChangeState(_player.wallSlideState);
        }
        
    }
}
