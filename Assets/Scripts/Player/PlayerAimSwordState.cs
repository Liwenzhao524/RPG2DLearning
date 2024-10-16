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
        SkillManager.instance.sword.SetDotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        _player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        _player.SetZeroVelocity();
        if(Input.GetMouseButtonUp(1))
            _player.stateMachine.ChangeState(_player.idleState);

        // 瞄准时人物转向
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _player.FlipController(mousePos.x - _player.transform.position.x);
    }
}
