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
    /// 创建残影 有偏移量
    /// </summary>
    /// <param name="targetPos">创建位置</param>
    /// <param name="offset">位置偏移量</param>
    public void CreateClone(Transform targetPos, Vector3 offset)
    {
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
}
