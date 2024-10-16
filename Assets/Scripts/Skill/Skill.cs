using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能父类
/// </summary>
public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
    protected Player _player;

    protected virtual void Start()
    {
        _player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// 能否使用技能（冷却
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {
        // 技能执行
    }
}
