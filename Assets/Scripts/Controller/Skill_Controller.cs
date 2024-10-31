using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Controller : MonoBehaviour
{
    protected Animator anim => GetComponentInChildren<Animator>();
    protected Rigidbody2D rb => GetComponent<Rigidbody2D>();
    protected Player player;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// 获取最近敌人
    /// </summary>
    /// <returns></returns>
    public virtual Transform FindClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform target = transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        // 获取所有近处敌人
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

    protected virtual void OnTriggerEnter2D (Collider2D collision)
    {
        
    }
}
