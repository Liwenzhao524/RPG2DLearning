using System;
using UnityEngine;

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
    public Stats magicResistence;

    [Header("Offense Stats")]
    public Stats ATK;
    public Stats critChance;
    public Stats critDamage;

    [Header("Magic Stats")]
    public Stats fireATK;
    public Stats iceATK;
    public Stats lightningATK;

    float AilmentDuration = 4;
    bool isignited;  // DOT
    bool ischilled;  // ����
    bool isshocked;  // ������

    float ignitedTimer;
    float chillTimer;
    float shockTimer;

    float ignitedDamageTimer;
    readonly float ignitedDamageCoolDown = 0.3f;
    float ignitedDamage;

    float thunderDamage;

    public float currentHP;

    /// <summary>
    /// ����HP�仯ʱ����
    /// </summary>
    public Action HPChange;

    protected Entity _entity;
    protected EntityFX _fx;
    [SerializeField] GameObject _shockThunderPrefab;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _fx = GetComponent<EntityFX>();
        _entity = GetComponent<Entity>();
        critChance.SetDefaultValue(0.05f);
        critDamage.SetDefaultValue(0.5f);
        currentHP = GetMaxHP();  // �˴�����Ҫע��Start��ִ��˳�� ����Setting����
    }

    /// <summary>
    /// �������HP
    /// </summary>
    /// <returns></returns>
    public float GetMaxHP()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isignited = false;

        if (chillTimer < 0)
            ischilled = false;

        if (shockTimer < 0)
            isshocked = false;

        TakeIgniteDamage();
    }

    

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="target"></param>
    public virtual void DoDamageTo(CharacterStats target)
    {
        if (DoEvasion(target)) return;

        float totalDamage = ATK.GetValue() + strength.GetValue();

        totalDamage = DoCrit(totalDamage);
        totalDamage = DoDefence(target, totalDamage);

        target.TakeDamage(totalDamage);
    }


    /// <summary>
    /// ��ɷ���
    /// </summary>
    /// <param name="target"></param>
    public virtual void DoMagicDamageTo(CharacterStats target)
    {
        float _fireDamage = fireATK.GetValue();
        float _iceDamage = iceATK.GetValue();
        float _lightningDamage = lightningATK.GetValue();

        float totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicDamage = DoMagicResistance(totalMagicDamage);

        target.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0) return;

        TakeAilment(target, _fireDamage, _iceDamage, _lightningDamage);
    }

    #region Aliment

    /// <summary>
    /// ��ɸ���Ч��
    /// </summary>
    /// <param name="target">���ܶ���</param>
    /// <param name="_fireDamage">���ջ���</param>
    /// <param name="_iceDamage">���ձ���</param>
    /// <param name="_lightningDamage">��������</param>
    private void TakeAilment(CharacterStats target, float _fireDamage, float _iceDamage, float _lightningDamage)
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
    private void ChooseWhichAilment(float _fireDamage, float _iceDamage, float _lightningDamage, out bool fireAliment, out bool iceAliment, out bool lightningAliment)
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
    public void TakeWhichAilment(bool ignite, bool chill, bool shock)
    {
        // �в�ͬ���� ������
        //if (isshocked || isignited || ischilled) return;

        bool canGetIgnite = !isignited && !ischilled && !isshocked;
        bool canGetChill = !isignited && !ischilled && !isshocked;
        bool canGetShock = !isignited && !ischilled;

        if (ignite && canGetIgnite)
        {
            isignited = ignite;
            ignitedTimer = AilmentDuration;
            _fx.ChangeToIgniteFX(AilmentDuration);
        }

        if (chill && canGetChill)
        {
            ischilled = chill;
            chillTimer = AilmentDuration;
            _fx.ChangeToChillFX(AilmentDuration);

            float slowPersentage = 0.2f;
            _entity.SlowEntitySpeed(slowPersentage, AilmentDuration);
        }

        if (shock && canGetShock)
        {
            // ��Ŀ��
            if(!isshocked)
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
    public void ApplyShock(bool shock)
    {
        if (isshocked) return;
        isshocked = shock;
        shockTimer = AilmentDuration;
        _fx.ChangeToShockFX(AilmentDuration);
    }

    /// <summary>
    /// �Ը������˽�������
    /// </summary>
    private void ThunderOnNearEnemy()
    {
        GameObject thunder = Instantiate(_shockThunderPrefab, transform.position, Quaternion.identity);
        Thunder_Controller ctrl = thunder.GetComponent<Thunder_Controller>();
        Transform target = ctrl.FindClosestEnemy();

        ctrl.SetUp(thunderDamage, target.GetComponent<CharacterStats>());
    }

    public void SetIgniteDamage(float damage) => ignitedDamage = damage;
    public void SetThunderDamage(float damage) => thunderDamage = damage;

    #endregion

    #region Damage Calculation

    /// <summary>
    /// ���㷨��
    /// </summary>
    /// <param name="totalMagicDamage">����ǰ����</param>
    /// <returns>�������</returns>
    protected virtual float DoMagicResistance(float totalMagicDamage)
    {
        totalMagicDamage -= magicResistence.GetValue() + intelligence.GetValue() * 3;
        if (totalMagicDamage < 1) totalMagicDamage = 1;
        return totalMagicDamage;
    }

    /// <summary>
    /// ���㱩��
    /// </summary>
    /// <param name="totalDamage"></param>
    /// <returns></returns>
    protected virtual float DoCrit(float totalDamage)
    {
        float totalChance = critChance.GetValue() * 100 + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < totalChance)
        {
            totalDamage *= (1 + critDamage.GetValue() + 0.02f * strength.GetValue());
        }

        return totalDamage;
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="target">�ܻ�����</param>
    /// <param name="totalDamage">����ǰ�˺�</param>
    /// <returns>������˺�</returns>
    protected virtual float DoDefence(CharacterStats target, float totalDamage)
    {
        float oringinArmor = target.armor.GetValue();

        if (ischilled) target.armor.AddModifer(-oringinArmor * 0.2f);

        totalDamage -= target.armor.GetValue();

        if (ischilled) target.armor.RemoveModifer(-oringinArmor * 0.2f);

        if (totalDamage < 1) totalDamage = 1;
        return totalDamage;
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="target">�ܻ�����</param>
    /// <returns>�ܻ������ܷ�ر��˺�</returns>
    protected virtual bool DoEvasion(CharacterStats target)
    {
        if (isshocked) target.evasion.AddModifer(-20);

        float totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (isshocked) target.evasion.RemoveModifer(-20);

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    #endregion

    protected virtual void DecreaseHP(float damage)
    {
        currentHP -= damage;

        HPChange?.Invoke();
    }

    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        DecreaseHP(damage);

        if (currentHP <= 0)
        {
            Die();
        }

        _entity.DamageEffect();
    }

    /// <summary>
    /// ����ȼ��DOT
    /// </summary>
    private void TakeIgniteDamage()
    {
        if (ignitedDamageTimer < 0 && isignited)
        {
            ignitedDamageTimer = ignitedDamageCoolDown;

            DecreaseHP(ignitedDamage);

            if (currentHP < 0) 
                Die();
        }
    }

    protected virtual void Die()
    {
        isignited = false;
        ischilled = false;
        isshocked = false;
    }
}
