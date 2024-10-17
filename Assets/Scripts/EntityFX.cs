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

    private void RedBlink()
    {
        if(sr.color != Color.white)
            sr.color = Color.white;
        else sr.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

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
