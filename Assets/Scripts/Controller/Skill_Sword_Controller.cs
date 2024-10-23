using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_Sword_Controller : Skill_Controller
{
    
    Collider2D _col;

    bool _canRotate = true;  // 碰撞时 正确处理 剑插入敌人的方向为飞行方向

    float _freezeDuration;

    [Header("Return Info")]
    bool _isReturn;
    float _returnSpeed;

    [Header("Bounce Info")]
    bool _canBounce;
    int _bounceCount;
    float _bounceSpeed;
    List<Transform> _bounceTarget;
    int _targetIndex;
    
    [Header("Pierce Info")]
    int _pierceCount;

    [Header("Spin Info")]
    bool _canSpin;
    bool _isSpinStopped;
    float _maxDistance;

    float _spinDuration;
    float _spinTimer;

    float _hitCoolDown;
    float _hitTimer;

    float _spinDirection;

    protected override void Start ()
    {
        base.Start();
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
    public void SetUpSword(Vector2 dir, float gravity, Player player, float freezeDuration, float returnSpeed)
    {
        base.player = player;
        rb.velocity = dir;
        rb.gravityScale = gravity;
        _freezeDuration = freezeDuration;
        _returnSpeed = returnSpeed;

        anim.SetBool("Rotate", true);

        _spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        _bounceTarget = new List<Transform>();  // Debug: public和[serializeField]会自动new，private默认不new，需要手动 

        Invoke(nameof(DestroySword), 7);
    }

    void FixedUpdate()
    {

        // 保证剑本体的飞行方向 朝向目标
        if (_canRotate)
            transform.right = rb.velocity;

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
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isReturn = true;
    }

    private void ReturnLogic()
    {
        if (_isReturn)
        {
            // 直接修改坐标
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                                                     _returnSpeed * Time.deltaTime);

            // 距离足够小时 进入收剑状态
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchSword();
        }
    }

    #endregion

    #region Bounce
    public void SetupBounce(bool canBounce, int bounceTime, float bounceSpeed)
    {
        _canBounce = canBounce;
        _bounceCount = bounceTime;
        _bounceSpeed = bounceSpeed;
    }

    private void BounceLogic()
    {
        if (_canBounce && _bounceTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _bounceTarget[_targetIndex].position, _bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, _bounceTarget[_targetIndex].position) < 0.1f)
            {
                if(_bounceTarget[_targetIndex].GetComponent<Enemy>()){
                    player.stats.DoDamageTo(_bounceTarget[_targetIndex].GetComponent<CharacterStats>());
                }
                _targetIndex++;
                if (_targetIndex >= _bounceTarget.Count)
                    _targetIndex = 0;

                // 弹射完自动返回
                _bounceCount--;
                if (_bounceCount <= 0)
                {
                    _canBounce = false;
                    _isReturn = true;
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

            if (_canBounce && _bounceTarget.Count <= 0)
            {
                // 获取范围内敌人 加入弹射列表
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        _bounceTarget.Add(hit.transform);
                }
            }
        }
        else _canBounce = false;
    }
    #endregion

    #region Pierce

    public void SetupPierce(int pierceCount)
    {
        _pierceCount = pierceCount;
    }

    #endregion

    #region Spin

    public void SetupSpin(bool canSpin, float maxDistance, float spinDuration, float hitCoolDown)
    {
        _canSpin = canSpin;
        _spinDuration = spinDuration;
        _maxDistance = maxDistance;
        _hitCoolDown = hitCoolDown;
    }

    private void SpinLogic()
    {
        if (_canSpin)
        {
            // 超距离且未停止
            if (!_isSpinStopped && Vector2.Distance(transform.position, player.transform.position) > _maxDistance)
            {
                SpinStop();
            }

            // 停止过程
            if (_isSpinStopped)
            {
                // 旋转时缓慢前移
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y), 1 * Time.deltaTime);

                // 原地旋转
                _spinTimer -= Time.deltaTime;
                if (_spinTimer < 0)
                {
                    _isReturn = true;
                    _canSpin = false;
                }

                // 定时造成伤害
                _hitTimer -= Time.deltaTime;
                if(_hitTimer < 0)
                {
                    _hitTimer = _hitCoolDown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
                    foreach (var hit in colliders)
                        if(hit.GetComponent<Enemy>())
                        {
                            player.stats.DoMagicDamageTo(hit.GetComponent<CharacterStats>());
                        }
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
        _isSpinStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }

    #endregion

    #region Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturn) return;

        if (collision.GetComponent<Enemy>())
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.StartCoroutine("FreezeTimerFor", _freezeDuration);
            player.stats.DoDamageTo(collision.GetComponent<CharacterStats>());
        }
        
        // 可以弹射
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
        if (_pierceCount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceCount--;
            return;
        }
        
        // 击中第一个 停下并旋转
        if (_canSpin)
        {
            SpinStop();  
            return;
        }

        _canRotate = false;
        _col.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_canBounce && _bounceTarget.Count > 0) return;  // 能弹跳

        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
