using System.Collections;
using UnityEngine;

public class Player : Entity
{

    [Header("Attack Detail")]
    public Vector2[] attackMove;
    public float attackSpeed = 1;
    public float counterAttackDuration = 0.5f;

    [Header("Move Info")]
    public float moveSpeed = 5;
    public float jumpForce = 10;
    public float swordReturnForce = 3;

    [Header("Dash Info")]
    public float dashSpeed = 25;
    public float dashDuraTime = 0.2f;
    public float dashDir {  get; private set; }

    [HideInInspector]public bool isBusy;  // 占用符 锁滑步之类
    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }  // 飞剑技能 限制只有一把

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }


    public PlayerPrimeAttackState primeAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "WallJump");
        primeAttackState = new PlayerPrimeAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckDashInput();
    }

    /// <summary>
    /// 当前飞剑位 空则创建
    /// </summary>
    /// <param name="newSword"></param>
    public void AssignSword(GameObject newSword)
    {
        sword = newSword;
    }

    /// <summary>
    /// 改变状态 并销毁飞剑物体
    /// </summary>
    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    /// <summary>
    /// 死锁一定时间
    /// </summary>
    /// <param name="seconds">死锁秒数</param>
    /// <returns></returns>
    public IEnumerator BusyFor(float seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

    /// <summary>
    /// 由挂载到Animator物体上的脚本调用
    /// </summary>
    public virtual void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    /// <summary>
    /// 保证冲刺高优先级，否则应写在PlayerGroundState
    /// </summary>
    public void CheckDashInput()
    {
        if (IsWallDetected() && !IsGroundDetected()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {

            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0) dashDir = faceDir;

            stateMachine.ChangeState(dashState);
        }
    }
}
