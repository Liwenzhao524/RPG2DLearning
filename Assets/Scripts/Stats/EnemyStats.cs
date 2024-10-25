using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy _enemy;
    ItemObject_Drop _dropSystem;

    [Header("Drop Item")]
    [SerializeField] int dropAmount;
    [SerializeField] ItemData[] possibleDrops;
    List<ItemData> dropList = new();

    [Header("Level")]
    [SerializeField] int level = 1;
    [SerializeField] float growPercentage;

    // Start is called before the first frame update
    protected override void Start()
    {
        LevelGrowth();

        base.Start();
        _enemy = entity as Enemy;
        _dropSystem = GetComponent<ItemObject_Drop>();
    }

    private void LevelGrowth ()
    {
        ModifyStat(ATK);
        ModifyStat(critChance);
        ModifyStat(critDamage);

        ModifyStat(maxHP);
        ModifyStat(armor);
        ModifyStat(magicResist);

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

    public void GenerateDrop ()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) < possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        for (int i = 0; i < dropAmount; i++)
        {
            if (dropList.Count <= 0) break;
            ItemData randomDrop = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomDrop);
            _dropSystem.DropItem(randomDrop);
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
        Destroy(gameObject, 5);
        if(_enemy.stats.isDead)
            GenerateDrop();
    }
}
