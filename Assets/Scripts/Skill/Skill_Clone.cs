using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冲刺留下残影 技能
/// </summary>
public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject newClonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack = true;
    
    /// <summary>
    /// 创建残影
    /// </summary>
    /// <param name="targetPos"></param>
    public void CreateClone(Transform targetPos)
    {
        GameObject clone = Instantiate(newClonePrefab);
        clone.GetComponent<Skill_Clone_Controller>().SetClone(targetPos, cloneDuration, canAttack);
    }
}
