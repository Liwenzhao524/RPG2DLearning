using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _playerStateMachine;
    protected Rigidbody2D _rb;
    protected float _xinput;
    private string _aniBoolName;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName)
    {
        _player = player;
        _playerStateMachine = playerStateMachine;
        _aniBoolName = aniBoolName;
    }

    public virtual void Enter()
    {
        _player.anim.SetBool(_aniBoolName, true);
        _rb = _player.rb;
    }

    public virtual void Update()
    {
        _xinput = Input.GetAxisRaw("Horizontal");
        _player.anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    public virtual void Exit()
    {
        _player.anim.SetBool(_aniBoolName, false);
    }
}
