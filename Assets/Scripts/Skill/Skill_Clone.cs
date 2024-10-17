using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������²�Ӱ ����
/// </summary>
public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject newClonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack = true;

    /// <summary>
    /// ������Ӱ ��ƫ����
    /// </summary>
    /// <param name="targetPos">����λ��</param>
    /// <param name="offset">λ��ƫ����</param>
    public void CreateClone(Transform targetPos, Vector3 offset)
    {
        GameObject clone = Instantiate(newClonePrefab);
        clone.GetComponent<Skill_Clone_Controller>().SetClone(targetPos, cloneDuration, canAttack, offset);
    }

    /// <summary>
    /// ������Ӱ ��ƫ����
    /// </summary>
    /// <param name="targetPos"></param>
    public void CreateClone(Transform targetPos)
    {
        CreateClone(targetPos, new Vector3(0, 0, 0));
    }
}
