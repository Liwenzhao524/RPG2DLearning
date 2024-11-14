using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatsType
{
    strength,
    agility,
    intelligence,
    vitality,

    maxHP,
    armor,
    evasion,
    magicResist,

    ATK,
    critChance,
    critDamage,

    fireATK,
    iceATK,
    lightningATK
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;  // �ӹ� ����
    public Stats agility;   // ������ ����
    public Stats intelligence;  // �ӷ��� ����
    public Stats vitality;  // ��Ѫ

    [Header("Defence Stats")]
    public Stats maxHP;
    public Stats armor;
    public Stats evasion;
    public Stats magicResist;

    [Header("Offense Stats")]
    public Stats ATK;
    public Stats critChance;
    public Stats critDamage;

    [Header("Magic Stats")]
    public Stats fireATK;
    public Stats iceATK;
    public Stats lightningATK;

    float _ailmentDuration = 4;
    bool _isignited;  // DOT
    bool _ischilled;  // ����
    bool _isshocked;  // ������

    float _ignitedTimer;
    float _chillTimer;
    float _shockTimer;

    float _ignitedDamageTimer;
    readonly float _ignitedDamageCoolDown = 0.3f;
    float _ignitedDamage;

    float _thunderDamage;

    public float currentHP;
    public bool isDead { get; private set; }

    public bool isInvensible {  get; set; }

    /// <summary>
    /// ����HP�仯ʱ����
    /// </summary>
    public Action HPChange;

    protected Entity entity;
    protected EntityFX fx;
    [SerializeField] GameObject shockThunderPrefab;

    // Start is called before the first frame update
    protected virtual void Start ()
    {
        fx = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
        critChance.SetDefaultValue(0.05f);
        critDamage.SetDefaultValue(0.5f);
        currentHP = GetMaxHP();  // �˴�����Ҫע��Start��ִ��˳�� ����Setting����
    }

    #region Get Stats Influenced By Major Stats

    public float GetMaxHP () => maxHP.GetValue() + vitality.GetValue() * 5;

    public float GetATK () => ATK.GetValue() + strength.GetValue();

    public float GetMagicResist () => magicResist.GetValue() + intelligence.GetValue() * 3;

    public float GetCritChance () => critChance.GetValue() * 100 + agility.GetValue();

    public float GetCritDamage () => critDamage.GetValue() + 0.02f * strength.GetValue();

    public float GetEvasion () => evasion.GetValue() + agility.GetValue();

    public float GetMagicDamage () => fireATK.GetValue() + iceATK.GetValue() + lightningATK.GetValue() + intelligence.GetValue();

    #endregion

    // Update is called once per frame
    protected virtual void Update ()
    {
        _ignitedTimer -= Time.deltaTime;
        _chillTimer -= Time.deltaTime;
        _shockTimer -= Time.deltaTime;
        _ignitedDamageTimer -= Time.deltaTime;

        if (_ignitedTimer < 0)
            _isignited = false;

        if (_chillTimer < 0)
            _ischilled = false;

        if (_shockTimer < 0)
            _isshocked = false;

        TakeIgniteDamage();
    }

    /// <summary>
    /// ���ⲿͨ��enum���ַ�����ȡ��ɫStats
    /// �Ա����ⲿ��ʾ���޸�
    /// </summary>
    /// <param name="statsType"></param>
    /// <returns></returns>
    public Stats GetStatsByType (StatsType statsType)
    {
        Dictionary<StatsType, Stats> _statsMap = new()
        {
            { StatsType.strength, strength },
            { StatsType.agility, agility },
            { StatsType.intelligence, intelligence },
            { StatsType.vitality, vitality },
            { StatsType.maxHP, maxHP },
            { StatsType.armor, armor },
            { StatsType.evasion, evasion },
            { StatsType.magicResist, magicResist },
            { StatsType.ATK, ATK },
            { StatsType.critChance, critChance },
            { StatsType.critDamage, critDamage },
            { StatsType.fireATK, fireATK },
            { StatsType.iceATK, iceATK },
            { StatsType.lightningATK, lightningATK }
        };

        if (_statsMap.TryGetValue(statsType, out Stats statsByType))
        {
            return statsByType;
        }
        else
        {
            Debug.Log("No Such Stats");
            return null;
        }
    }

    public virtual void AddBuffToStats (Stats statsToBuff, float modifier, float duration)
    {
        StartCoroutine(StatsBuff(statsToBuff, modifier, duration));
    }

    IEnumerator StatsBuff (Stats statsToBuff, float modifier, float duration)
    {
        statsToBuff.AddModifier(modifier);
        yield return new WaitForSeconds(duration);
        statsToBuff.RemoveModifier(modifier);
    }

    #region Aliment

    /// <summary>
    /// ��ɸ���Ч��
    /// </summary>
    /// <param name="target">���ܶ���</param>
    /// <param name="_fireDamage">���ջ���</param>
    /// <param name="_iceDamage">���ձ���</param>
    /// <param name="_lightningDamage">��������</param>
    private void TakeAilment (CharacterStats target, float _fireDamage, float _iceDamage, float _lightningDamage)
    {
        // ѡ��һ��Ч��
        bool fireAliment, iceAliment, lightningAliment;
        ChooseWhichAilment(_fireDamage, _iceDamage, _lightningDamage, out fireAliment, out iceAliment, out lightningAliment);

        if (fireAliment) target.SetIgniteDamage(_fireDamage * 0.2f);
        if (lightningAliment) target.SetThunderDamage(_lightningDamage * 0.2f);

        target.TakeWhichAilment(fireAliment, iceAliment, lightningAliment);
    }

    /// <summary>
    /// �����˺�ѡ��һ�ָ���Ч��
    /// </summary>
    /// <param name="_fireDamage"></param>
    /// <param name="_iceDamage"></param>
    /// <param name="_lightningDamage"></param>
    /// <param name="fireAliment"></param>
    /// <param name="iceAliment"></param>
    /// <param name="lightningAliment"></param>
    private void ChooseWhichAilment (float _fireDamage, float _iceDamage, float _lightningDamage, out bool fireAliment, out bool iceAliment, out bool lightningAliment)
    {
        fireAliment = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        iceAliment = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        lightningAliment = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        // ������ֵ��ͬ �����ȡһ��
        while (!fireAliment && !iceAliment && !lightningAliment)
        {
            if (UnityEngine.Random.value < 0.3f && _fireDamage > 0)
            {
                fireAliment = true;
                continue;
            }
            if (UnityEngine.Random.value < 0.5f && _iceDamage > 0)
            {
                iceAliment = true;
                continue;
            }
            if (UnityEngine.Random.value < 0.7f && _lightningDamage > 0)
            {
                lightningAliment = true;
                continue;
            }
        }
    }

    /// <summary>
    /// ���ѡ���ĸ���Ч��
    /// </summary>
    /// <param name="ignite"></param>
    /// <param name="chill"></param>
    /// <param name="shock"></param>
    public void TakeWhichAilment (bool ignite, bool chill, bool shock)
    {
        // �в�ͬ���� ������
        //if (_isshocked || _isignited || _ischilled) return;

        bool canGetIgnite = !_isignited && !_ischilled && !_isshocked;
        bool canGetChill = !_isignited && !_ischilled && !_isshocked;
        bool canGetShock = !_isignited && !_ischilled;

        if (ignite && canGetIgnite)
        {
            _isignited = ignite;
            _ignitedTimer = _ailmentDuration;
            fx.ChangeToIgniteFX(_ailmentDuration);
        }

        if (chill && canGetChill)
        {
            _ischilled = chill;
            _chillTimer = _ailmentDuration;
            fx.ChangeToChillFX(_ailmentDuration);

            float slowPersentage = 0.2f;
            entity.SlowEntitySpeed(slowPersentage, _ailmentDuration);
        }

        if (shock && canGetShock)
        {
            // ��Ŀ��
            if (!_isshocked)
            {
                ApplyShock(shock);
            }
            // ����һĿ����׻�
            else
            {
                if (GetComponent<Player>() != null) return;
                ThunderOnNearEnemy();
            }
        }
    }

    /// <summary>
    /// ͬ����chill�� ��ҪΪ��Thunder_Controller�����Ӿ�Ч��
    /// </summary>
    /// <param name="shock"></param>
    public void ApplyShock (bool shock)
    {
        if (_isshocked) return;
        _isshocked = shock;
        _shockTimer = _ailmentDuration;
        fx.ChangeToShockFX(_ailmentDuration);
    }

    /// <summary>
    /// �Ը������˽�������
    /// </summary>
    private void ThunderOnNearEnemy ()
    {
        GameObject thunder = Instantiate(shockThunderPrefab, transform.position, Quaternion.identity);
        Thunder_Controller ctrl = thunder.GetComponent<Thunder_Controller>();
        Transform target = ctrl.FindClosestEnemy();

        // debug: FindClosestEnemy������ û�е��˻᷵���׻���transform
        if (target == thunder.transform)
            target = transform;

        ctrl.SetUp(_thunderDamage, target.GetComponent<CharacterStats>());
    }

    public void SetIgniteDamage (float damage) => _ignitedDamage = damage;
    public void SetThunderDamage (float damage) => _thunderDamage = damage;

    #endregion

    #region Damage Calculation

    /// <summary>
    /// ���㷨��
    /// </summary>
    /// <param name="totalMagicDamage">����ǰ����</param>
    /// <returns>�������</returns>
    protected virtual float DoMagicResistance (float totalMagicDamage)
    {
        totalMagicDamage -= GetMagicResist();
        if (totalMagicDamage < 1) totalMagicDamage = 1;
        return totalMagicDamage;
    }

    /// <summary>
    /// ���㱩��
    /// </summary>
    /// <param name="totalDamage"></param>
    /// <returns></returns>
    protected virtual float DoCrit (float totalDamage)
    {
        float totalChance = GetCritChance();
        if (UnityEngine.Random.Range(0, 100) < totalChance)
        {
            totalDamage *= ( 1 + GetCritDamage() );
        }

        return totalDamage;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="target">�ܻ�����</param>
    /// <param name="totalDamage">����ǰ�˺�</param>
    /// <returns>������˺�</returns>
    protected virtual float DoDefence (CharacterStats target, float totalDamage)
    {
        float oringinArmor = target.armor.GetValue();

        if (_ischilled) target.armor.AddModifier(-oringinArmor * 0.2f);

        totalDamage -= target.armor.GetValue();

        if (_ischilled) target.armor.RemoveModifier(-oringinArmor * 0.2f);

        if (totalDamage < 1) totalDamage = 1;
        return totalDamage;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="target">�ܻ�����</param>
    /// <returns>�ܻ������ܷ�ر��˺�</returns>
    protected virtual bool DoEvasion (CharacterStats target)
    {
        if (_isshocked) target.evasion.AddModifier(-20);

        float totalEvasion = target.GetEvasion();

        if (_isshocked) target.evasion.RemoveModifier(-20);

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            if (target.GetComponent<Player>() != null)
            {
                PlayerManager.instance.player.skill.dodge.DodgeClone();
            }
            return true;
        }
        return false;
    }

    #endregion

    #region Damage And HPChange

    public virtual void IncreaseHP (float hp)
    {
        currentHP += hp;

        currentHP = Mathf.Clamp(currentHP, 0, GetMaxHP());

        HPChange?.Invoke();
    }

    protected virtual void DecreaseHP (float damage)
    {
        currentHP -= damage;

        HPChange?.Invoke();
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="target"></param>
    public virtual void DoDamageTo (CharacterStats target)
    {
        if (target.isInvensible || DoEvasion(target)) return;

        float totalDamage = GetATK();

        totalDamage = DoCrit(totalDamage);
        totalDamage = DoDefence(target, totalDamage);

        target.GetComponent<Entity>().SetKnockDirection(transform);
        target.TakeDamage(totalDamage);

        // ���ݸ���ϲ�� �Ƿ�ͬʱ���ӷ���
        //DoMagicDamageTo(target);
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="target"></param>
    public virtual void DoDamageTo (CharacterStats target, float cloneATK = 1)
    {
        if (target.isInvensible || DoEvasion(target)) return;

        float totalDamage = GetATK() * cloneATK;

        totalDamage = DoCrit(totalDamage);
        totalDamage = DoDefence(target, totalDamage);

        target.TakeDamage(totalDamage);
    }

    /// <summary>
    /// ��ɷ���
    /// </summary>
    /// <param name="target"></param>
    public virtual void DoMagicDamageTo (CharacterStats target)
    {
        if(target.isInvensible) return;

        float _fireDamage = fireATK.GetValue();
        float _iceDamage = iceATK.GetValue();
        float _lightningDamage = lightningATK.GetValue();

        float totalMagicDamage = GetMagicDamage();

        totalMagicDamage = DoMagicResistance(totalMagicDamage);

        target.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) return;

        TakeAilment(target, _fireDamage, _iceDamage, _lightningDamage);
    }

    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage (float damage)
    {
        DecreaseHP(damage);

        if (currentHP <= 0 && !isDead)
        {
            Die();
        }

        entity.DamageEffect();
    }

    /// <summary>
    /// ����ȼ��DOT
    /// </summary>
    private void TakeIgniteDamage ()
    {
        if (isInvensible) return;

        if (_ignitedDamageTimer < 0 && _isignited)
        {
            _ignitedDamageTimer = _ignitedDamageCoolDown;

            DecreaseHP(_ignitedDamage);

            if (currentHP <= 0 && !isDead)
                Die();
        }
    }

    #endregion

    protected virtual void Die ()
    {
        if (isDead) return;
        isDead = true;
        _isignited = false;
        _ischilled = false;
        _isshocked = false;
    }

    public virtual void KillEntity ()
    {
        if (!isDead)
            Die();
    }

}
