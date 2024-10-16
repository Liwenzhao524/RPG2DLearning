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

    [Header("Bounce Info")]
    [SerializeField] private int bounceTime;
    
    [Tooltip("相对刻度")]
    [SerializeField] private float bounceGravityScale;  // *= 改变重力刻度

    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    private Vector2 finalDir;

    [Header("AimLine Info")]
    private int numOfDots = 20;
    private float dotsBetweenDis = 0.1f;
    private GameObject[] dots;
    [SerializeField] private GameObject dotsPrefab;
    [SerializeField] private GameObject dotsParent;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }
    protected override void Update()
    {
        base.Update();

        // 按下显示瞄准线
        if (Input.GetMouseButton(1))
        {
            for(int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotPosition(i * dotsBetweenDis);
            }
        }

        // 抬起时确定最终方向
        if(Input.GetMouseButtonUp(1)) 
            finalDir = new Vector2(launchDir.x * AimDirection().x, launchDir.y * AimDirection().y);
    }

    /// <summary>
    /// 创建并设置飞剑物体
    /// </summary>
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, _player.transform.position, transform.rotation);
        Skill_Sword_Controller ctrl = newSword.GetComponent<Skill_Sword_Controller>();
        ctrl.SetUpSword(finalDir, swordGravity, _player);

        if(swordType == Sword_Type.Bounce)
        {
            ctrl.SetupBounce(true, bounceTime, bounceGravityScale);
        }

        _player.AssignSword(newSword);
        SetDotsActive(false);
    }

    #region Aim
    /// <summary>
    /// 实时确定瞄准方向 返回单位方向向量
    /// </summary>
    /// <returns></returns>
    public Vector2 AimDirection()
    {
        Vector2 playerPos = _player.transform.position;
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
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(active);
        }
    }

    /// <summary>
    /// 初始化瞄准线
    /// </summary>
    private void GenerateDots()
    {
        dots = new GameObject[numOfDots];
        for(int i = 0; i < numOfDots; i++)
        {
            dots[i] = Instantiate(dotsPrefab, _player.transform.position, Quaternion.identity, dotsParent.transform);
            dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// 根据抛物线方程设置瞄准线
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector2 DotPosition(float t)
    {
        Vector2 position = (Vector2)_player.transform.position + new Vector2(
        AimDirection().x * launchDir.x * t,
        AimDirection().y * launchDir.y * t) + (0.5f * Physics2D.gravity * swordGravity * t * t);
        return position;
    }

    #endregion
}
