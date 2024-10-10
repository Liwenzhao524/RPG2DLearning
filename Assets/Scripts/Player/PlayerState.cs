using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���״̬����
/// </summary>
public class PlayerState
{
    protected Player _player;
    protected PlayerStateMachine _playerStateMachine;
    protected Rigidbody2D _rb;

    protected float _xinput;
    protected float _yinput;
    private string _aniBoolName;  // ����״̬��Ӧ������
    protected bool _aniTrigger;  // �����¼�����

    protected float stateTimer;  // ĳЩ״̬����ʱ��

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
        _aniTrigger = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        _xinput = Input.GetAxisRaw("Horizontal");
        _yinput = Input.GetAxisRaw("Vertical");
        _player.anim.SetFloat("yVelocity", _rb.velocity.y);
    }

    public virtual void Exit()
    {
        _player.anim.SetBool(_aniBoolName, false);
    }

    public virtual void AnimFinishTrigger()
    {
        _aniTrigger = true;
    }
}
