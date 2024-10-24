using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在第三击触发 创建一道冲击波
/// </summary>
[CreateAssetMenu(fileName = "Ice And Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class Effect_IceAndFire : ItemEffect
{
    [SerializeField] GameObject icefirePrefab;
    [SerializeField] float xVelocity;

    public override void ExecuteEffect (Transform target)
    {
        base.ExecuteEffect (target);

        if(player.primeAttackState.comboCounter == 2 )
        {
            GameObject newEffect = Instantiate(icefirePrefab, target.position, player.transform.rotation);

            newEffect.GetComponent<Rigidbody2D>().velocity = new Vector2( xVelocity * player.faceDir, 0);

            Destroy(newEffect, 5);
        }

    }
}
