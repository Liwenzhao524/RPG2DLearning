using UnityEngine;

public class Skill_Clone_Controller : Skill_Controller
{
    [SerializeField] float cloneTimer;
    [SerializeField] float exitSpeed = 1f;

    [SerializeField] Transform attackCheck;
    [SerializeField] float attackRadius = 0.7f;

    SpriteRenderer _sr;

    private float cloneDuration;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            _sr.color = new Color(1, 1, 1, _sr.color.a - Time.deltaTime * exitSpeed);
            if (_sr.color.a < 0)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// ��Ӱ��������
    /// </summary>
    /// <param name="targetPos">��Ӱλ��</param>
    /// <param name="cloneDuration">����ʱ��</param>
    /// <param name="canAttack">�ܷ񹥻�</param>
    /// <param name="offset">λ��ƫ��</param>
    public void SetClone(Transform targetPos, float cloneDuration, bool canAttack, Vector3 offset)
    {
        if (canAttack)
        {
            _anim.SetInteger("AttackNum", Random.Range(1, 4));
        }

        cloneTimer = cloneDuration;
        transform.position = targetPos.position + offset;
        FaceToEnemy();
    }

    /// <summary>
    /// ��Ӱ�������� ��λ��ƫ��
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="cloneDuration"></param>
    /// <param name="canAttack"></param>
    public void SetClone(Transform targetPos, float cloneDuration, bool canAttack)
    {
        SetClone(targetPos, cloneDuration, canAttack, new Vector3(0, 0, 0));
    }

    private void AnimTrigger()
    {

    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    /// <summary>
    /// �泯�������
    /// </summary>
    private void FaceToEnemy()
    {
        Transform target = FindClosestEnemy();
        
        if (target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }
}
