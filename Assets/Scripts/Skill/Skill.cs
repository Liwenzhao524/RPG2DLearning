using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ܸ���
/// </summary>
public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float coolDownTimer;
    protected Player _player;

    protected virtual void Start()
    {
        _player = PlayerManager._instance._player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// �ܷ�ʹ�ü��ܣ���ȴ
    /// </summary>
    /// <returns></returns>
    public virtual bool CanUseSkill()
    {
        if(coolDownTimer > 0 || _player.stateMachine.currentState == _player.deadState)
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
