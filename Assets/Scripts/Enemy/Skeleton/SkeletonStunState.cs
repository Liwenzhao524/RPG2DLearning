using UnityEngine;

public class SkeletonStunState : EnemyStunState
{
    public SkeletonStunState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
    }

    public override void AnimFinishTrigger ()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter ()
    {
        base.Enter();
    }

    public override void Exit ()
    {
        base.Exit();
    }

    public override void Update ()
    {
        base.Update();
    }
}
