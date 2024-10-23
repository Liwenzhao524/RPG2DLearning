using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Skill_Sword_Controller : Skill_Controller
{
    
    Collider2D _col;

    bool _canRotate = true;  // ��ײʱ ��ȷ���� ��������˵ķ���Ϊ���з���

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
    /// ���ڶ�ʱ����
    /// </summary>
    public void DestroySword()
    {
        Destroy(gameObject);
    }

    // �ⲿ���� ��ʼ��
    public void SetUpSword(Vector2 dir, float gravity, Player player, float freezeDuration, float returnSpeed)
    {
        base.player = player;
        rb.velocity = dir;
        rb.gravityScale = gravity;
        _freezeDuration = freezeDuration;
        _returnSpeed = returnSpeed;

        anim.SetBool("Rotate", true);

        _spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        _bounceTarget = new List<Transform>();  // Debug: public��[serializeField]���Զ�new��privateĬ�ϲ�new����Ҫ�ֶ� 

        Invoke(nameof(DestroySword), 7);
    }

    void FixedUpdate()
    {

        // ��֤������ķ��з��� ����Ŀ��
        if (_canRotate)
            transform.right = rb.velocity;

        // ������
        ReturnLogic();

        // ������
        BounceLogic();

        // ����ת
        SpinLogic();
    }
    
    #region Return
    /// <summary>
    /// ���� ���ջ�
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
            // ֱ���޸�����
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                                                     _returnSpeed * Time.deltaTime);

            // �����㹻Сʱ �����ս�״̬
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

                // �������Զ�����
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
    /// Ѱ�ҿ��Ե����Ŀ��
    /// </summary>
    /// <param name="collision"></param>
    private void SetTargetForBounce(Collider2D collision)
    {
        // ���е���ʱ������
        if (collision.GetComponent<Enemy>() != null)
        {

            if (_canBounce && _bounceTarget.Count <= 0)
            {
                // ��ȡ��Χ�ڵ��� ���뵯���б�
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
            // ��������δֹͣ
            if (!_isSpinStopped && Vector2.Distance(transform.position, player.transform.position) > _maxDistance)
            {
                SpinStop();
            }

            // ֹͣ����
            if (_isSpinStopped)
            {
                // ��תʱ����ǰ��
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y), 1 * Time.deltaTime);

                // ԭ����ת
                _spinTimer -= Time.deltaTime;
                if (_spinTimer < 0)
                {
                    _isReturn = true;
                    _canSpin = false;
                }

                // ��ʱ����˺�
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
    /// ͣ�²���ת
    /// </summary>
    private void SpinStop()
    {
        // ֹͣ ͬʱ��ת��ʱ
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
        
        // ���Ե���
        SetTargetForBounce(collision);

        // ���ܵ�����ǵ���
        StuckIn(collision);
    }

    /// <summary>
    /// ����������ײ������
    /// </summary>
    /// <param name="collision"></param>
    private void StuckIn(Collider2D collision)
    {
        // ��͸����
        if (_pierceCount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceCount--;
            return;
        }
        
        // ���е�һ�� ͣ�²���ת
        if (_canSpin)
        {
            SpinStop();  
            return;
        }

        _canRotate = false;
        _col.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_canBounce && _bounceTarget.Count > 0) return;  // �ܵ���

        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
