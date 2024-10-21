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
        if (yinput < 0) player.SetVelocity(0, rb.velocity.y);
        else player.SetVelocity(0, rb.velocity.y * 0.7f);

        // 提前反向移动 或 落地，进入待机
        if(xinput * player.faceDir < 0 || player.IsGroundDetected())
            player.stateMachine.ChangeState(player.idleState);

        // 蹬墙跳
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.ChangeState(player.wallJumpState);
            //return;
        }
    }
}
