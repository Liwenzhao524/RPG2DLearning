using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player _player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _player = entity as Player;
    }

    // Update is called once per frame
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

    protected override void DecreaseHP (float damage)
    {
        base.DecreaseHP(damage);

        Inventory.instance.GetEquipmentByType(EquipmentType.Armor)?.ExecuteEffects(transform);
    }

    protected override void Die()
    {
        base.Die();
        _player.Die();
    }

    
}
