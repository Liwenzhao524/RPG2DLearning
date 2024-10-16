using System.Collections.Generic;
using UnityEngine;

public class Skill_Sword_Controller : MonoBehaviour
{
    Animator _anim;
    Rigidbody2D _rb;
    Collider2D _col;
    Player _player;

    bool canRotate = true;  // ��ײʱ ��ȷ���� ��������˵ķ���Ϊ���з���

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

    // �ⲿ���� ��ʼ��
    public void SetUpSword(Vector2 dir, float gravity, Player player)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravity;

        _anim.SetBool("Rotate", true);

        bounceTarget = new List<Transform>();  // Debug: public��[serializeField]���Զ�new��privateĬ�ϲ�new����Ҫ�ֶ� 
    }

    private void FixedUpdate()
    {
        // ��֤������ķ��з��� ����Ŀ��
        if (canRotate)
            transform.right = _rb.velocity;

        // ������
        ReturnLogic();

        // ������
        BounceLogic();
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

                // �������Զ�����
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

        // ���ܵ�����ǵ���
        StuckIn(collision);
    }

    /// <summary>
    /// ������ײ������
    /// </summary>
    /// <param name="collision"></param>
    private void StuckIn(Collider2D collision)
    {
        canRotate = false;
        _col.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (canBounce) return;  // �ܵ���

        _anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }

    #endregion

}
