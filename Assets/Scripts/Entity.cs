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
    [SerializeField] protected Vector2 knockDirection;
    [SerializeField] protected float knockDuration;
    protected bool isKnocked;

    public float faceDir { get; private set; } = 1;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>(); 
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnock");
    }

    public IEnumerator HitKnock()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockDirection.x * -faceDir, knockDirection.y);
        yield return new WaitForSeconds(knockDuration);
        isKnocked = false;
    }

    #region Velocity
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
    protected virtual void Flip()
    {
        faceDir *= -1;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipController(float x)
    {
        if (x * faceDir < 0)
        {
            Flip();
        }
    }
    #endregion
}
