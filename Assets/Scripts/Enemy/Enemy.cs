using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ����
/// </summary>
public class Enemy : Entity
{
    [Header("Stunned Info")]
    public float stunDuration = 1;
    public Vector2 stunDirection;
    [HideInInspector]public bool canBeStunned;
    [SerializeField] protected GameObject counterImage; 

    [Header("Move Info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1f;
    public float battleTime = 5f;

    [Header("Attack Info")]
    public float attackDistance = 2f;
    public float attackCoolDown = 0.5f;
    [HideInInspector] public float lastAttackTime = 0;
    [SerializeField] protected LayerMask playerLayer;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }

    protected override void Start()
    {
        base.Start();
        counterImage.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    /// <summary>
    /// �ɹ��ص�Animator�����ϵĽű�����
    /// </summary>
    public virtual void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    public bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCoolDown;
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    /// <summary>
    /// ���ڲ�ѯ�õ��˴˿��ܷ񱻵���
    /// </summary>
    /// <returns></returns>
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * faceDir, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, 5, playerLayer);

}
