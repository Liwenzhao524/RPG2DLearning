using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������²�Ӱ ����
/// </summary>
public class Skill_Clone : Skill
{
    [Header("Clone Info")]
    [SerializeField] GameObject _newClonePrefab;
    [SerializeField] float _cloneDuration;

    [Header("Attack")]
    [SerializeField] UI_SkillTreeSlot _attackUnlock;
    public bool canAttack {  get; private set; }

    [Header("Aggresive")]
    [SerializeField] UI_SkillTreeSlot _aggresiveUnlock;
    public bool beAggresive {  get; private set; }

    [Header("Clone Crystal Blink")]
    [SerializeField] UI_SkillTreeSlot _cloneCrystalMirageUnlock;
    bool _canCloneCrystalBlink;

    [Header("Clone Duplicate")]
    [SerializeField] UI_SkillTreeSlot _duplicateUnlock;
    [SerializeField] float _duplicateChance;
    bool _canDuplicate;

    [Header("Crystal Instead Clone")]
    [SerializeField] UI_SkillTreeSlot _crystalInsteadCloneUnlock;
    bool _canCrystalInsteadClone;

    protected override void Start ()
    {
        base.Start();

        _attackUnlock.GetComponent<Button>().onClick.AddListener(AttackUnlock);
        _aggresiveUnlock.GetComponent<Button>().onClick.AddListener(AggresiveUnlock);
        _cloneCrystalMirageUnlock.GetComponent<Button>().onClick.AddListener(CloneCrystalMirageUnlock);
        _duplicateUnlock.GetComponent<Button>().onClick.AddListener(DuplicateUnlock);
        _crystalInsteadCloneUnlock.GetComponent<Button>().onClick.AddListener(CrystalInsteadCloneUnlock);

    }

    /// <summary>
    /// ������Ӱ ��ƫ����
    /// </summary>
    /// <param name="targetPos">����λ��</param>
    /// <param name="offset">λ��ƫ����</param>
    public void CreateClone(Transform targetPos, Vector3 offset)
    {
        if (_canCrystalInsteadClone)
        {
            player.skill.crystal.CreateCrystal(targetPos);
            return;
        }
        GameObject clone = Instantiate(_newClonePrefab);
        clone.GetComponent<Skill_Clone_Controller>().SetClone(targetPos, _cloneDuration, canAttack, offset);
    }

    /// <summary>
    /// ������Ӱ ��ƫ����
    /// </summary>
    /// <param name="targetPos"></param>
    public void CreateClone(Transform targetPos)
    {
        CreateClone(targetPos, new Vector3(0, 0, 0));
    }

    public IEnumerator CreateCloneDelay (Transform targetPos, Vector3 offset, float delay)
    {
        yield return new WaitForSeconds(delay);
        CreateClone(targetPos, offset);
    }

    /// <summary>
    /// ��λʱ��ԭλ������crystal
    /// </summary>
    public void CloneCrystalBlink()
    {
        if(_canCloneCrystalBlink)
        {
            CreateClone(player.transform, new Vector3(0, 0, 0));
        }
    }

    /// <summary>
    /// ��Ӱ���Ը��Ʋ�Ӱ
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="faceDir"></param>
    public void CloneDuplicate(Transform targetPos, int faceDir = 1)
    {
        if (_canDuplicate)
        {
            _duplicateChance = Mathf.Clamp(_duplicateChance, 0, 100);
            if (Random.Range(0, 100) < _duplicateChance)
            {
                CreateClone(targetPos.transform, new Vector3(1.5f * faceDir, 0));
            }
        }
    }

    void AttackUnlock ()
    {
        if (_attackUnlock.unlocked)
            canAttack = true;
    }

    void AggresiveUnlock ()
    {
        if (_aggresiveUnlock.unlocked)
            beAggresive = true;
    }

    void CloneCrystalMirageUnlock ()
    {
        if(_cloneCrystalMirageUnlock.unlocked)
            _canCloneCrystalBlink = true;
    }

    void DuplicateUnlock ()
    {
        if(_duplicateUnlock.unlocked)
            _canDuplicate = true;
    }

    void CrystalInsteadCloneUnlock ()
    {
        if(_crystalInsteadCloneUnlock.unlocked)
            _canCrystalInsteadClone = true;
    } 
}
