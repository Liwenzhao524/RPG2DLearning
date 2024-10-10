using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// µÐÈË×´Ì¬¸¸Àà
/// </summary>
public class EnemyState
{
    protected Enemy _enemyBase;
    protected EnemyStateMachine _enemyStateMachine;
    protected Rigidbody2D _rb;

    private string _aniBoolName;
    protected bool _anitrigger;

    protected float stateTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName)
    {
        _enemyBase = enemyBase;
        _enemyStateMachine = enemyStateMachine;
        _aniBoolName = aniBoolName;
    }

    public virtual void Enter()
    {
        _anitrigger = false;
        _rb = _enemyBase.rb;
        _enemyBase.anim.SetBool(_aniBoolName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        _enemyBase.anim.SetBool(_aniBoolName, false);
    }

    public virtual void AnimFinishTrigger()
    {
        _anitrigger = true;
    }
}
