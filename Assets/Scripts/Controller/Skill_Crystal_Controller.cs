using UnityEngine;

public class Skill_Crystal_Controller : Skill_Controller
{
    CircleCollider2D _col;

    float _crystalTimer;

    bool _canGrow;
    float _growSpeed = 5;
    bool _canExplode;

    bool _canMove;
    float _moveSpeed;

    protected override void Awake()
    {
        base.Awake();
        _col = GetComponent<CircleCollider2D>();
    }

    public void SetUpCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed)
    {
        _crystalTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMove;
        _moveSpeed = moveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _crystalTimer -= Time.deltaTime;
        if (_crystalTimer < 0)
        {
            CrystalEnd();
        }

        // 增强爆炸视觉
        if (_canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), _growSpeed * Time.deltaTime);
        }

        MoveLogic();
    }
    
    /// <summary>
    /// 可移动水晶
    /// </summary>
    private void MoveLogic()
    {
        if (_canMove)
        {
            Transform target = FindClosestEnemy();
            transform.position = Vector2.MoveTowards(transform.position, target.position, _moveSpeed * Time.deltaTime);

            // 靠近时 执行爆炸或消失
            if (Vector2.Distance(transform.position, target.position) < _col.radius)
            {
                CrystalEnd();
                _canMove = false;  // 避免爆炸时滑动
            }
        }
    }

    /// <summary>
    /// 动画事件 触发爆炸伤害
    /// </summary>
    public void AnimExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _col.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicDamageTo(hit.GetComponent<CharacterStats>());
                SkillManager.instance.clone.CloneDuplicate(hit.transform);
            }
        }
    }

    /// <summary>
    /// 决定水晶消失方式
    /// </summary>
    public void CrystalEnd()
    {
        if (_canExplode)
        {
            _canGrow = true;
            anim.SetTrigger("Explode");
        }
        else SelfDestroy();
    }

    private void SelfDestroy() => Destroy(gameObject);

}
