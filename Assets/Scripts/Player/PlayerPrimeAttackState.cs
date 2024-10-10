using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeAttackState : PlayerState
{
    private int comboCounter;  // ����������
    private float attackDir;
    private float lastAttackTime;
    private float comboWindow = 1;  // ���������
    public PlayerPrimeAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // ������� �� ��ʱ�� �ص�һ��
        if(comboCounter > 2 || Time.time - lastAttackTime > comboWindow)
            comboCounter = 0;

        #region Attack Direction
        
        if (_xinput != 0) attackDir = _xinput;
        else attackDir = _player.faceDir;
        #endregion

        // ��������Сλ��
        _player.SetVelocity(_player.attackMove[comboCounter].x * attackDir, 
                            _player.attackMove[comboCounter].y);

        _player.anim.SetInteger("comboCounter", comboCounter);
        _player.anim.speed = _player.attackSpeed;

        stateTimer = 0.2f;  // ����ǰ ά�����ٻ���ʱ��
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter ++ ;
        lastAttackTime = Time.time;

        // ��idle -> move ���⹥����϶�ƶ�
        _player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        // ��ʼ���� ���ƶ�
        if(stateTimer < 0)
            _player.SetZeroVelocity();

        if(_aniTrigger)
            _player.stateMachine.ChangeState(_player.idleState);
    }
}
