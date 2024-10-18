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
        SkillManager._instance.clone.CloneDashStart();
        stateTimer = _player.dashDuraTime;  // 冲刺持续时间
    }

    public override void Exit()
    {
        base.Exit();
        _player.SetVelocity(0, _rb.velocity.y);
        SkillManager._instance.clone.CloneDashEnd();
    }

    public override void Update()
    {
        base.Update();

        _player.SetVelocity(_player.dashSpeed * _player.dashDir, 0);

        if(stateTimer < 0) 
            _player.stateMachine.ChangeState(_player.idleState);

        // 空中冲刺到墙可以滑墙
        if(_player.IsWallDetected() && !_player.IsGroundDetected())
            _player.stateMachine.ChangeState(_player.wallSlideState);
    }
}
