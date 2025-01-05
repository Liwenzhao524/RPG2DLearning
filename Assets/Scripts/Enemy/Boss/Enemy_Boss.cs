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
    public int castInterval = 2;
    public int castAmount = 3;
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

    }

    protected override void Update ()
    {
        base.Update();
    }

    public bool CanCast_CoolDown () => Time.time > lastCastTime + _castCooldown;
    
    public void CreateCast ()
    {
        Player player = PlayerManager.instance.player;
        Vector2 target = player.transform.position;
        Vector2 offset = new(player.rb.velocity.x * 0.5f + 0.2f, 1.5f);

        GameObject newCast = Instantiate(_castPrefab, new Vector2(target.x + offset.x, target.y + offset.y), Quaternion.identity);
        newCast.GetComponent<BossCast_Controller>().SetupCast(GetComponent<CharacterStats>());
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
            FindPosition();
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
