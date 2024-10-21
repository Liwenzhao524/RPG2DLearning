using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimeAttackState : PlayerState
{
    int _comboCounter;  // 连击计数器
    float _attackDir;
    float _lastAttackTime;
    readonly float _comboWindow = 1;  // 连击最大间隔
    public PlayerPrimeAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 完成连击 或 超时， 回第一下
        if(_comboCounter > 2 || Time.time - _lastAttackTime > _comboWindow)
            _comboCounter = 0;

        #region Attack Direction
        
        if (xinput != 0) _attackDir = xinput;
        else _attackDir = player.faceDir;
        #endregion

        // 攻击动作小位移
        player.SetVelocity(player.attackMove[_comboCounter].x * _attackDir, 
                            player.attackMove[_comboCounter].y);

        player.anim.SetInteger("comboCounter", _comboCounter);
        player.anim.speed = player.attackSpeed;

        stateTimer = 0.2f;  // 攻击前 维持移速滑步时间
    }

    public override void Exit()
    {
        base.Exit();
        _comboCounter ++ ;
        _lastAttackTime = Time.time;

        // 锁idle -> move 避免攻击间隙移动
        player.StartCoroutine("BusyFor", 0.2f);
    }

    public override void Update()
    {
        base.Update();

        // 开始动作 禁移动
        if(stateTimer < 0)
            player.SetZeroVelocity();

        if(aniTrigger)
            player.stateMachine.ChangeState(player.idleState);
    }
}
