using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 冲刺留下残影 技能
/// </summary>
public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject newClonePrefab;
    [SerializeField] float cloneDuration;

    [Header("Attack")]
    bool _canAttack;
    [SerializeField] UI_SkillTreeSlot attackUnlock;

    [Header("Skill Branch Info")]
    [Header("Clone Dash Start")]
    bool _canCloneDashStart;
    [SerializeField] UI_SkillTreeSlot cloneDashStartUnlock;

    [Header("Clone Dash End")]
    bool _canCloneDashEnd;
    [SerializeField] UI_SkillTreeSlot cloneDashEndUnlock;

    [Header("Clone Counter Attack")]
    bool _canCloneCounterAttack;
    [SerializeField] UI_SkillTreeSlot cloneCounterAttackUnlock;

    [Header("Clone Crystal Blink")]
    bool _canCloneCrystalBlink;
    [SerializeField] UI_SkillTreeSlot cloneCrystalMirageUnlock;

    [Header("Clone Duplicate")]
    bool _canDuplicate;
    [SerializeField] UI_SkillTreeSlot duplicateUnlock;
    [SerializeField] float duplicateChance;

    [Header("Crystal Instead Clone")]
    bool _canCrystalInsteadClone;
    [SerializeField] UI_SkillTreeSlot crystalInsteadCloneUnlock;

    protected override void Start ()
    {
        base.Start();

        attackUnlock.GetComponent<Button>().onClick.AddListener(AttackUnlock);
        cloneDashStartUnlock.GetComponent<Button>().onClick.AddListener(CloneDashStartUnlock);
        cloneDashEndUnlock.GetComponent<Button>().onClick.AddListener (CloneDashEndUnlock);
        cloneCounterAttackUnlock.GetComponent<Button>().onClick.AddListener(CloneCounterAttackUnlock);
        cloneCrystalMirageUnlock.GetComponent<Button>().onClick.AddListener(CloneCrystalMirageUnlock);
        duplicateUnlock.GetComponent<Button>().onClick.AddListener(DuplicateUnlock);
        crystalInsteadCloneUnlock.GetComponent<Button>().onClick.AddListener(CrystalInsteadCloneUnlock);

    }

    /// <summary>
    /// 创建残影 有偏移量
    /// </summary>
    /// <param name="targetPos">创建位置</param>
    /// <param name="offset">位置偏移量</param>
    public void CreateClone(Transform targetPos, Vector3 offset)
    {
        if (_canCrystalInsteadClone)
        {
            SkillManager.instance.crystal.CreateCrystal(targetPos);
            return;
        }
        GameObject clone = Instantiate(newClonePrefab);
        clone.GetComponent<Skill_Clone_Controller>().SetClone(targetPos, cloneDuration, _canAttack, offset);
    }

    /// <summary>
    /// 创建残影 无偏移量
    /// </summary>
    /// <param name="targetPos"></param>
    public void CreateClone(Transform targetPos)
    {
        CreateClone(targetPos, new Vector3(0, 0, 0));
    }

    #region Skill Branch

    /// <summary>
    /// 冲刺起始位置
    /// </summary>
    public void CloneDashStart()
    {
        if(_canCloneDashStart)
        {
            CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// 冲刺结束位置
    /// </summary>
    public void CloneDashEnd()
    {
        if (_canCloneDashEnd)
        {
            CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// 反击成功
    /// </summary>
    /// <param name="targetPos"></param>
    public void CloneCounterAttack(Transform targetPos)
    {
        if (_canCloneCounterAttack)
        {
            StartCoroutine(CreateCloneDelay(targetPos, new Vector3(1.5f * player.faceDir, 0), 0.4f));
        }
    }

    /// <summary>
    /// 换位时在原位置生成crystal
    /// </summary>
    public void CloneCrystalBlink()
    {
        if(_canCloneCrystalBlink)
        {
            CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// 残影可以复制残影
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="faceDir"></param>
    public void CloneDuplicate(Transform targetPos, int faceDir = 1)
    {
        if (_canDuplicate)
        {
            duplicateChance = Mathf.Clamp(duplicateChance, 0, 100);
            if (Random.Range(0, 100) < duplicateChance)
            {
                SkillManager.instance.clone.CreateClone(targetPos.transform, new Vector3(1.5f * faceDir, 0));
            }
        }
    }

    #endregion

    void AttackUnlock ()
    {
        if (attackUnlock.unlocked)
            _canAttack = true;
    }

    void CloneDashStartUnlock ()
    {
        if (cloneDashStartUnlock.unlocked)
            _canCloneDashStart = true;
    }

    void CloneDashEndUnlock ()
    {
        if(cloneDashEndUnlock.unlocked)
            _canCloneDashEnd = true;
    }

    void CloneCounterAttackUnlock ()
    {
        if (cloneCounterAttackUnlock.unlocked)
            _canCloneCounterAttack = true;
    }

    void CloneCrystalMirageUnlock ()
    {
        if(cloneCrystalMirageUnlock.unlocked)
            _canCloneCrystalBlink = true;
    }

    void DuplicateUnlock ()
    {
        if(duplicateUnlock.unlocked)
            _canDuplicate = true;
    }

    void CrystalInsteadCloneUnlock ()
    {
        if(crystalInsteadCloneUnlock.unlocked)
            _canCrystalInsteadClone = true;
    }

    public IEnumerator CreateCloneDelay(Transform targetPos, Vector3 offset, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(targetPos, offset);
    }
}
