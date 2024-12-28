using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Slime : Enemy
{
    #region State
    public SlimeIdleState idleState {  get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeGroundState groundState { get; private set; }
    
    public SlimeAttackState attackState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    public SlimeStunState stunState { get; private set; }

    #endregion

    protected override void Awake ()
    {
        base.Awake();
        battleState = new SlimeBattleState (this, stateMachine, "s");

    }
}
