using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Crystal : Skill
{
    GameObject _currentCrystal;

    [SerializeField] GameObject _crystalPrefab;
    [SerializeField] float _crystalDuration;

    [Header("Crystal Unlock")]
    [SerializeField] UI_SkillTreeSlot _crystalUnlock;
    public bool canUseCrystal { get; private set; }

    [Header("Explode Unlock")]
    [SerializeField] UI_SkillTreeSlot _explodeUnlock;
    bool _canExplode;

    [Header("Move Unlock")]
    [SerializeField] UI_SkillTreeSlot _moveUnlock;
    bool _canMove;
    float _moveSpeed = 5;

    [Header("Multi Unlock")]
    [SerializeField] UI_SkillTreeSlot _multiUnlock;
    bool _canUseMulti;

    float _useSkillWindow = 5;
    int _crystalCount = 3;
    float _multiCoolDown = 3;
    List<GameObject> _crystalList = new();

    protected override void Start ()
    {
        base.Start();

        _crystalUnlock.GetComponent<Button>().onClick.AddListener(CrystalUnlock);
        _explodeUnlock.GetComponent<Button>().onClick.AddListener(ExplodeUnlock);
        _moveUnlock.GetComponent<Button>().onClick.AddListener (MoveUnlock);
        _multiUnlock.GetComponent<Button>().onClick.AddListener(MultiUnlock);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        SkillManager.instance.crystalUse = true;

        if (_canUseMulti)
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
        _currentCrystal = Instantiate(_crystalPrefab, targetPos.position, Quaternion.identity);
        Skill_Crystal_Controller ctrl = _currentCrystal.GetComponent<Skill_Crystal_Controller>();
        ctrl.SetUpCrystal(_crystalDuration, _canExplode, _canMove, _moveSpeed);
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
            if (Vector2.Distance(_currentCrystal.transform.position, player.transform.position) < 25)
            {
                Vector2 playerPos = player.transform.position;

                player.skill.clone.CloneCrystalBlink();  // 技能分支
                player.transform.position = _currentCrystal.transform.position;
                _currentCrystal.transform.position = playerPos;

                _currentCrystal.GetComponent<Skill_Crystal_Controller>().CrystalEnd();
                _currentCrystal = null;

                coolDownTimer = coolDown;
            }
        }
    }

    #region Multi Logic

    private void UseMutiCrystal()
    {
        if (_crystalList.Count > 0)
        {
            // 重置时间窗
            if (_crystalList.Count == _crystalCount)
                Invoke(nameof(ResetAbility), _useSkillWindow);

            coolDown = 0;
            GameObject chosen = _crystalList[_crystalList.Count - 1];
            GameObject newCrystal = Instantiate(chosen, player.transform.position, Quaternion.identity);
            _crystalList.Remove(chosen);

            Skill_Crystal_Controller ctrl = newCrystal.GetComponent<Skill_Crystal_Controller>();
            ctrl.SetUpCrystal(_crystalDuration, _canExplode, _canMove, _moveSpeed);
        }
        // 冷却再装填
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
            _crystalList.Add(_crystalPrefab);
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

    protected override void LoadUnlock ()
    {
        CrystalUnlock();
        ExplodeUnlock();
        MoveUnlock();
        MultiUnlock();
    }

    void CrystalUnlock ()
    {
        if (_crystalUnlock.unlocked)
            canUseCrystal = true;
    }

    void ExplodeUnlock ()
    {
        if(_explodeUnlock.unlocked)
            _canExplode = true;
    }

    void MoveUnlock ()
    {
        if(_moveUnlock.unlocked)
            _canMove = true;
    }

    void MultiUnlock ()
    {
        if(_multiUnlock.unlocked)
            _canUseMulti = true;
    }

}
