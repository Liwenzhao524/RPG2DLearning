using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState {  get; private set; }

    public void Init(PlayerState state)
    {
        currentState = state;
        currentState.Enter();
    }

    public void ChangeState(PlayerState newstate)
    {
        currentState.Exit();
        currentState = newstate;
        currentState.Enter();
    }
}
