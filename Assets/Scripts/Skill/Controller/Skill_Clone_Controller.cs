using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Skill_Clone_Controller : MonoBehaviour
{
    [SerializeField] private float cloneTimer;
    [SerializeField] private float exitSpeed = 1f;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = 0.7f;

    private Animator anim;
    private SpriteRenderer sr;
    
    private float cloneDuration;
    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * exitSpeed);
            if(sr.color.a < 0)
                Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// ��Ӱ��������
    /// </summary>
    /// <param name="targetPos">��Ӱλ��</param>
    /// <param name="cloneDuration">����ʱ��</param>
    /// <param name="canAttack">�ܷ񹥻�</param>
    public void SetClone(Transform targetPos, float cloneDuration, bool canAttack)
    {
        if(canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        }

        cloneTimer = cloneDuration;
        transform.position = targetPos.position;
        FaceToEnemy();
    }

    private void AnimTrigger()
    {

    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    /// <summary>
    /// �泯�������
    /// </summary>
    private void FaceToEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform target = transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        // ��ȡ���н�������
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                if(Vector2.Distance(hit.transform.position, transform.position) < minDistance)
                {
                    minDistance = Vector2.Distance(hit.transform.position, transform.position);
                    target = hit.transform;
                }
        }

        if (target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }
}
