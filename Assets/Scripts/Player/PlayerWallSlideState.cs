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

        // 加速下落
        if (_yinput < 0) _player.SetVelocity(0, _rb.velocity.y);
        else _player.SetVelocity(0, _rb.velocity.y * 0.7f);

        // 提前反向移动 或 落地，进入待机
        if(_xinput * _player.faceDir < 0 || _player.IsGroundDetected())
            _player.stateMachine.ChangeState(_player.idleState);

        // 蹬墙跳
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.stateMachine.ChangeState(_player.wallJumpState);
            //return;
        }
    }
}
