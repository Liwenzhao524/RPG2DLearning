using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAniTrigger : EnemyAniTrigger
{
    Enemy_Boss _enemy;
    protected override void AniTrigger ()
    {
        base.AniTrigger();
    }

    protected override void AttackTrigger ()
    {
        base.AttackTrigger();
    }

    protected override void Start ()
    {
        base.Start();
        _enemy = enemy as Enemy_Boss;
    }

    void Relocate () => _enemy.FindPosition();

    void MakeInvisible() => _enemy.fx.MakeTransparent(true);

    void Makevisible() => _enemy.fx.MakeTransparent(false);

}
