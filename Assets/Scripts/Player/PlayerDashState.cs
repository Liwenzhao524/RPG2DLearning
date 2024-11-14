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
        player.stats.isInvensible = true;
        player.skill.dash.CloneDashStart();
        stateTimer = player.dashDuraTime;  // ��̳���ʱ��
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.velocity.y);
        player.skill.dash.CloneDashEnd();
        player.stats.isInvensible = false;  
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if(stateTimer < 0) 
            player.stateMachine.ChangeState(player.idleState);

        // ���г�̵�ǽ���Ի�ǽ
        if(player.IsWallDetected() && !player.IsGroundDetected())
            player.stateMachine.ChangeState(player.wallSlideState);
    }
}
