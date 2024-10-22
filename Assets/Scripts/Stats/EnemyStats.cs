using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy _enemy;

    [Header("Level")]
    [SerializeField] int level = 1;
    [SerializeField] float growPercentage;

    // Start is called before the first frame update
    protected override void Start()
    {
        LevelGrowth();

        base.Start();
        _enemy = entity as Enemy;
    }

    private void LevelGrowth ()
    {
        ModifyStat(ATK);
        ModifyStat(critChance);
        ModifyStat(critDamage);

        ModifyStat(maxHP);
        ModifyStat(armor);
        ModifyStat(magicResistence);

        ModifyStat(iceATK);
        ModifyStat(fireATK);
        ModifyStat(lightningATK);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void ModifyStat (Stats stats)
    {
        for(int i = 1; i < level; i ++)
        {
            stats.AddModifier(stats.GetValue() * growPercentage);
        }
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
