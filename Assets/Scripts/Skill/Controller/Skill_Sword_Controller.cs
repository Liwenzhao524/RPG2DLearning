using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : Skill_Controller
{
    
    Collider2D _col;
    Player _player;

    bool canRotate = true;  // ��ײʱ ��ȷ���� ��������˵ķ���Ϊ���з���

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
    /// ���ڶ�ʱ����
    /// </summary>
    public void DestroySword()
    {
        Destroy(gameObject);
    }

    // �ⲿ���� ��ʼ��
    public void SetUpSword(Vector2 dir, float gravity, Player player, float _freezeDuration, float _returnSpeed)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravity;
        freezeDuration = _freezeDuration;
        returnSpeed = _returnSpeed;

        _anim.SetBool("Rotate", true);

        spinDirection = Mathf.Clamp(_rb.velocity.x, -1, 1);

        bounceTarget = new List<Transform>();  // Debug: public��[serializeField]���Զ�new��privateĬ�ϲ�new����Ҫ�ֶ� 

        Invoke("DestroySword", 7);
    }

    void FixedUpdate()
    {

        // ��֤������ķ��з��� ����Ŀ��
        if (canRotate)
            transform.right = _rb.velocity;

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
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturn = true;
    }

    private void ReturnLogic()
    {
        if (isReturn)
        {
            // ֱ���޸�����
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                                                     returnSpeed * Time.deltaTime);

            // �����㹻Сʱ �����ս�״̬
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

                // �������Զ�����
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
    /// Ѱ�ҿ��Ե����Ŀ��
    /// </summary>
    /// <param name="collision"></param>
    private void SetTargetForBounce(Collider2D collision)
    {
        // ���е���ʱ������
        if (collision.GetComponent<Enemy>() != null)
        {

            if (canBounce && bounceTarget.Count <= 0)
            {
                // ��ȡ��Χ�ڵ��� ���뵯���б�
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
            // ��������δֹͣ
            if (!isSpinStopped && Vector2.Distance(transform.position, _player.transform.position) > maxDistance)
            {
                SpinStop();
            }

            // ֹͣ����
            if (isSpinStopped)
            {
                // ��תʱ����ǰ��
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1 * Time.deltaTime);

                // ԭ����ת
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isReturn = true;
                    canSpin = false;
                }

                // ��ʱ����˺�
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
    /// ͣ�²���ת
    /// </summary>
    private void SpinStop()
    {
        // ֹͣ ͬʱ��ת��ʱ
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
        if (pierceCount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceCount--;
            return;
        }
        
        // ���е�һ�� ͣ�²���ת
        if (canSpin)
        {
            SpinStop();  
            return;
        }

        canRotate = false;
        _col.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (canBounce && bounceTarget.Count > 0) return;  // �ܵ���

        _anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
