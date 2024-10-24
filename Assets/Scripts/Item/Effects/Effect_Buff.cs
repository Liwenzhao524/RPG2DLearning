using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsType
{
    strength,  
    agility,  
    intelligence,  
    vitality,
    
    maxHP,
    armor,
    evasion,
    magicResistence,

    ATK,
    critChance,
    critDamage,

    fireATK,
    iceATK,
    lightningATK
}


[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item Effect/Buff")]
public class Effect_Buff : ItemEffect
{
    [SerializeField] StatsType buffType;
    [SerializeField] float buffModifier;
    [SerializeField] float buffDuration;

    Dictionary<StatsType, Stats> _statsMap;

    public override void ExecuteEffect (Transform target)
    {
        _statsMap = new Dictionary<StatsType, Stats>
        {
            { StatsType.strength, stats.strength },
            { StatsType.agility, stats.agility },
            { StatsType.intelligence, stats.intelligence },
            { StatsType.vitality, stats.vitality },
            { StatsType.maxHP, stats.maxHP },
            { StatsType.armor, stats.armor },
            { StatsType.evasion, stats.evasion },
            { StatsType.magicResistence, stats.magicResistence },
            { StatsType.ATK, stats.ATK },
            { StatsType.critChance, stats.critChance },
            { StatsType.critDamage, stats.critDamage },
            { StatsType.fireATK, stats.fireATK },
            { StatsType.iceATK, stats.iceATK },
            { StatsType.lightningATK, stats.lightningATK }
        };

        if(_statsMap.TryGetValue(buffType, out Stats statsToBuff))
            stats.AddBuffToStats(statsToBuff, buffModifier, buffDuration);
    }
} 
