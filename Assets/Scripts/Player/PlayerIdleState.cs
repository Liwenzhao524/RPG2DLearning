using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        // 面对墙 不移动
        if (xinput == player.faceDir && player.IsWallDetected()) return;

        // 有移动输入 且 玩家状态不被占用 可以移动
        if (xinput != 0 && !player.isBusy)
           stateMachine.ChangeState(player.moveState); 
    }

    public override void Exit() 
    { 
        base.Exit();
    }
}
