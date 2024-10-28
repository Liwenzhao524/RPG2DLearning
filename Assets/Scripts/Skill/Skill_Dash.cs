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
    public bool dashUnlock;
    [SerializeField] UI_SkillTreeSlot unlockDashButton;

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start ()
    {
        base.Start();
        unlockDashButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
    }

    void UnlockDash ()
    {
        if(unlockDashButton.unlocked)
            dashUnlock = true;
    }
}
