using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    public EnemyState defaultState { get; private set; }

    public void Init(EnemyState state)
    {
        currentState = state;
        defaultState = state;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newstate)
    {
        currentState.Exit();
        currentState = newstate;
        currentState.Enter();
    }
}
