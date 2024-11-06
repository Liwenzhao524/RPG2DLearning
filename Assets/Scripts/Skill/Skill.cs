using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܸ���
/// </summary>
public class Skill : MonoBehaviour
{
    public float coolDown;
    protected float coolDownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;

        LoadUnlock();
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// ����ʱ��鼼���Ƿ����
    /// </summary>
    protected virtual void LoadUnlock ()
    {

    }

    /// <summary>
    /// �ܷ�ʹ�ü��ܣ���ȴ
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer > 0 || player.stateMachine.currentState == player.deadState)
        {
            return false;
        }

        UseSkill();
        coolDownTimer = coolDown;
        return true;
    }

    public virtual void UseSkill()
    {
        // ����ִ��
    }

    public virtual Transform FindClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform target = transform;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
        // ��ȡ���н�������
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
