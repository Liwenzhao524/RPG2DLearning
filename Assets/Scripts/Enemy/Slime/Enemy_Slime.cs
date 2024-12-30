
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType { Big, Medium, Small};

public class Enemy_Slime : Enemy
{
    [Header("Self-Multi After Die")]
    [SerializeField] GameObject _slimePrefab;
    [SerializeField] int _slimeAmount = 2;
    public SlimeType slimeType = SlimeType.Big;
    [SerializeField] Vector2 _maxInitVelocity;
    [SerializeField] Vector2 _minInitVelocity;


    protected override void Awake ()
    {
        base.Awake();
        idleState = new SlimeIdleState(this, stateMachine, "Idle");
        moveState = new SlimeMoveState(this, stateMachine, "Move");
        battleState = new SlimeBattleState(this, stateMachine, "Move");
        attackState = new SlimeAttackState(this, stateMachine, "Attack");
        stunState = new SlimeStunState(this, stateMachine, "Stun");
        deadState = new SlimeDeadState(this, stateMachine, "Idle");
    }

    protected override void Start ()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update ()
    {
        base.Update();
    }

    public override bool CanBeStunned ()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }

    public override void Die ()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        if (slimeType == SlimeType.Small) return;

        CreateSlime();

    }

    void CreateSlime ()
    {
        for(int i = 0; i < _slimeAmount; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);
            SetupSlime(newSlime.GetComponent<Enemy_Slime>());
        }
    }

    private void SetupSlime (Enemy_Slime newSlime)
    {
        float xVelocity = Random.Range(_minInitVelocity.x, _maxInitVelocity.x);
        float yVelocity = Random.Range(_minInitVelocity.y, _maxInitVelocity.y);

        // 此处设置速度不生效 原因不明
        Rigidbody2D rb = newSlime.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(xVelocity * -faceDir, yVelocity);

        isKnocked = true;
        Invoke(nameof(CancelKnock), 1.5f);
    }

    void CancelKnock () => isKnocked = false;

}
