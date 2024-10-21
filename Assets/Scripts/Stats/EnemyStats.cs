using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy _enemy;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _enemy = entity as Enemy;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DoDamageTo(CharacterStats target)
    {
        base.DoDamageTo(target);
    }

    public override void DoMagicDamageTo(CharacterStats target)
    {
        base.DoMagicDamageTo(target);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        _enemy.Die();
    }
}
