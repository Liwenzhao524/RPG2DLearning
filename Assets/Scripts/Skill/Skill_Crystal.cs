using System.Collections.Generic;
using UnityEngine;

public class Skill_Crystal : Skill
{
    GameObject _currentCrystal;
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration;

    [Header("Explode Info")]
    [SerializeField] bool canExplode;

    [Header("Move Info")]
    [SerializeField] bool canMove;
    [SerializeField] float moveSpeed;

    [Header("Muti Info")]
    [SerializeField] bool canUseMuti;
    [SerializeField] float useSkillWindow = 5;
    [SerializeField] int crystalCount = 3;
    [SerializeField] float mutiCoolDown = 3;
    List<GameObject> _crystalList = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (canUseMuti)
        {
            if (_crystalList.Count <= 0) FillCrystalList();
            UseMutiCrystal();
            return;
        }

        SingleLogic();
    }

    /// <summary>
    /// 创建并设置水晶
    /// </summary>
    public void CreateCrystal(Transform targetPos)
    {
        _currentCrystal = Instantiate(crystalPrefab, targetPos.position, Quaternion.identity);
        Skill_Crystal_Controller ctrl = _currentCrystal.GetComponent<Skill_Crystal_Controller>();
        ctrl.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed);
    }

    private void SingleLogic()
    {
        if (_currentCrystal == null)
        {
            CreateCrystal(player.transform);
        }
        // 再按下 互换位置
        else
        {
            //if (_canMove) return;
            if (Vector2.Distance(_currentCrystal.transform.position, player.transform.position) < 25)
            {
                Vector2 playerPos = player.transform.position;

                player.skill.clone.CloneCrystalMirage();  // 技能分支
                player.transform.position = _currentCrystal.transform.position;
                _currentCrystal.transform.position = playerPos;

                _currentCrystal.GetComponent<Skill_Crystal_Controller>().CrystalEnd();
                _currentCrystal = null;
            }
        }
    }
    
    

    private void UseMutiCrystal()
    {
        if (_crystalList.Count > 0)
        {
            // 重置时间窗
            if (_crystalList.Count == crystalCount)
                Invoke(nameof(ResetAbility), useSkillWindow);

            coolDown = 0;
            GameObject chosen = _crystalList[_crystalList.Count - 1];
            GameObject newCrystal = Instantiate(chosen, player.transform.position, Quaternion.identity);
            _crystalList.Remove(chosen);

            Skill_Crystal_Controller ctrl = newCrystal.GetComponent<Skill_Crystal_Controller>();
            ctrl.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed);
        }
        // 冷却再装填
        if (_crystalList.Count <= 0)
        {
            coolDown = mutiCoolDown;
            FillCrystalList();
        }
    }

    private void FillCrystalList()
    {
        _crystalList.Clear();
        for (int i = 0; i < crystalCount; i++)
        {
            _crystalList.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (coolDownTimer <= 0)
        {
            coolDownTimer = mutiCoolDown;
            FillCrystalList();
        }
    }
}
