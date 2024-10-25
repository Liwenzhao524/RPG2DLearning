using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff")]
public class Effect_Buff : ItemEffect
{
    [SerializeField] StatsType buffType;
    [SerializeField] float buffModifier;
    [SerializeField] float buffDuration;


    public override void ExecuteEffect (Transform target)
    {

        Stats statsToBuff = stats.GetStatsByType(buffType);

        stats.AddBuffToStats(statsToBuff, buffModifier, buffDuration);
    }
} 
