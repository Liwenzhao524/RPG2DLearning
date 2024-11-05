using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Skill_CounterAttack : Skill
{
    [Header("Basic Info")]
    [SerializeField] UI_SkillTreeSlot _parryUnlock;
    public bool canParry {  get; private set; }

    [Header("Restore")]
    [SerializeField] UI_SkillTreeSlot _restoreUnlock;
    bool _canRestore;

    [Header("Clone")]
    [SerializeField] UI_SkillTreeSlot _parryCloneUnlock;
    bool _canParryClone;


    protected override void Start()
    {
        base.Start();

        _parryUnlock.GetComponent<Button>().onClick.AddListener(ParryUnlock);
        _restoreUnlock.GetComponent<Button>().onClick.AddListener(RestoreUnlock);
        _parryCloneUnlock.GetComponent<Button>().onClick.AddListener(ParryCloneUnlock);
    }

    // Update is called once per frame
    protected override void Update()
    {
       base.Update();
    }

    public override void UseSkill ()
    {
        base.UseSkill();
        SkillManager.instance.parryUse = true;
    }

    public void ParryLogic ()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius * 1.2f);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    player.anim.SetBool("SuccessfulCA", true);
                    ParryRestore();
                    ParryClone(hit.transform);
                }
        }
    }

    public void ParryRestore ()
    {
        if (_canRestore)
        {
            player.stats.IncreaseHP(player.stats.GetMaxHP() * 0.05f);
        }
    }

    public void ParryClone (Transform target)
    {
        if (_canParryClone)
        {
            StartCoroutine(player.skill.clone.CreateCloneDelay(target, new Vector3(1.5f * player.faceDir, 0), 0.2f));
        }
    }

    void ParryUnlock ()
    {
        if(_parryUnlock.unlocked)
            canParry = true;
    }

    void RestoreUnlock ()
    {
        if(_restoreUnlock.unlocked)
            _canRestore = true;
    }

    void ParryCloneUnlock ()
    {
        if( _parryCloneUnlock.unlocked)
            _canParryClone = true;
    }

}
