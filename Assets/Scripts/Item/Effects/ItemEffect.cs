using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特殊效果 词条
/// </summary>

public class ItemEffect : ScriptableObject
{
    protected Player player;
    protected PlayerStats stats;
    public virtual void ExecuteEffect (Transform target)
    {
        player = PlayerManager.instance.player;
        stats = player.stats as PlayerStats;
    }
}
