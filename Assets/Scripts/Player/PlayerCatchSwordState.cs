using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    Transform _sword;
    public PlayerCatchSwordState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _sword = player.sword.transform;

        // ����
        rb.velocity = new Vector2(player.swordReturnForce * -player.faceDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.05f);  // ���� ����δ����ʱ�ƶ�ָ��µĻ���
    }

    public override void Update()
    {
        base.Update();

        if(aniTrigger)
            stateMachine.ChangeState(player.idleState);
    }
}
