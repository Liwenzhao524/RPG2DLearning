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
        //AudioManager.instance.PlaySFX("PlayerWalk");
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.StopSFX("PlayerWalk");
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xinput * player.moveSpeed, rb.velocity.y);

        // 移动指令消失 或 撞墙时停止移动
        if(xinput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
