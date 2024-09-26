using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDis = 0.33f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDis = 0.33f;
    [SerializeField] protected LayerMask GroundLayer;

    public float faceDir { get; private set; } = 1;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    #region Velocity
    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        FlipController(x);
    }

    public virtual void SetZeroVelocity() => rb.velocity = Vector2.zero;
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, GroundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDis, GroundLayer);

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
                        new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDis * faceDir, wallCheck.position.y));
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
