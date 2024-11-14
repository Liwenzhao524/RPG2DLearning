using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDis = 0.33f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDis = 0.33f;
    [SerializeField] protected LayerMask GroundLayer;

    [Header("Knockdown Info")]
    [SerializeField] protected Vector2 knockDistance;
    [SerializeField] protected float knockDuration;
    protected bool isKnocked;
    int _knockDir = 1;

    public float faceDir { get; private set; } = 1;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D col { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; protected set; }
    #endregion

    /// <summary>
    /// 仅在翻转时调用
    /// </summary>
    public Action OnFlip;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        fx = GetComponent<EntityFX>(); 
        stats = GetComponent<CharacterStats>();
    }
    
    protected virtual void Update()
    {

    }

    public virtual void SlowEntitySpeed(float slowPercentage, float duration)
    {

    }

    public virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageEffect()
    {
        fx.StartCoroutine(fx.FlashFX());
        StartCoroutine(HitKnock());
    }

    public virtual void SetKnockDirection(Transform dmgFrom)
    {
        if (dmgFrom.position.x < transform.position.x)
            _knockDir = 1;
        else
            _knockDir = -1;
    }

    public virtual void SetKnockDistance(Vector2 distance)
    {

    }

    /// <summary>
    /// 受击硬直击退
    /// </summary>
    /// <returns></returns>
    public IEnumerator HitKnock()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockDistance.x * _knockDir, knockDistance.y);
        yield return new WaitForSeconds(knockDuration);
        isKnocked = false;
    }

    #region Velocity

   /// <summary>
   /// 设置速度 包含翻转控制
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
    public virtual void SetVelocity(float x, float y)
    {
        if (isKnocked) return;
        rb.velocity = new Vector2(x, y);
        FlipController(x);
    }

    public virtual void SetZeroVelocity()
    {
        SetVelocity(0, 0);
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, GroundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDis, GroundLayer);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
                        new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDis * faceDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion

    #region Flip

    /// <summary>
    /// 翻转180
    /// </summary>
    protected virtual void Flip()
    {
        faceDir *= -1;
        transform.Rotate(0, 180, 0);

        OnFlip?.Invoke();
    }

    /// <summary>
    /// 根据传入的值与当前面朝向 决定是否翻转
    /// </summary>
    /// <param name="x"></param>
    public virtual void FlipController(float x)
    {
        if (x * faceDir < 0)
        {
            Flip();
        }
    }
    #endregion

    public virtual void Die()
    {

    }

}
