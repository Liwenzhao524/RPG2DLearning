using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效
/// </summary>
public class EntityFX : MonoBehaviour
{
    SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] Material flashMat;
    [SerializeField] float flashDuration = 0.2f;
    Material originMat;

    [Header("Ailment Color")]
    [SerializeField] Color[] igniteColor;
    [SerializeField] Color chillColor;
    [SerializeField] Color shockColor;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
    }

    /// <summary>
    /// 闪烁特效 切换材质实现
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {
        sr.material = flashMat;
        //Color temp = sr.color;
        //sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originMat;
        //sr.color = temp;
    }

    public void RedBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    #region Ailment Color


    public void ChangeToChillFX(float second)
    {
        sr.color = chillColor;
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToShockFX(float second)
    {
        sr.color = shockColor;
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToIgniteFX(float second)
    {
        InvokeRepeating(nameof(IgniteColor), 0, 0.3f);
        Invoke(nameof(CancelColorChange), second);
    }

    private void IgniteColor()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else sr.color = igniteColor[1];
    }

    #endregion

    public void MakeTransparent(bool istransparent)
    {
        if (istransparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
