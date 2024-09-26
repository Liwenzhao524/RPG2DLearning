using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    #region State
    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        moveState = new EnemyMoveState(this, stateMachine, "Move");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update(); 
        stateMachine.currentState.Update();
    }
}
