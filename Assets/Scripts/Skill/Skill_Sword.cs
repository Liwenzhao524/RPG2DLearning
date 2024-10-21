using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] float swordGravity = 2;
    [SerializeField] float freezeDuration = 1;
    [SerializeField] float returnSpeed = 30;

    [Header("AimLine Info")]
    readonly int _numOfDots = 25;
    readonly float _dotsBetweenDis = 0.1f;
    GameObject[] _dots;
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] GameObject dotsParent;
    Vector2 _finalDir;

    [Header("Bounce Info")]
    [SerializeField] int bounceCount = 4;
    [SerializeField] float bounceGravityScale = 0.8f;
    [SerializeField] float bounceSpeed = 20;

    [Header("Pierce Info")]
    [SerializeField] int pierceCount = 1;
    [SerializeField] float pierceGravityScale = 0.03f;

    [Header("Spin Info")]
    [SerializeField] float maxDistance = 10;
    [SerializeField] float spinDuration = 1;
    [SerializeField] float spinGravityScale = 0.5f;
    [SerializeField] float hitCoolDown = 0.34f;

    protected override void Start()
    {
        base.Start();
        SetSwordGravity();
        GenerateDots();    
    }

    /// <summary>
    /// 根据类型设定重力
    /// </summary>
    private void SetSwordGravity()
    {
        switch (swordType)
        {
            default:
            case Sword_Type.Bounce: swordGravity *= bounceGravityScale; break;
            case Sword_Type.Pierce: swordGravity *= pierceGravityScale; break;
            case Sword_Type.Spinning: swordGravity *= spinGravityScale; break;
        }
    }

    protected override void Update()
    {
        base.Update();

        // 按下显示瞄准线
        if (Input.GetMouseButton(1))
        {
            for(int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotPosition(i * _dotsBetweenDis);
            }
        }

        // 抬起时确定最终方向
        if(Input.GetMouseButtonUp(1)) 
            _finalDir = new Vector2(launchDir.x * AimDirection().x, launchDir.y * AimDirection().y);
    }

    /// <summary>
    /// 创建并设置飞剑物体
    /// </summary>
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Skill_Sword_Controller ctrl = newSword.GetComponent<Skill_Sword_Controller>();
        ctrl.SetUpSword(_finalDir, swordGravity, player, freezeDuration, returnSpeed);

        switch (swordType)
        {
            case Sword_Type.Regular: break;
            case Sword_Type.Bounce: ctrl.SetupBounce(true, bounceCount, bounceSpeed); break;
            case Sword_Type.Pierce: ctrl.SetupPierce(pierceCount);  break;
            case Sword_Type.Spinning: ctrl.SetupSpin(true, maxDistance, spinDuration, hitCoolDown);  break;
        }

        player.AssignSword(newSword);
        SetDotsActive(false);
    }

    #region Aim
    /// <summary>
    /// 实时确定瞄准方向 返回单位方向向量
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimPos = (mousePos - playerPos);
        aimPos.Normalize();

        return aimPos;
    }

    /// <summary>
    /// 是否激活瞄准线
    /// </summary>
    /// <param name="active">传入是否</param>
    public void SetDotsActive(bool active)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(active);
        }
    }

    /// <summary>
    /// 初始化瞄准线
    /// </summary>
    private void GenerateDots()
    {
        _dots = new GameObject[_numOfDots];
        for(int i = 0; i < _numOfDots; i++)
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
    private Vector2 DotPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
        AimDirection().x * launchDir.x * t,
        AimDirection().y * launchDir.y * t) + (0.5f * Physics2D.gravity * swordGravity * t * t);
        return position;
    }

    #endregion
}
