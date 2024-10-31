using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player player, PlayerStateMachine playerStateMachine, string aniBoolName) : base(player, playerStateMachine, aniBoolName)
    {
    }

    public override void AnimFinishTrigger()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCA", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.skill.parry.ParryLogic();

        if (stateTimer < 0 || aniTrigger)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
