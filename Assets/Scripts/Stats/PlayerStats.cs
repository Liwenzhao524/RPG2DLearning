using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : CharacterStats, ISaveManager
{
    Player _player;

    [Space]
    public int currentMoney;

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

    public override void DoDamageTo (CharacterStats target, float cloneATK = 1)
    {
        base.DoDamageTo(target, cloneATK);
    }

    public override void DoMagicDamageTo(CharacterStats target)
    {
        base.DoMagicDamageTo(target);
    }

    public override void TakeDamage(float damage)
    {
        if (damage > GetMaxHP() * 0.3f && !isDead)
        {
            _player.SetKnockDistance(new Vector2(8, 4));
            AudioManager.instance.PlaySFX("PlayerHitted");
        }

        base.TakeDamage(damage);
            
            _player.SetKnockDistance(Vector2.zero);
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

    public bool HasEnoughMoney(int price)
    {
        if(currentMoney < price)
        {
            Debug.Log("No Enough Money");
            return false;
        }
        else
        {
            currentMoney -= price;
            return true;
        }
    }

    public int ReturnCurrentMoney() => currentMoney;

    public void LoadGame (GameData gameData)
    {
        currentMoney = gameData.money;
    }

    public void SaveGame (ref GameData gameData)
    {
        gameData.money = currentMoney;
    }
}
