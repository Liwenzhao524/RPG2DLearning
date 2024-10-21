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

        // 反震
        rb.velocity = new Vector2(player.swordReturnForce * -player.faceDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.05f);  // 避免 动画未结束时移动指令导致的滑步
    }

    public override void Update()
    {
        base.Update();

        if(aniTrigger)
            stateMachine.ChangeState(player.idleState);
    }
}
