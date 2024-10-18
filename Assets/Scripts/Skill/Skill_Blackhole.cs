using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Blackhole : Skill
{
    [SerializeField] GameObject blackholePrefab;
    [Header("Basic Info")]
    [SerializeField] float blackholeDuration;
    [SerializeField] float maxSize;
    [SerializeField] float growSpeed;
    [SerializeField] float shrinkSpeed;

    [Header("Attack Info")]
    [SerializeField] int cloneAttackCount;
    [SerializeField] float cloneAttackCoolDown;

    Skill_Blackhole_Controller ctrl;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, _player.transform.position, Quaternion.identity);

        ctrl = newBlackhole.GetComponent<Skill_Blackhole_Controller>();
        ctrl.SetUpBlackhole(maxSize, growSpeed, shrinkSpeed, cloneAttackCount, cloneAttackCoolDown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeEnd()
    {
        if (!ctrl) return false;

        if(ctrl.playerCanExitSkill)
        {
            ctrl = null;
            return true;
        }

        return false;
    }
}
