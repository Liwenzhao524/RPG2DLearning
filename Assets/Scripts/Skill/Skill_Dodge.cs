using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Dodge : Skill
{
    [Header("Basic")]
    [SerializeField] UI_SkillTreeSlot dodgeUnlock;
    bool _canDodge = true;  //debuglog:见下
    bool _evasionAdded;

    [Header("Dodge Clone")]
    [SerializeField] UI_SkillTreeSlot dodgeCloneUnlock;
    bool _canDodgeClone;

    protected override void Start ()
    {
        base.Start();

        // debuglog:Start()执行顺序又出问题UI_SkillTreeSlot dodgeUnlock
        dodgeUnlock.GetComponent<Button>().onClick.AddListener(DodgeUnlock);

        dodgeCloneUnlock.GetComponent<Button>().onClick.AddListener(DodgeColneUnlock);
    }

    void AddDodge ()
    {
        if(_canDodge && !_evasionAdded)
        {
            player.stats.evasion.AddModifier(10);
            _evasionAdded = true;
            Inventory.instance.UpdateStatsUI();
        }
    }

    public void DodgeClone ()
    {
        if (_canDodgeClone)
        {
            Transform target = FindClosestEnemy();
            player.skill.clone.CreateClone(target, new Vector3(1.2f * player.faceDir, 0));
        }
    }

    void DodgeUnlock ()
    {
        if(dodgeUnlock.unlocked)
            _canDodge = true;
        AddDodge();
    }

    void DodgeColneUnlock ()
    {
        if(dodgeCloneUnlock.unlocked)
            _canDodgeClone = true;
    }
}
