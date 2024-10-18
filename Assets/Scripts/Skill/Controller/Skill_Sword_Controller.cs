using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : Skill_Controller
{
    
    Collider2D _col;
    Player _player;

    bool canRotate = true;  // 碰撞时 正确处理 剑插入敌人的方向为飞行方向

    float freezeDuration;

    [Header("Return Info")]
    bool isReturn;
    float returnSpeed;

    [Header("Bounce Info")]
    bool canBounce;
    int bounceCount;
    float bounceSpeed;
    List<Transform> bounceTarget;
    int targetIndex;
    
    [Header("Pierce Info")]
    int pierceCount;

    [Header("Spin Info")]
    bool canSpin;
    bool isSpinStopped;
    float maxDistance;

    float spinDuration;
    float spinTimer;

    float hitCoolDown;
    float hitTimer;

    float spinDirection;


    protected override void Awake()
    {
        base.Awake();
        _col = GetComponent<Collider2D>();

    }

    /// <summary>
    /// 用于定时销毁
    /// </summary>
    public void DestroySword()
    {
        Destroy(gameObject);
    }

    // 外部调用 初始化
    public void SetUpSword(Vector2 dir, float gravity, Player player, float _freezeDuration, float _returnSpeed)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravity;
        freezeDuration = _freezeDuration;
        returnSpeed = _returnSpeed;

        _anim.SetBool("Rotate", true);

        spinDirection = Mathf.Clamp(_rb.velocity.x, -1, 1);

        bounceTarget = new List<Transform>();  // Debug: public和[serializeField]会自动new，private默认不new，需要手动 

        Invoke("DestroySword", 7);
    }

    void FixedUpdate()
    {

        // 保证剑本体的飞行方向 朝向目标
        if (canRotate)
            transform.right = _rb.velocity;

        // 剑返回
        ReturnLogic();

        // 剑弹射
        BounceLogic();

        // 剑旋转
        SpinLogic();
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
    public void SetupBounce(bool _canBounce, int _bounceTime, float _bounceSpeed)
    {
        canBounce = _canBounce;
        bounceCount = _bounceTime;
        bounceSpeed = _bounceSpeed;
    }

    private void BounceLogic()
    {
        if (canBounce && bounceTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, bounceTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, bounceTarget[targetIndex].position) < 0.1f)
            {
                bounceTarget[targetIndex].GetComponent<Enemy>()?.DamageEffect();
                targetIndex++;
                if (targetIndex >= bounceTarget.Count)
                    targetIndex = 0;

                // 弹射完自动返回
                bounceCount--;
                if (bounceCount <= 0)
                {
                    canBounce = false;
                    isReturn = true;
                }
            }
        }
    }

    /// <summary>
    /// 寻找可以弹射的目标
    /// </summary>
    /// <param name="collision"></param>
    private void SetTargetForBounce(Collider2D collision)
    {
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
    }
    #endregion

    #region Pierce

    public void SetupPierce(int _pierceCount)
    {
        pierceCount = _pierceCount;
    }

    #endregion

    #region Spin

    public void SetupSpin(bool _canSpin, float _maxDistance, float _spinDuration, float _hitCoolDown)
    {
        canSpin = _canSpin;
        spinDuration = _spinDuration;
        maxDistance = _maxDistance;
        hitCoolDown = _hitCoolDown;
    }

    private void SpinLogic()
    {
        if (canSpin)
        {
            // 超距离且未停止
            if (!isSpinStopped && Vector2.Distance(transform.position, _player.transform.position) > maxDistance)
            {
                SpinStop();
            }

            // 停止过程
            if (isSpinStopped)
            {
                // 旋转时缓慢前移
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1 * Time.deltaTime);

                // 原地旋转
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isReturn = true;
                    canSpin = false;
                }

                // 定时造成伤害
                hitTimer -= Time.deltaTime;
                if(hitTimer < 0)
                {
                    hitTimer = hitCoolDown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
                    foreach (var hit in colliders)
                        hit.GetComponent<Enemy>()?.DamageEffect();
                }
            }
        }
    }

    /// <summary>
    /// 停下并旋转
    /// </summary>
    private void SpinStop()
    {
        // 停止 同时旋转计时
        isSpinStopped = true;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    #endregion

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturn) return;

        if (collision.GetComponent<Enemy>())
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.DamageEffect();
            enemy.StartCoroutine("FreezeTimerFor", freezeDuration);
        }
        
        SetTargetForBounce(collision);

        // 不能弹射或不是敌人
        StuckIn(collision);
    }

    /// <summary>
    /// 插在最终碰撞物体上
    /// </summary>
    /// <param name="collision"></param>
    private void StuckIn(Collider2D collision)
    {
        // 穿透敌人
        if (pierceCount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceCount--;
            return;
        }
        
        // 击中第一个 停下并旋转
        if (canSpin)
        {
            SpinStop();  
            return;
        }

        canRotate = false;
        _col.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (canBounce && bounceTarget.Count > 0) return;  // 能弹跳

        _anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
