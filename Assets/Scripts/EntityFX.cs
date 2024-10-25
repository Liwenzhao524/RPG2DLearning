using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效
/// </summary>
public class EntityFX : MonoBehaviour
{
    SpriteRenderer _sr;

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
        _sr = GetComponentInChildren<SpriteRenderer>();
        originMat = _sr.material;
    }

    /// <summary>
    /// 闪烁特效 切换材质实现
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {
        _sr.material = flashMat;
        //Color _temp = _sr.color;
        //_sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        _sr.material = originMat;
        //_sr.color = _temp;
    }

    public void RedBlink()
    {
        if(_sr.color != Color.white)
            _sr.color = Color.white;
        else _sr.color = Color.red;
    }

    public void CancelColorChange()
    {
        CancelInvoke();
        _sr.color = Color.white;
    }

    #region Ailment Color


    public void ChangeToChillFX(float second)
    {
        _sr.color = chillColor;
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToShockFX(float second)
    {
        _sr.color = shockColor;
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToIgniteFX(float second)
    {
        InvokeRepeating(nameof(IgniteColor), 0, 0.3f);
        Invoke(nameof(CancelColorChange), second);
    }

    private void IgniteColor()
    {
        if (_sr.color != igniteColor[0])
            _sr.color = igniteColor[0];
        else _sr.color = igniteColor[1];
    }

    #endregion

    public void MakeTransparent(bool istransparent)
    {
        if (istransparent)
        {
            _sr.color = Color.clear;
        }
        else
        {
            _sr.color = Color.white;
        }
    }
}
