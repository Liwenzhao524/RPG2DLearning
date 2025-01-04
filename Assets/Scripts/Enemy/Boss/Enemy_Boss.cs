using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("Surrounding Check")]
    [SerializeField] Collider2D _arena;
    [SerializeField] Vector2 _surroundingCheckDis;

    [Header("Teleport Info")]
    [SerializeField] float _defaultTeleportChance = 20;
    public float teleportChance { get; set; }

    [Header("Cast Info")]
    [SerializeField] GameObject _castPrefab;
    [SerializeField] float _castCooldown;
    [SerializeField] int _defaultAmountOfcast = 3;
    public int amountOfCast {  get; set; }
    public float lastCastTime { get; set; }

    public BossCastState castState {  get; private set; }
    public BossTeleportState teleportState { get; private set; }


    public override void Die ()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void Awake ()
    {
        base.Awake();
        idleState = new BossIdleState(this, stateMachine, "Idle");
        moveState = new BossMoveState(this, stateMachine, "Move");
        battleState = new BossBattleState(this, stateMachine, "Move");
        attackState = new BossAttackState(this, stateMachine, "Attack");
        deadState = new BossDeadState(this, stateMachine, "Idle");
        castState = new BossCastState(this, stateMachine, "Cast");
        teleportState = new BossTeleportState(this, stateMachine, "Teleport");
    }

    protected override void Start ()
    {
        base.Start();
        stateMachine.Init(idleState);
        teleportChance = _defaultTeleportChance;
        amountOfCast = _defaultAmountOfcast;

        _castPrefab.GetComponent<BossCast_Controller>().SetupCast(GetComponent<EnemyStats>());
    }

    protected override void Update ()
    {
        base.Update();
    }

    public bool CanCast ()
    {
        float castCD = Random.Range(_castCooldown * 0.8f, _castCooldown * 1.2f);
        return (Time.time > lastCastTime + castCD) && (amountOfCast > 0);
    }

    public bool CanTeleport ()
    {
        if(Random.Range(0,100) <= teleportChance)
        {
            teleportChance = _defaultTeleportChance;
            return true;
        }

        return false;
    }

    public void FindPosition ()
    {
        float x = Random.Range(_arena.bounds.min.x + 3, _arena.bounds.max.x - 3);
        float y = Random.Range(_arena.bounds.min.y + 3, _arena.bounds.max.y - 3);

        transform.position = new Vector2 (x, y);
        transform.position = new Vector2(transform.position.x, transform.position.y - GroundBelowCheck().distance + ( col.bounds.size.y / 2 ));

        if(!GroundBelowCheck() || SurroundingCheck())
        {
            FindPosition();
            Debug.Log("Find New Position");
        }
    }

    RaycastHit2D GroundBelowCheck () => Physics2D.Raycast(transform.position, Vector2.down, 100, GroundLayer);
    bool SurroundingCheck () => Physics2D.BoxCast(transform.position, _surroundingCheckDis, 0, Vector2.zero, 0, GroundLayer);

    protected override void OnDrawGizmos ()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelowCheck().distance));
        Gizmos.DrawWireCube(transform.position, _surroundingCheckDis);
    }

}
