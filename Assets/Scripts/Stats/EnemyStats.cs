using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy _enemy;
    ItemObject_Drop _dropSystem;

    [Header("Drop Item")]
    [SerializeField] int _dropAmount;
    [SerializeField] ItemData[] _possibleDrops;
    List<ItemData> _dropList = new();
    [SerializeField] Stats _dropMoney;

    [Header("Level")]
    [SerializeField] int _level = 1;
    [SerializeField] float _growPercentage;

    // Start is called before the first frame update
    protected override void Start()
    {
        _dropMoney.SetDefaultValue(100);
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

        ModifyStat(_dropMoney);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void ModifyStat (Stats stats)
    {
        for(int i = 1; i < _level; i ++)
        {
            stats.AddModifier(stats.GetValue() * _growPercentage);
        }
    }

    public void GenerateDrop ()
    {
        for (int i = 0; i < _possibleDrops.Length; i++)
        {
            if (Random.Range(0, 100) < _possibleDrops[i].dropChance)
            {
                _dropList.Add(_possibleDrops[i]);
            }
        }

        for (int i = 0; i < _dropAmount; i++)
        {
            if (_dropList.Count <= 0) break;
            ItemData randomDrop = _dropList[Random.Range(0, _dropList.Count - 1)];

            _dropList.Remove(randomDrop);
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
        if (SkillManager.instance.sword.beVulnurable) damage *= 1.1f;

        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        _enemy.Die();

        PlayerStats stats = PlayerManager.instance.player.stats as PlayerStats;
        stats.currentMoney += (int)_dropMoney.GetValue();
        Destroy(gameObject, 5);
        if(_enemy.stats.isDead)
            GenerateDrop();
    }
}
