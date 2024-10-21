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

        // ��������
        if (yinput < 0) player.SetVelocity(0, rb.velocity.y);
        else player.SetVelocity(0, rb.velocity.y * 0.7f);

        // ��ǰ�����ƶ� �� ��أ��������
        if(xinput * player.faceDir < 0 || player.IsGroundDetected())
            player.stateMachine.ChangeState(player.idleState);

        // ��ǽ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.ChangeState(player.wallJumpState);
            //return;
        }
    }
}
