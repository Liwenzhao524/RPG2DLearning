using UnityEngine;

public class Skill_Clone_Controller : Skill_Controller
{
    SpriteRenderer _sr;
    
    [SerializeField] float cloneTimer;
    [SerializeField] float exitSpeed = 1f;

    [SerializeField] Transform attackCheck;
    [SerializeField] float attackRadius = 0.7f;

    int _faceDir = 1;
    float _cloneATK = 0.3f;

    protected override void Start ()
    {
        base.Start();
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
    /// 残影具体设置
    /// </summary>
    /// <param name="targetPos">残影位置</param>
    /// <param name="cloneDuration">持续时间</param>
    /// <param name="canAttack">能否攻击</param>
    /// <param name="offset">位置偏移</param>
    public void SetClone(Transform targetPos, float cloneDuration, bool canAttack, Vector3 offset)
    {
        if (canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        }

        cloneTimer = cloneDuration;
        transform.position = targetPos.position + offset;
        FaceToEnemy();
    }

    /// <summary>
    /// 残影具体设置 无位置偏移
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
        AudioManager.instance.PlaySFX("PlayerPrimeAttack");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (player.skill.clone.beAggresive) 
                { 
                    _cloneATK = 0.8f;
                    Inventory.instance.GetEquipmentByType(EquipmentType.Weapon)?.ExecuteEffects(hit.transform);
                }

                player.stats.DoDamageTo(hit.GetComponent<CharacterStats>(), _cloneATK);
                player.skill.clone.CloneDuplicate(hit.transform, _faceDir);
            }
        }
    }

    /// <summary>
    /// 面朝最近敌人
    /// </summary>
    private void FaceToEnemy()
    {
        Transform target = FindClosestEnemy();
        
        if (target.position.x < transform.position.x)
        {
            _faceDir *= -1;
            transform.Rotate(0, 180, 0);
        }
    }
}
