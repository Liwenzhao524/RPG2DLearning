using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    bool _skillUsed;
    float _flyTime = 0.2f;

    float _defaultGravity;
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

        AudioManager.instance.PlaySFX("Blackhole");
        _defaultGravity = rb.gravityScale;

        _skillUsed = false;
        rb.gravityScale = 0;
        stateTimer = _flyTime;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = _defaultGravity;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);
            if (!_skillUsed)
            {
                //if (SkillManager.instance.blackhole.CanUseSkill())
                    _skillUsed = true;
            }
            else if (SkillManager.instance.blackhole.BlackholeEnd())
                player.stateMachine.ChangeState(player.airState);
        }
    }

    
}
