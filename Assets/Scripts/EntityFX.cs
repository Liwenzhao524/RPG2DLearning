using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效
/// </summary>
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material flashMat;
    [SerializeField] private float flashDuration = 0.2f;
    private Material originMat;

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

        yield return new WaitForSeconds(flashDuration);

        sr.material = originMat;
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

    public void ChangeToIgniteFX(float second)
    {
        InvokeRepeating("IgniteColor", 0, 0.3f);
        Invoke("CancelColorChange", second);
    }

    public void ChangeToChillFX(float second)
    {
        sr.color = chillColor;
        Invoke("CancelColorChange", second);
    }

    public void ChangeToShockFX(float second)
    {
        sr.color = shockColor;
        Invoke("CancelColorChange", second);
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
