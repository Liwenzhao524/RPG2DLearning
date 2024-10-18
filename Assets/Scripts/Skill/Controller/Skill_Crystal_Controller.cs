using UnityEngine;

public class Skill_Crystal_Controller : Skill_Controller
{
    CircleCollider2D _col;

    float crystalTimer;

    bool canGrow;
    float growSpeed = 5;
    bool canExplode;

    bool canMove;
    float moveSpeed;

    protected override void Awake()
    {
        base.Awake();
        _col = GetComponent<CircleCollider2D>();
    }

    public void SetUpCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed)
    {
        crystalTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        crystalTimer -= Time.deltaTime;
        if (crystalTimer < 0)
        {
            CrystalEnd();
        }

        // ��ǿ��ը�Ӿ�
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }

        MoveLogic();
    }
    
    /// <summary>
    /// ���ƶ�ˮ��
    /// </summary>
    private void MoveLogic()
    {
        if (canMove)
        {
            Transform target = FindClosestEnemy();
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // ����ʱ ִ�б�ը����ʧ
            if (Vector2.Distance(transform.position, target.position) < _col.radius)
            {
                CrystalEnd();
                canMove = false;  // ���ⱬըʱ����
            }
        }
    }

    /// <summary>
    /// �����¼� ������ը�˺�
    /// </summary>
    public void AnimExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _col.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
                SkillManager.instance.clone.CloneDuplicate(hit.transform);
            }
        }
    }

    /// <summary>
    /// ����ˮ����ʧ��ʽ
    /// </summary>
    public void CrystalEnd()
    {
        if (canExplode)
        {
            canGrow = true;
            _anim.SetTrigger("Explode");
        }
        else SelfDestroy();
    }

    private void SelfDestroy() => Destroy(gameObject);

}
