using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���е���ʱ����һ���׻�
/// </summary>
[CreateAssetMenu(fileName = "Thunder Effect", menuName = "Data/Item Effect/Thunder")]
public class Effect_Thunder : ItemEffect
{
    [SerializeField] GameObject thunderPrefab;

    public override void ExecuteEffect (Transform target)
    {
        base.ExecuteEffect (target);
        GameObject newThunder = Instantiate(thunderPrefab, target.position, Quaternion.identity);

        Destroy(newThunder, 1);
    }
}
