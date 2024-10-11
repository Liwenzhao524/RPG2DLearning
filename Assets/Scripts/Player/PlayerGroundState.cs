using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
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

        if(Input.GetKeyDown(KeyCode.Q)) 
            _player.stateMachine.ChangeState(_player.counterAttackState);

        if(Input.GetMouseButton(0)) // Á¬°´
            _player.stateMachine.ChangeState(_player.primeAttackState);
        
        if(!_player.IsGroundDetected())
            _player.stateMachine.ChangeState(_player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && _player.IsGroundDetected())
            _player.stateMachine.ChangeState(_player.jumpState);
    }
}
