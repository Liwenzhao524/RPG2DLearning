using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public enum Sword_Type
{
    Regular,
    Bounce,
    Pierce,
    Spinning
}

/// <summary>
/// 飞剑 技能
/// </summary>
public class Skill_Sword : Skill
{
    public Sword_Type swordType = Sword_Type.Regular;

    [Header("Skill Info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchDir;
    float _swordGravity = 1;
    
    float _returnSpeed = 30;

    [Header("AimLine Info")]
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] GameObject dotsParent;
    int _numOfDots = 25;
    float _dotsBetweenDis = 0.1f;
    GameObject[] _dots;
    Vector2 _finalDir;

    [Header("Time Stop")]
    float _freezeDuration = 0;
    [SerializeField] UI_SkillTreeSlot timestopUnlock;

    //[Header("Bounce")]
    bool _canBounce;
    [SerializeField] UI_SkillTreeSlot bounceUnlock;

    int _bounceCount = 4;
    float _bounceGravityScale = 0.8f;
    float _bounceSpeed = 20;

    [Header("Pierce")]
    bool _canPierce;
    [SerializeField] UI_SkillTreeSlot pierceUnlock;

    int _pierceCount = 3;
    float _pierceGravityScale = 0.03f;

    [Header("Spin")]
    bool _canSpin;
    [SerializeField] UI_SkillTreeSlot spinUnlock;

    float _maxDistance = 5;
    float _spinDuration = 1;
    float _spinGravityScale = 0.5f;
    float _hitCoolDown = 0.34f;

    protected override void Start ()
    {
        base.Start();
        GenerateDots();

        bounceUnlock.GetComponent<Button>().onClick.AddListener(BounceUnlock);
        pierceUnlock.GetComponent<Button>().onClick.AddListener(PierceUnlock);
        spinUnlock.GetComponent<Button>().onClick.AddListener(SpinUnlock);
        timestopUnlock.GetComponent<Button>().onClick.AddListener(TimeStopUnlock);
    }

    /// <summary>
    /// 根据类型设定重力
    /// </summary>
    private void SetSwordGravity ()
    {
        switch (swordType)
        {
            default:
            case Sword_Type.Bounce: _swordGravity *= _bounceGravityScale; break;
            case Sword_Type.Pierce: _swordGravity *= _pierceGravityScale; break;
            case Sword_Type.Spinning: _swordGravity *= _spinGravityScale; break;
        }
    }

    protected override void Update ()
    {
        base.Update();

        // 按下显示瞄准线
        if (Input.GetMouseButton(1))
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotPosition(i * _dotsBetweenDis);
            }
        }

        // 抬起时确定最终方向
        if (Input.GetMouseButtonUp(1))
            _finalDir = new Vector2(launchDir.x * AimDirection().x, launchDir.y * AimDirection().y);
    }

    /// <summary>
    /// 创建并设置飞剑物体
    /// </summary>
    public void CreateSword ()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Skill_Sword_Controller ctrl = newSword.GetComponent<Skill_Sword_Controller>();

        ctrl.SetUpSword(_finalDir, _swordGravity, _freezeDuration, _returnSpeed);

        switch (swordType)
        {
            case Sword_Type.Regular: break;
            case Sword_Type.Bounce: ctrl.SetupBounce(_canBounce, _bounceCount, _bounceSpeed); break;
            case Sword_Type.Pierce: ctrl.SetupPierce(_canPierce, _pierceCount); break;
            case Sword_Type.Spinning: ctrl.SetupSpin(_canSpin, _maxDistance, _spinDuration, _hitCoolDown); break;
        }

        player.AssignSword(newSword);
        SetDotsActive(false);
    }

    void TimeStopUnlock ()
    {
        if (timestopUnlock.unlocked)
            _freezeDuration = 1;
    }

    void BounceUnlock ()
    {
        if (bounceUnlock.unlocked)
        {
            _canBounce = true;
            swordType = Sword_Type.Bounce;
            SetSwordGravity();
        }
    }

    void PierceUnlock ()
    {
        if (pierceUnlock.unlocked)
        {
            _canPierce = true;
            swordType = Sword_Type.Pierce;
            SetSwordGravity();
        }
    }

    void SpinUnlock ()
    {
        if (spinUnlock.unlocked)
        {
            _canSpin = true;
            swordType = Sword_Type.Spinning;
            SetSwordGravity();
        }
    }

    #region Aim
    /// <summary>
    /// 实时确定瞄准方向 返回单位方向向量
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection ()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimPos = ( mousePos - playerPos );
        aimPos.Normalize();

        return aimPos;
    }

    /// <summary>
    /// 是否激活瞄准线
    /// </summary>
    /// <param name="active">传入是否</param>
    public void SetDotsActive (bool active)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(active);
        }
    }

    /// <summary>
    /// 初始化瞄准线
    /// </summary>
    private void GenerateDots ()
    {
        _dots = new GameObject[_numOfDots];
        for (int i = 0; i < _numOfDots; i++)
        {
            _dots[i] = Instantiate(dotsPrefab, player.transform.position, Quaternion.identity, dotsParent.transform);
            _dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// 根据抛物线方程设置瞄准线
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 DotPosition (float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
        AimDirection().x * launchDir.x * t,
        AimDirection().y * launchDir.y * t) + ( 0.5f * Physics2D.gravity * _swordGravity * t * t );
        return position;
    }

    #endregion
}
