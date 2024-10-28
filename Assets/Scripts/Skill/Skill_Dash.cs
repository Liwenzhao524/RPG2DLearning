using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��� ��Ҫ�߼���Player �˴��ṩ��ȴʱ�������
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
