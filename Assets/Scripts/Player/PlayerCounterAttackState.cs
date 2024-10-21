using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    bool _createSingleClone;
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
        _createSingleClone = true;
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 100;
                    player.anim.SetBool("SuccessfulCA", true);
                    if(_createSingleClone)
                    {
                        _createSingleClone = false;
                        player.skill.clone.CloneCounterAttack(hit.transform);
                    }
                }
        }

        if (stateTimer < 0 || aniTrigger)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
