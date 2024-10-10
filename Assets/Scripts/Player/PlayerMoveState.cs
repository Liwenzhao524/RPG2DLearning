using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
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

        _player.SetVelocity(_xinput * _player.moveSpeed, _rb.velocity.y);

        // 移动指令消失 或 撞墙时停止移动
        if(_xinput == 0 || _player.IsWallDetected())
            _playerStateMachine.ChangeState(_player.idleState);
    }
}
