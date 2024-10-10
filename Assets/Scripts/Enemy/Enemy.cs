using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask playerLayer;

    [Header("Move Info")]
    public float moveSpeed = 1.5f;
    public float idleTime = 1f;
    public float battleTime = 5f;

    [Header("Attack Info")]
    public float attackDistance = 2f;
    public float attackCoolDown = 0.5f;
    [HideInInspector] public float lastAttackTime = 0;

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    /// <summary>
    /// 由挂载到Animator物体上的脚本调用
    /// </summary>
    public virtual void AnimTrigger() => stateMachine.currentState.AnimFinishTrigger();

    public bool CanAttack()
    {
        return Time.time > lastAttackTime + attackCoolDown;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * faceDir, transform.position.y));
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, 5, playerLayer);
}
