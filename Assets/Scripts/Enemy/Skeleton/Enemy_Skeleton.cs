using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle");
        moveState = new SkeletonMoveState(this, stateMachine, "Move");
        battleState = new SkeletonBattleState(this, stateMachine, "Move");
        attackState = new SkeletonAttackState(this, stateMachine, "Attack");
        stunState = new SkeletonStunState(this, stateMachine, "Stunned");
        deadState = new SkeletonDeadState(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();

    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
    
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }
}
