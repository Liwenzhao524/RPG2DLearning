using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Skill_Dash dash {  get; private set; }
    public Skill_Clone clone { get; private set; }
    public Skill_Sword sword { get; private set; }
    public Skill_Blackhole blackhole { get; private set; }
    public Skill_Crystal crystal { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        dash = GetComponent<Skill_Dash>();
        clone = GetComponent<Skill_Clone>();
        sword = GetComponent<Skill_Sword>();
        blackhole = GetComponent<Skill_Blackhole>();
        crystal = GetComponent<Skill_Crystal>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
