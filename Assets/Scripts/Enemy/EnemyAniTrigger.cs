using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private Enemy _enemy => GetComponentInParent<Enemy>();

    private void AniTrigger()
    {
        _enemy.AnimTrigger();
    } 
}
