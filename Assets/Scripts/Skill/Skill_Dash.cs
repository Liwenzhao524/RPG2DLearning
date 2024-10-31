using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 冲刺 主要逻辑在Player 此处提供冷却时间等属性
/// </summary>
public class Skill_Dash : Skill
{
    [Header("Dash")]
    [SerializeField] UI_SkillTreeSlot _dashUnlock;
    public bool canDash {  get; private set; }

    [Header("Clone Dash Start")]
    [SerializeField] UI_SkillTreeSlot _cloneDashStartUnlock;
    bool _canCloneDashStart;

    [Header("Clone Dash End")]
    [SerializeField] UI_SkillTreeSlot _cloneDashEndUnlock;
    bool _canCloneDashEnd;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start ()
    {
        base.Start();
        _dashUnlock.GetComponent<Button>().onClick.AddListener(UnlockDash);
        _cloneDashStartUnlock.GetComponent<Button>().onClick.AddListener(CloneDashStartUnlock);
        _cloneDashEndUnlock.GetComponent<Button>().onClick.AddListener(CloneDashEndUnlock);
    }

    /// <summary>
    /// 冲刺起始位置
    /// </summary>
    public void CloneDashStart ()
    {
        if (_canCloneDashStart)
        {
            player.skill.clone.CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// 冲刺结束位置
    /// </summary>
    public void CloneDashEnd ()
    {
        if (_canCloneDashEnd)
        {
            player.skill.clone.CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }


    void UnlockDash ()
    {
        if(_dashUnlock.unlocked)
            canDash = true;
    }
    void CloneDashStartUnlock ()
    {
        if (_cloneDashStartUnlock.unlocked)
            _canCloneDashStart = true;
    }

    void CloneDashEndUnlock ()
    {
        if (_cloneDashEndUnlock.unlocked)
            _canCloneDashEnd = true;
    }
}
