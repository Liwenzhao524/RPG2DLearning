using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;  // 加攻 暴伤
    public Stats agility;   // 加闪避 暴击
    public Stats intelligence;  // 加法伤 法抗
    public Stats vitality;  // 加血

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

    float _ailmentDuration = 4;
    bool _isignited;  // DOT
    bool _ischilled;  // 减防
    bool _isshocked;  // 减命中

    float _ignitedTimer;
    float _chillTimer;
    float _shockTimer;

    float _ignitedDamageTimer;
    readonly float _ignitedDamageCoolDown = 0.3f;
    float _ignitedDamage;

    float _thunderDamage;

    public float currentHP;

    /// <summary>
    /// 仅当HP变化时调用
    /// </summary>
    public Action HPChange;

    protected Entity entity;
    protected EntityFX fx;
    [SerializeField] GameObject shockThunderPrefab;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        entity = GetComponent<Entity>();
        critChance.SetDefaultValue(0.05f);
        critDamage.SetDefaultValue(0.5f);
        currentHP = GetMaxHP();  // 此处可能要注意Start的执行顺序 可在Setting调整
    }

    /// <summary>
    /// 返回最大HP
    /// </summary>
    /// <returns></returns>
    public float GetMaxHP()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }

    // Update is called once per frame
    protected virtual void Update()
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
    /// 造成物伤
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
    /// 造成法伤
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
    /// 造成负面效果
    /// </summary>
    /// <param name="target">承受对象</param>
    /// <param name="_fireDamage">最终火伤</param>
    /// <param name="_iceDamage">最终冰伤</param>
    /// <param name="_lightningDamage">最终雷伤</param>
    private void TakeAilment(CharacterStats target, float _fireDamage, float _iceDamage, float _lightningDamage)
    {
        // 选择一种效果
        bool fireAliment, iceAliment, lightningAliment;
        ChooseWhichAilment(_fireDamage, _iceDamage, _lightningDamage, out fireAliment, out iceAliment, out lightningAliment);

        if (fireAliment) target.SetIgniteDamage(_fireDamage * 0.2f);
        if (lightningAliment) target.SetThunderDamage(_lightningDamage * 0.2f);

        target.TakeWhichAilment(fireAliment, iceAliment, lightningAliment);
    }

    /// <summary>
    /// 根据伤害选择一种负面效果
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

        // 若有数值相同 则随机取一种
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
    /// 造成选定的负面效果
    /// </summary>
    /// <param name="ignite"></param>
    /// <param name="chill"></param>
    /// <param name="shock"></param>
    public void TakeWhichAilment(bool ignite, bool chill, bool shock)
    {
        // 有不同条件 不能用
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
            // 主目标
            if(!_isshocked)
            {
                ApplyShock(shock);
            }
            // 找另一目标给雷击
            else
            {
                if (GetComponent<Player>() != null) return;
                ThunderOnNearEnemy();
            }
        }
    }

    /// <summary>
    /// 同上面chill等 主要为了Thunder_Controller调用视觉效果
    /// </summary>
    /// <param name="shock"></param>
    public void ApplyShock(bool shock)
    {
        if (_isshocked) return;
        _isshocked = shock;
        _shockTimer = _ailmentDuration;
        fx.ChangeToShockFX(_ailmentDuration);
    }

    /// <summary>
    /// 对附近敌人降下落雷
    /// </summary>
    private void ThunderOnNearEnemy()
    {
        GameObject thunder = Instantiate(shockThunderPrefab, transform.position, Quaternion.identity);
        Thunder_Controller ctrl = thunder.GetComponent<Thunder_Controller>();
        Transform target = ctrl.FindClosestEnemy();

        ctrl.SetUp(_thunderDamage, target.GetComponent<CharacterStats>());
    }

    public void SetIgniteDamage(float damage) => _ignitedDamage = damage;
    public void SetThunderDamage(float damage) => _thunderDamage = damage;

    #endregion

    #region Damage Calculation

    /// <summary>
    /// 计算法抗
    /// </summary>
    /// <param name="totalMagicDamage">计算前法伤</param>
    /// <returns>计算后法伤</returns>
    protected virtual float DoMagicResistance(float totalMagicDamage)
    {
        totalMagicDamage -= magicResistence.GetValue() + intelligence.GetValue() * 3;
        if (totalMagicDamage < 1) totalMagicDamage = 1;
        return totalMagicDamage;
    }

    /// <summary>
    /// 计算暴击
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
    /// 计算防御
    /// </summary>
    /// <param name="target">受击对象</param>
    /// <param name="totalDamage">计算前伤害</param>
    /// <returns>计算后伤害</returns>
    protected virtual float DoDefence(CharacterStats target, float totalDamage)
    {
        float oringinArmor = target.armor.GetValue();

        if (_ischilled) target.armor.AddModifier(-oringinArmor * 0.2f);

        totalDamage -= target.armor.GetValue();

        if (_ischilled) target.armor.RemoveModifier(-oringinArmor * 0.2f);

        if (totalDamage < 1) totalDamage = 1;
        return totalDamage;
    }

    /// <summary>
    /// 计算闪避
    /// </summary>
    /// <param name="target">受击对象</param>
    /// <returns>受击对象能否回避伤害</returns>
    protected virtual bool DoEvasion(CharacterStats target)
    {
        if (_isshocked) target.evasion.AddModifier(-20);

        float totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (_isshocked) target.evasion.RemoveModifier(-20);

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
    /// 承受伤害
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        DecreaseHP(damage);

        if (currentHP <= 0)
        {
            Die();
        }

        entity.DamageEffect();
    }

    /// <summary>
    /// 承受燃烧DOT
    /// </summary>
    private void TakeIgniteDamage()
    {
        if (_ignitedDamageTimer < 0 && _isignited)
        {
            _ignitedDamageTimer = _ignitedDamageCoolDown;

            DecreaseHP(_ignitedDamage);

            if (currentHP < 0) 
                Die();
        }
    }

    protected virtual void Die()
    {
        _isignited = false;
        _ischilled = false;
        _isshocked = false;
    }
}
