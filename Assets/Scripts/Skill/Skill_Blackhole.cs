using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Blackhole : Skill
{
    [SerializeField] GameObject blackholePrefab;
    [Header("Basic Info")]
    [SerializeField] float blackholeDuration = 8;
    [SerializeField] float maxSize = 15;
    [SerializeField] float growSpeed = 5;
    [SerializeField] float shrinkSpeed = 5;

    [Header("Attack Info")]
    [SerializeField] int cloneAttackCount;
    [SerializeField] float cloneAttackCoolDown;

    [Header("Blackhole Unlock")]
    public bool canUseBlackhole;
    [SerializeField] UI_SkillTreeSlot blackholeUnlock;

    Skill_Blackhole_Controller _ctrl;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        _ctrl = newBlackhole.GetComponent<Skill_Blackhole_Controller>();
        _ctrl.SetUpBlackhole(maxSize, growSpeed, shrinkSpeed, cloneAttackCount, cloneAttackCoolDown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlock.GetComponent<Button>().onClick.AddListener(BlackholeUnlock);
    }

    protected override void Update()
    {
        base.Update();
    }

    void BlackholeUnlock ()
    {
        if (blackholeUnlock.unlocked)
            canUseBlackhole = true;
    }

    public bool BlackholeEnd()
    {
        if (!_ctrl) return false;

        if(_ctrl.playerCanExitSkill)
        {
            _ctrl = null;
            return true;
        }

        return false;
    }
}
