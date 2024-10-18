using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player _player;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void DoDamageTo(CharacterStats target)
    {
        base.DoDamageTo(target);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        _player.Die();
    }
}
