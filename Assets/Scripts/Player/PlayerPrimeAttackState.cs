using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeAttackState : PlayerState
{
    int _comboCounter;  // ����������
    float _attackDir;
    float _lastAttackTime;
    readonly float _comboWindow = 1;  // ���������
    public PlayerPrimeAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // ������� �� ��ʱ�� �ص�һ��
        if(_comboCounter > 2 || Time.time - _lastAttackTime > _comboWindow)
            _comboCounter = 0;

        #region Attack Direction
        
        if (xinput != 0) _attackDir = xinput;
        else _attackDir = player.faceDir;
        #endregion

        // ��������Сλ��
        player.SetVelocity(player.attackMove[_comboCounter].x * _attackDir, 
                            player.attackMove[_comboCounter].y);

        player.anim.SetInteger("comboCounter", _comboCounter);
        player.anim.speed = player.attackSpeed;

        stateTimer = 0.2f;  // ����ǰ ά�����ٻ���ʱ��
    }

    public override void Exit()
    {
        base.Exit();
        _comboCounter ++ ;
        _lastAttackTime = Time.time;

        // ��idle -> move ���⹥����϶�ƶ�
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        // ��ʼ���� ���ƶ�
        if(stateTimer < 0)
            player.SetZeroVelocity();

        if(aniTrigger)
            player.stateMachine.ChangeState(player.idleState);
    }
}
