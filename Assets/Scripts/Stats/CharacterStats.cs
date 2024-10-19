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
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;

    bool isignited;  // DOT
    bool ischilled;  // ����
    bool isshocked;  // ������

    float ignitedTimer;
    float chillTimer;
    float shockTimer;

    float ignitedDamageTimer;
    float ignitedDamageCoolDown = 0.3f;
    float ignitedDamage;

    public float currentHP;
    public Action HPChange;

    EntityFX _fx;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _fx = GetComponent<EntityFX>();
        critChance.SetDefaultValue(0.05f);
        critDamage.SetDefaultValue(0.5f);
        currentHP = GetMaxHP();
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

        if (ignitedDamageTimer < 0 && isignited)
        {
            ignitedDamageTimer = ignitedDamageCoolDown;

            DecreaseHP(ignitedDamage);

            if (currentHP < 0) Die();
        }
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
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();

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
        bool fireAliment = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool iceAliment = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool lightningAliment = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

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

        if (fireAliment) target.SetIgniteDamage(fireDamage.GetValue() * 0.2f);
        target.TakeWhichAilment(fireAliment, iceAliment, lightningAliment);
    }


    public void TakeWhichAilment(bool ignite, bool chill, bool shock)
    {
        if (isshocked || isignited || ischilled) return;

        if (ignite)
        {
            ignitedTimer = 3;
            _fx.ChangeToIgniteFX(ignitedTimer);
        }

        if (chill)
        {
            chillTimer = 10;
            _fx.ChangeToChillFX(chillTimer);
        }

        if (shock)
        {
            shockTimer = 10;
            _fx.ChangeToShockFX(shockTimer);
        }

        ischilled = chill;
        isignited = ignite;
        isshocked = shock;
    }

    public void SetIgniteDamage(float damage) => ignitedDamage = damage;

    #endregion

    #region Damage Calculation

    /// <summary>
    /// ���㷨��
    /// </summary>
    /// <param name="totalMagicDamage">����ǰ����</param>
    /// <returns></returns>
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
    /// <returns></returns>
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


    public virtual void TakeDamage(float damage)
    {
        DecreaseHP(damage);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void DecreaseHP(float damage)
    {
        currentHP -= damage;

        HPChange?.Invoke();
    }

    protected virtual void Die()
    {

    }

    public float GetMaxHP()
    {
        return maxHP.GetValue() + vitality.GetValue() * 5;
    }
}
