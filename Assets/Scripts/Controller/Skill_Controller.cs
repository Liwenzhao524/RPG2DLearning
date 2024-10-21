using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Controller : MonoBehaviour
{
    protected Animator _anim;
    protected Rigidbody2D _rb;
    protected Player _player;

    protected virtual void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _player = PlayerManager._instance._player;
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// ��ȡ�������
    /// </summary>
    /// <returns>����Transform</returns>
    public virtual Transform FindClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform target = transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        // ��ȡ���н�������
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(hit.transform.position, transform.position) > 0.5f)
            {
                if (Vector2.Distance(hit.transform.position, transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(hit.transform.position, transform.position);
                    target = hit.transform;
                 
                }
            }
        }
        return target;  
    }
}
