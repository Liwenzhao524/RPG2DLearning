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
        if(xinput * player.faceDir > 0)
            player.SetVelocity(0, 1.2f * player.jumpForce);
        else player.SetVelocity(5 * -player.faceDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
            player.stateMachine.ChangeState(player.airState);

        if (player.IsGroundDetected())
            player.stateMachine.ChangeState(player.idleState);
    }
}
