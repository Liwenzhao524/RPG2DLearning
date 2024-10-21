using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µÐÈË×´Ì¬¸¸Àà
/// </summary>
public class EnemyState
{
    protected Enemy enemyBase;

    protected EnemyStateMachine enemyStateMachine;
    protected Rigidbody2D rb;

    string _aniBoolName;
    protected bool anitrigger;

    protected float stateTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName)
    {
        this.enemyBase = enemyBase;
        this.enemyStateMachine = enemyStateMachine;
        _aniBoolName = aniBoolName;
    }

    public virtual void Enter()
    {
        anitrigger = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(_aniBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(_aniBoolName, false);

        enemyBase.AssignLastAniName(_aniBoolName);
    }

    public virtual void AnimFinishTrigger()
    {
        anitrigger = true;
    }
}
