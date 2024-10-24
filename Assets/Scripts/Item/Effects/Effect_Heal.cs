using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal")]
public class Effect_Heal : ItemEffect
{
    [Range(0f, 1f)]
    public float healPercentage;

    public override void ExecuteEffect (Transform target)
    {
        base.ExecuteEffect(target);

        float healHP = stats.GetMaxHP() * healPercentage;

        stats.IncreaseHP(healHP);
    }
}
