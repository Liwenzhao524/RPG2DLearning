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
        
        if(player.IsWallDetected())
            player.stateMachine.ChangeState(player.wallSlideState);

        if(xinput != 0)
            player.SetVelocity(player.moveSpeed * xinput * 0.8f, rb.velocity.y);

        if(rb.velocity.y < 0.001f && player.IsGroundDetected())
            player.stateMachine.ChangeState(player.idleState);
    }
}
