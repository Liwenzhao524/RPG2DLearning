using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Blackhole : Skill
{
    [Header("Blackhole Unlock")]
    [SerializeField] UI_SkillTreeSlot _blackholeUnlock;
    public bool canUseBlackhole{ get; private set;}

    [Header("Basic Info")]
    [SerializeField] GameObject _blackholePrefab;
    [SerializeField] float _blackholeDuration = 8;
    [SerializeField] float _maxSize = 15;
    [SerializeField] float _growSpeed = 5;
    [SerializeField] float _shrinkSpeed = 5;

    [Header("Attack Info")]
    [SerializeField] int _cloneAttackCount = 5;
    [SerializeField] float _cloneAttackCoolDown = 0.3f;


    Skill_Blackhole_Controller _ctrl;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        SkillManager.instance.blackholeUse = true;
        GameObject newBlackhole = Instantiate(_blackholePrefab, player.transform.position, Quaternion.identity);

        _ctrl = newBlackhole.GetComponent<Skill_Blackhole_Controller>();
        _ctrl.SetUpBlackhole(_maxSize, _growSpeed, _shrinkSpeed, _cloneAttackCount, _cloneAttackCoolDown, _blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();

        _blackholeUnlock.GetComponent<Button>().onClick.AddListener(BlackholeUnlock);
    }

    protected override void LoadUnlock ()
    {
        BlackholeUnlock();
    }

    void BlackholeUnlock ()
    {
        if (_blackholeUnlock.unlocked)
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
