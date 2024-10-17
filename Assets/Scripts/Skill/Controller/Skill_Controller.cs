using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Controller : MonoBehaviour
{
    protected Animator _anim;
    protected Rigidbody2D _rb;

    protected virtual void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual Transform FindClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform target = transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        // 获取所有近处敌人
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if (Vector2.Distance(hit.transform.position, transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(hit.transform.position, transform.position);
                    target = hit.transform;
                }
        }
        return target;
    }
}
