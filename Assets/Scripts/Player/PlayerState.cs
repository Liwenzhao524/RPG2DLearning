using UnityEngine;

/// <summary>
/// ���״̬����
/// </summary>
public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected float xinput;
    protected float yinput;
    string _aniBoolName;  // ����״̬��Ӧ������
    protected bool aniTrigger;  // �����¼�����

    protected float stateTimer;  // ĳЩ״̬����ʱ��

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
