using UnityEngine;

public class SkeletonStunState : EnemyState
{
    Enemy_Skeleton _enemy;
    public SkeletonStunState (Enemy enemyBase, EnemyStateMachine enemyStateMachine, string aniBoolName) : base(enemyBase, enemyStateMachine, aniBoolName)
    {
        _enemy = enemyBase as Enemy_Skeleton;
    }

    public override void AnimFinishTrigger ()
    {
        base.AnimFinishTrigger();
    }

    public override void Enter ()
    {
        base.Enter();

        stateTimer = _enemy.stunDuration;
        rb.velocity = new Vector2(_enemy.stunDirection.x * -_enemy.faceDir, _enemy.stunDirection.y);
        _enemy.fx.InvokeRepeating(nameof(_enemy.fx.RedBlink), 0, 0.1f);
    }

    public override void Exit ()
    {
        base.Exit();
        _enemy.fx.Invoke(nameof(_enemy.fx.CancelColorChange), 0);
    }

    public override void Update ()
    {
        base.Update();
        if (stateTimer < 0)
            _enemy.stateMachine.ChangeState(_enemy.idleState);
    }
}
