using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Crystal : Skill
{
    GameObject _currentCrystal;

    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration;

    [Header("Crystal Unlock")]
    bool _canUseCrystal;
    [SerializeField] UI_SkillTreeSlot crystalUnlock;

    [Header("Explode Unlock")]
    bool _canExplode;
    [SerializeField] UI_SkillTreeSlot explodeUnlock;

    [Header("Move Unlock")]
    bool _canMove;
    [SerializeField] UI_SkillTreeSlot moveUnlock;
    float _moveSpeed = 5;

    [Header("Multi Unlock")]
    bool _canUseMulti;
    [SerializeField] UI_SkillTreeSlot multiUnlock;

    float _useSkillWindow = 5;
    int _crystalCount = 3;
    float _multiCoolDown = 3;
    List<GameObject> _crystalList = new();

    protected override void Start ()
    {
        base.Start();

        crystalUnlock.GetComponent<Button>().onClick.AddListener(CrystalUnlock);
        explodeUnlock.GetComponent<Button>().onClick.AddListener(ExplodeUnlock);
        moveUnlock.GetComponent<Button>().onClick.AddListener (MoveUnlock);
        multiUnlock.GetComponent<Button>().onClick.AddListener(MultiUnlock);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (_canUseMulti)
        {
            if (_crystalList.Count <= 0) FillCrystalList();
            UseMutiCrystal();
            return;
        }

        if(_canUseCrystal)
            SingleLogic();
    }

    /// <summary>
    /// ����������ˮ��
    /// </summary>
    public void CreateCrystal(Transform targetPos)
    {
        _currentCrystal = Instantiate(crystalPrefab, targetPos.position, Quaternion.identity);
        Skill_Crystal_Controller ctrl = _currentCrystal.GetComponent<Skill_Crystal_Controller>();
        ctrl.SetUpCrystal(crystalDuration, _canExplode, _canMove, _moveSpeed);
    }

    private void SingleLogic()
    {
        if (_currentCrystal == null)
        {
            CreateCrystal(player.transform);
        }
        // �ٰ��� ����λ��
        else
        {
            //if (_canMove) return;
            if (Vector2.Distance(_currentCrystal.transform.position, player.transform.position) < 25)
            {
                Vector2 playerPos = player.transform.position;

                player.skill.clone.CloneCrystalBlink();  // ���ܷ�֧
                player.transform.position = _currentCrystal.transform.position;
                _currentCrystal.transform.position = playerPos;

                _currentCrystal.GetComponent<Skill_Crystal_Controller>().CrystalEnd();
                _currentCrystal = null;
            }
        }
    }

    #region Multi Logic

    private void UseMutiCrystal()
    {
        if (_crystalList.Count > 0)
        {
            // ����ʱ�䴰
            if (_crystalList.Count == _crystalCount)
                Invoke(nameof(ResetAbility), _useSkillWindow);

            coolDown = 0;
            GameObject chosen = _crystalList[_crystalList.Count - 1];
            GameObject newCrystal = Instantiate(chosen, player.transform.position, Quaternion.identity);
            _crystalList.Remove(chosen);

            Skill_Crystal_Controller ctrl = newCrystal.GetComponent<Skill_Crystal_Controller>();
            ctrl.SetUpCrystal(crystalDuration, _canExplode, _canMove, _moveSpeed);
        }
        // ��ȴ��װ��
        if (_crystalList.Count <= 0)
        {
            coolDown = _multiCoolDown;
            FillCrystalList();
        }
    }

    private void FillCrystalList()
    {
        _crystalList.Clear();
        for (int i = 0; i < _crystalCount; i++)
        {
            _crystalList.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (coolDownTimer <= 0)
        {
            coolDownTimer = _multiCoolDown;
            FillCrystalList();
        }
    }

    #endregion

    void CrystalUnlock ()
    {
        if (crystalUnlock.unlocked)
            _canUseCrystal = true;
    }

    void ExplodeUnlock ()
    {
        if(explodeUnlock.unlocked)
            _canExplode = true;
    }

    void MoveUnlock ()
    {
        if(moveUnlock.unlocked)
            _canMove = true;
    }

    void MultiUnlock ()
    {
        if(multiUnlock.unlocked)
            _canUseMulti = true;
    }

}
