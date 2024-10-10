using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeAttackState : PlayerState
{
    private int comboCounter;  // 连击计数器
    private float attackDir;
    private float lastAttackTime;
    private float comboWindow = 1;  // 连击最大间隔
    public PlayerPrimeAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 完成连击 或 超时， 回第一下
        if(comboCounter > 2 || Time.time - lastAttackTime > comboWindow)
            comboCounter = 0;

        #region Attack Direction
        
        if (_xinput != 0) attackDir = _xinput;
        else attackDir = _player.faceDir;
        #endregion

        // 攻击动作小位移
        _player.SetVelocity(_player.attackMove[comboCounter].x * attackDir, 
                            _player.attackMove[comboCounter].y);

        _player.anim.SetInteger("comboCounter", comboCounter);
        _player.anim.speed = _player.attackSpeed;

        stateTimer = 0.2f;  // 攻击前 维持移速滑步时间
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter ++ ;
        lastAttackTime = Time.time;

        // 锁idle -> move 避免攻击间隙移动
        _player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        // 开始动作 禁移动
        if(stateTimer < 0)
            _player.SetZeroVelocity();

        if(_aniTrigger)
            _player.stateMachine.ChangeState(_player.idleState);
    }
}
