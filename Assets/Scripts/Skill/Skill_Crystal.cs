using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Crystal : Skill
{
    GameObject currentCrystal;
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
    List<GameObject> crystalList = new List<GameObject>();
    
    public override void UseSkill()
    {
        base.UseSkill();

        if (canUseMuti)
        {
            if(crystalList.Count <= 0) FillCrystalList();
            UseMutiCrystal();
            return;
        }

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, _player.transform.position, Quaternion.identity);
            Skill_Crystal_Controller ctrl = currentCrystal.GetComponent<Skill_Crystal_Controller>();
            ctrl.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed);
        }  
        else
        {
            //if (canMove) return;
            if(Vector2.Distance(currentCrystal.transform.position, _player.transform.position) < 25)
            {
                Vector2 playerPos = _player.transform.position;

                _player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPos;

                currentCrystal.GetComponent<Skill_Crystal_Controller>().CrystalEnd();
                currentCrystal = null;
            }
        }
    }

    private void UseMutiCrystal()
    {
        if (crystalList.Count > 0)
        {
            if (crystalList.Count == crystalCount)
                Invoke("ResetAbility", useSkillWindow);

            coolDown = 0;
            GameObject chosen = crystalList[crystalList.Count - 1];
            GameObject newCrystal = Instantiate(chosen, _player.transform.position, Quaternion.identity);
            crystalList.Remove(chosen);

            Skill_Crystal_Controller ctrl = newCrystal.GetComponent<Skill_Crystal_Controller>();
            ctrl.SetUpCrystal(crystalDuration, canExplode, canMove, moveSpeed);
        }
        if (crystalList.Count <= 0)
        {
            coolDown = mutiCoolDown;
            FillCrystalList();
        }
    }

    private void FillCrystalList()
    {
        crystalList.Clear();
        for (int i = 0; i < crystalCount; i++)
        {
            crystalList.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(coolDownTimer <= 0)
        {
            coolDownTimer = mutiCoolDown;
            FillCrystalList();
        }
    }
}
