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
    [SerializeField] protected GameObject counterImage; 
    [HideInInspector] public bool canBeStunned;

    [Header("Move Info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1f;
    public float battleTime = 5f;
    float _defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance = 2f;
    public float attackCoolDown = 0.5f;
    [SerializeField] protected LayerMask playerLayer;
    [HideInInspector] public float lastAttackTime = 0;

    public EnemyStateMachine stateMachine { get; private set; }

    public string lastAniName {  get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        _defaultMoveSpeed = moveSpeed;
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

    #region Speed Change

    public override void SlowEntitySpeed(float slowPercentage, float duration)
    {
        slowPercentage = Mathf.Clamp01(slowPercentage);
        moveSpeed *= (1 - slowPercentage);

        Invoke(nameof(ReturnDefaultSpeed), duration);
    }

    public override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = _defaultMoveSpeed;
    }

    public void FreezeTime(bool isFreeze)
    {
        if (isFreeze)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = _defaultMoveSpeed;
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
    /// �ɹ��ص�Animator�����ϵĽű�����
    /// </summary>
    public virtual void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    /// <summary>
    /// ��鹥����ȴ
    /// </summary>
    /// <returns></returns>
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

    #endregion

    /// <summary>
    /// ��¼��ǰ���� ��ʱ���ֲ���
    /// </summary>
    /// <param name="aniBoolName"></param>
    public virtual void AssignLastAniName(string aniBoolName)
    {
        lastAniName = aniBoolName;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, 5, playerLayer);
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * faceDir, transform.position.y));
    }
}
