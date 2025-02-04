using UnityEngine;

public class SkillManager : SingletonMono<SkillManager>
{
    public Skill_Dash dash { get; private set; }
    public bool dashUse {  get; set; }
    public Skill_Clone clone { get; private set; }
    public Skill_Sword sword { get; private set; }
    public Skill_Blackhole blackhole { get; private set; }
    public bool blackholeUse { get; set; }
    public Skill_Crystal crystal { get; private set; }
    public bool crystalUse { get; set; }
    public Skill_CounterAttack parry { get; private set; }
    public bool parryUse { get; set; }
    public Skill_Dodge dodge { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        dash = GetComponent<Skill_Dash>();
        clone = GetComponent<Skill_Clone>();
        sword = GetComponent<Skill_Sword>();
        blackhole = GetComponent<Skill_Blackhole>();
        crystal = GetComponent<Skill_Crystal>();
        parry = GetComponent<Skill_CounterAttack>();
        dodge = GetComponent<Skill_Dodge>();
    }
}
