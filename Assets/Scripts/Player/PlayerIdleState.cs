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

        // ���ǽ ���ƶ�
        if (xinput == player.faceDir && player.IsWallDetected()) return;

        // ���ƶ����� �� ���״̬����ռ�� �����ƶ�
        if (xinput != 0 && !player.isBusy)
           stateMachine.ChangeState(player.moveState); 
    }

    public override void Exit() 
    { 
        base.Exit();
    }
}
