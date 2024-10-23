using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Effect", menuName = "Data/Item effect/Thunder")]
public class Effect_Thunder : ItemEffect
{
    [SerializeField] GameObject thunderPrefab;

    public override void ExecuteEffect ()
    {
        Player player = PlayerManager.instance.player;
        GameObject newThunder = Instantiate(thunderPrefab, player.transform.position + 
                                                           new Vector3(player.faceDir * 2, 0), Quaternion.identity);

        Destroy(newThunder, 1);
    }
}
