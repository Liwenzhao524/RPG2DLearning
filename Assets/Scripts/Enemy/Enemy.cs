using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敌人 基类
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
    float defaultMoveSpeed;

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
        defaultMoveSpeed = moveSpeed;
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

    #region FreezeTime
    public void FreezeTime(bool isFreeze)
    {
        if (isFreeze)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float time)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(time);
        FreezeTime(false);
    }

    #endregion

    /// <summary>
    /// 由挂载到Animator物体上的脚本调用
    /// </summary>
    public virtual void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    public bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCoolDown;
    }

    #region Counter Attack
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
    /// 用于查询该敌人此刻能否被弹反
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

    #endregion

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, 5, playerLayer);
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * faceDir, transform.position.y));
    }
}
