using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

/// <summary>
/// 冲刺留下残影 技能
/// </summary>
public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject newClonePrefab;
    [SerializeField] float cloneDuration;
    [SerializeField] bool canAttack;

    [Header("Skill Branch Info")]
    [SerializeField] bool canCloneDashStart;
    [SerializeField] bool canCloneDashEnd;
    [SerializeField] bool canCloneCounterAttack;
    [SerializeField] bool canCloneCrystalMirage;

    [SerializeField] bool canDuplicate;
    [SerializeField] float duplicateChance;
    [SerializeField] bool crystalInsteadClone;  

    /// <summary>
    /// 创建残影 有偏移量
    /// </summary>
    /// <param name="targetPos">创建位置</param>
    /// <param name="offset">位置偏移量</param>
    public void CreateClone(Transform targetPos, Vector3 offset)
    {
        if (crystalInsteadClone)
        {
            SkillManager.instance.crystal.CreateCrystal(targetPos);
            return;
        }
        GameObject clone = Instantiate(newClonePrefab);
        clone.GetComponent<Skill_Clone_Controller>().SetClone(targetPos, cloneDuration, canAttack, offset);
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
        if(canCloneDashStart)
        {
            CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// 冲刺结束位置
    /// </summary>
    public void CloneDashEnd()
    {
        if (canCloneDashEnd)
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
        if (canCloneCounterAttack)
        {
            StartCoroutine(CreateCloneDelay(targetPos, new Vector3(1.5f * player.faceDir, 0), 0.4f));
        }
    }

    /// <summary>
    /// 水晶换位
    /// </summary>
    public void CloneCrystalMirage()
    {
        if(canCloneCrystalMirage)
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
        if (canDuplicate)
        {
            duplicateChance = Mathf.Clamp(duplicateChance, 0, 100);
            if (Random.Range(0, 100) < duplicateChance)
            {
                SkillManager.instance.clone.CreateClone(targetPos.transform, new Vector3(1.5f * faceDir, 0));
            }
        }
    }

    #endregion

    public IEnumerator CreateCloneDelay(Transform targetPos, Vector3 offset, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(targetPos, offset);
    }
}
