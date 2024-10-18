using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stats strength;  // ¼Ó¹¥ ±©ÉË
    public Stats agility;   // ¼ÓÉÁ±Ü ±©»÷
    public Stats intelligence;  // ¼Ó·¨ÉË ·¨¿¹
    public Stats vitality;  // ¼ÓÑª

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

    bool isignited;
    bool ischilled;
    bool isshocked;

    [SerializeField] protected float currentHP;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        critChance.SetDefaultValue(0.05f);
        critDamage.SetDefaultValue(0.5f);
        currentHP = maxHP.GetValue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void DoDamageTo(CharacterStats target)
    {
        if (DoEvasion(target)) return;

        float totalDamage = ATK.GetValue() + strength.GetValue();

        totalDamage = DoCrit(totalDamage);
        totalDamage = DoDefence(target, totalDamage);

        target.TakeDamage(totalDamage);
    }

    public virtual void DoMagicDamageTo(CharacterStats target)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();

        float totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        
        totalMagicDamage = DoMagicResistance(totalMagicDamage);

        target.TakeDamage(totalMagicDamage);





    }

    protected virtual float DoMagicResistance(float totalMagicDamage)
    {
        totalMagicDamage -= magicResistence.GetValue() + intelligence.GetValue() * 3;
        if (totalMagicDamage < 1) totalMagicDamage = 1;
        return totalMagicDamage;
    }

    public void DoAilment(bool ignite, bool chill, bool shock)
    {
        if (isshocked || isignited || ischilled) return;

        ischilled = chill;
        isignited = ignite;
        isshocked = shock;
    }

    protected virtual float DoCrit(float totalDamage)
    {
        float totalChance = critChance.GetValue() * 100 + agility.GetValue();
        if(Random.Range(0, 100) < totalChance)
        {
            totalDamage *= (1 + critDamage.GetValue() + 0.02f * strength.GetValue());
        }
        
        return totalDamage;
    }

    protected virtual float DoDefence(CharacterStats target, float totalDamage)
    {
        totalDamage -= target.armor.GetValue();

        if (totalDamage < 1) totalDamage = 1;
        return totalDamage;
    }

    protected virtual bool DoEvasion(CharacterStats target)
    {
        float totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Evasion");
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

    }
}
