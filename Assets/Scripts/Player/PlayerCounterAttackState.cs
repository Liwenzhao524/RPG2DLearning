using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    bool createSingleClone;
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
        _player.SetZeroVelocity();
        createSingleClone = true;
        stateTimer = _player.counterAttackDuration;
        _player.anim.SetBool("SuccessfulCA", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 100;
                    _player.anim.SetBool("SuccessfulCA", true);
                    if(createSingleClone)
                    {
                        createSingleClone = false;
                        _player.skill.clone.CloneCounterAttack(hit.transform);
                    }
                }
        }

        if (stateTimer < 0 || _aniTrigger)
        {
            _player.stateMachine.ChangeState(_player.idleState);
        }
    }
}
