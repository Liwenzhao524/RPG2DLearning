using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb;
    Collider2D _col;
    Player _player;

    bool canRotate = true;  // 碰撞时 正确处理 剑插入敌人的方向为飞行方向

    [Header("Return Info")]
    bool isReturn;
    [SerializeField] private float returnSpeed = 20;

    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed = 15;
    bool canBounce;
    private int bounceTime;
    private List<Transform> bounceTarget;
    private int targetIndex;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>(); 
    }

    // 外部调用 初始化
    public void SetUpSword(Vector2 dir, float gravity, Player player)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravity;

        _anim.SetBool("Rotate", true);

        bounceTarget = new List<Transform>();  // Debug: public和[serializeField]会自动new，private默认不new，需要手动 
    }

    private void FixedUpdate()
    {
        // 保证剑本体的飞行方向 朝向目标
        if (canRotate)
            transform.right = _rb.velocity;

        // 剑返回
        ReturnLogic();

        // 剑弹射
        BounceLogic();
    }

    #region Return
    /// <summary>
    /// 触发 剑收回
    /// </summary>
    public void ReturnSword()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturn = true;
    }

    private void ReturnLogic()
    {
        if (isReturn)
        {
            // 直接修改坐标
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                                                     returnSpeed * Time.deltaTime);

            // 距离足够小时 进入收剑状态
            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
                _player.CatchSword();
        }
    }

    #endregion

    #region Bounce
    public void SetupBounce(bool _canBounce, int _bounceTime, float _bounceGravityScale = 1)
    {
        canBounce = _canBounce;
        bounceTime = _bounceTime;
        _rb.gravityScale *= _bounceGravityScale;
    }

    private void BounceLogic()
    {
        if (canBounce && bounceTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, bounceTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, bounceTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                if (targetIndex >= bounceTarget.Count)
                    targetIndex = 0;

                // 弹射完自动返回
                bounceTime--;
                if (bounceTime <= 0)
                {
                    canBounce = false;
                    isReturn = true;
                }
            }
        }
    }

    #endregion

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn) return;

        // 击中敌人时允许弹射
        if (collision.GetComponent<Enemy>() != null)
        {
            
            if (canBounce && bounceTarget.Count <= 0)
            {
                // 获取范围内敌人 加入弹射列表
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        bounceTarget.Add(hit.transform);
                }
            }
        }
        else canBounce = false;

        // 不能弹射或不是敌人
        StuckIn(collision);
    }

    /// <summary>
    /// 插在碰撞物体上
    /// </summary>
    /// <param name="collision"></param>
    private void StuckIn(Collider2D collision)
    {
        canRotate = false;
        _col.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (canBounce) return;  // 能弹跳

        _anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
