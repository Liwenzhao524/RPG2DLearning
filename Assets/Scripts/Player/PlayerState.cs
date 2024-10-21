using UnityEngine;

/// <summary>
/// 玩家状态父类
/// </summary>
public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected float xinput;
    protected float yinput;
    string _aniBoolName;  // 动画状态对应变量名
    protected bool aniTrigger;  // 动画事件触发

    protected float stateTimer;  // 某些状态持续时间

    public PlayerState (Player player, PlayerStateMachine playerStateMachine, string aniBoolName)
    {
        this.player = player;
        stateMachine = playerStateMachine;
        _aniBoolName = aniBoolName;
    }

    public virtual void Enter ()
    {
        player.anim.SetBool(_aniBoolName, true);
        rb = player.rb;
        aniTrigger = false;
    }

    public virtual void Update ()
    {
        stateTimer -= Time.deltaTime;

        xinput = Input.GetAxisRaw("Horizontal");
        yinput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit ()
    {
        player.anim.SetBool(_aniBoolName, false);
    }

    public virtual void AnimFinishTrigger ()
    {
        aniTrigger = true;
    }
}
