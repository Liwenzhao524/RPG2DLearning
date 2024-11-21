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

        if(Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.canUseBlackhole && player.skill.blackhole.CanUseSkill()) 
            player.stateMachine.ChangeState(player.blackholeState);

        if(Input.GetMouseButtonDown(1) && HasNoSword())
            player.stateMachine.ChangeState(player.aimSwordState);

        if(Input.GetKeyDown(KeyCode.Q) && player.skill.parry.canParry && player.skill.parry.CanUseSkill()) 
            player.stateMachine.ChangeState(player.counterAttackState);

        if(Input.GetMouseButton(0))
            player.stateMachine.ChangeState(player.primeAttackState);
        
        if(!player.IsGroundDetected())
            player.stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            player.stateMachine.ChangeState(player.jumpState);
    }

    /// <summary>
    /// 检查是否无剑 有则触发返回
    /// </summary>
    /// <returns>无则返回true</returns>
    private bool HasNoSword()
    {
        if (!player.sword) return true;

        player.sword.GetComponent<Skill_Sword_Controller>().ReturnSword();
        return false;
    }
}
