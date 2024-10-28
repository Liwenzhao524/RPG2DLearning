using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.sword.SetDotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
        if(Input.GetMouseButtonUp(1))
            player.stateMachine.ChangeState(player.idleState);

        // ��׼ʱ����ת��
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.FlipController(mousePos.x - player.transform.position.x);
    }
}
