using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 5;
    public float jumpForce = 10;

    [Header("Dash Info")]
    [SerializeField] private float dashCoolDown = 1;
    private float dashAbleTimer;
    public float dashSpeed = 25;
    public float dashDuraTime = 0.2f;
    public float dashDir {  get; private set; }
    

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDis = 0.33f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDis = 0.33f;
    [SerializeField] private LayerMask GroundLayer;

    public float faceDir { get; private set; } = 1;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpstate jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }


    public PlayerPrimeAttackState primeAttackState { get; private set; }
    #endregion

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpstate(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        primeAttackState = new PlayerPrimeAttackState(this, stateMachine, "Attack");
    }


    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Init(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
        CheckDashInput();
    }

    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
        FlipController(x);
    }

    public void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    /// <summary>
    /// 保证冲刺高优先级，否则应写在PlayerGroundState
    /// </summary>
    public void CheckDashInput()
    {
        if (IsWallDetected() && !IsGroundDetected()) return;
        dashAbleTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashAbleTimer < 0)
        {
            dashAbleTimer = dashCoolDown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0) dashDir = faceDir;

            stateMachine.ChangeState(dashState);
        }
            
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDis, GroundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDis, GroundLayer);

    private void Flip()
    {
        faceDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float x)
    {
        if(x * faceDir < 0)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, 
                        new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDis));
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDis * faceDir, wallCheck.position.y));
    }
}
