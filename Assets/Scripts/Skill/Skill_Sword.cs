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
/// �ɽ� ����
/// </summary>
public class Skill_Sword : Skill
{
    public Sword_Type swordType = Sword_Type.Regular;

    [Header("Skill Info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchDir;
    [SerializeField] float swordGravity;
    [SerializeField] float freezeDuration;
    [SerializeField] float returnSpeed;

    [Header("AimLine Info")]
    int numOfDots = 20;
    float dotsBetweenDis = 0.1f;
    GameObject[] dots;
    [SerializeField] GameObject dotsPrefab;
    [SerializeField] GameObject dotsParent;
    Vector2 finalDir;

    [Header("Bounce Info")]
    [SerializeField] int bounceCount;
    [SerializeField] float bounceGravityScale = 0.8f;
    [SerializeField] float bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] int pierceCount;
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
    /// ���������趨����
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

        // ������ʾ��׼��
        if (Input.GetMouseButton(1))
        {
            for(int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotPosition(i * dotsBetweenDis);
            }
        }

        // ̧��ʱȷ�����շ���
        if(Input.GetMouseButtonUp(1)) 
            finalDir = new Vector2(launchDir.x * AimDirection().x, launchDir.y * AimDirection().y);
    }

    /// <summary>
    /// ���������÷ɽ�����
    /// </summary>
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, _player.transform.position, transform.rotation);
        Skill_Sword_Controller ctrl = newSword.GetComponent<Skill_Sword_Controller>();
        ctrl.SetUpSword(finalDir, swordGravity, _player, freezeDuration, returnSpeed);

        switch (swordType)
        {
            case Sword_Type.Regular: break;
            case Sword_Type.Bounce: ctrl.SetupBounce(true, bounceCount, bounceSpeed); break;
            case Sword_Type.Pierce: ctrl.SetupPierce(pierceCount);  break;
            case Sword_Type.Spinning: ctrl.SetupSpin(true, maxDistance, spinDuration, hitCoolDown);  break;
        }

        _player.AssignSword(newSword);
        SetDotsActive(false);
    }

    #region Aim
    /// <summary>
    /// ʵʱȷ����׼���� ���ص�λ��������
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
    /// �Ƿ񼤻���׼��
    /// </summary>
    /// <param name="active">�����Ƿ�</param>
    public void SetDotsActive(bool active)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(active);
        }
    }

    /// <summary>
    /// ��ʼ����׼��
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
    /// ���������߷���������׼��
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
