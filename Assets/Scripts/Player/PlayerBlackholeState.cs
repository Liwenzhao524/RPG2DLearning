using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    bool skillUsed;
    float flyTime = 0.2f;

    float defaultGravity;
    public PlayerBlackholeState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void AnimFinishTrigger()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = _rb.gravityScale;

        skillUsed = false;
        _rb.gravityScale = 0;
        stateTimer = flyTime;
    }

    public override void Exit()
    {
        base.Exit();
        _rb.gravityScale = defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            _rb.velocity = new Vector2(0, 15);
        }

        if(stateTimer < 0)
        {
            _rb.velocity = new Vector2(0, -0.1f);
            if (!skillUsed)
            {
                if (SkillManager._instance.blackhole.CanUseSkill())
                    skillUsed = true;
            }
            else if (SkillManager._instance.blackhole.BlackholeEnd())
                _player.stateMachine.ChangeState(_player.airState);
        }
    }

    
}
