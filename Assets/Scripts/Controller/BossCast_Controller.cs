using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCast_Controller : MonoBehaviour
{
    [SerializeField] Transform _check;
    [SerializeField] Vector2 _boxSize;
    [SerializeField] LayerMask _playerLayer;

    CharacterStats _stats;

    public void SetupCast(CharacterStats stats) => _stats = stats;

    public void AnimTrigger ()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(_check.position, _boxSize, _playerLayer);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Entity>().SetKnockDirection(transform);
                _stats.DoDamageTo(hit.GetComponent<CharacterStats>());
            }
        }
    }

    public void SelfDestroy() => Destroy(gameObject);

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireCube(_check.position, _boxSize);
    }

}
