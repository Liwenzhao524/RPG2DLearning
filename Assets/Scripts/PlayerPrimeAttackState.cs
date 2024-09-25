using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeAttackState : PlayerState
{
    public PlayerPrimeAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(_rb.velocity.x * 0.3f, _rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(_animTrigger)
            _player.stateMachine.ChangeState(_player.idleState);
    }
}
