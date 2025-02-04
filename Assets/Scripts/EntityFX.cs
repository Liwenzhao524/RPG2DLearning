using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum HitType
{
    Primary,
    Critical,
    Crystal,
}

/// <summary>
/// 特效
/// </summary>
public class EntityFX : MonoBehaviour
{
    SpriteRenderer _sr;

    [Header("Flash FX")]
    [SerializeField] Material flashMat;
    [SerializeField] float _flashDuration = 0.2f;
    Material _originMat;

    [Header("Ailment Color")]
    [SerializeField] Color[] igniteColor;
    [SerializeField] Color chillColor;
    [SerializeField] Color shockColor;

    [Header("Ailment FX")]
    [SerializeField] ParticleSystem _igniteFX;
    [SerializeField] ParticleSystem _chillFX;
    [SerializeField] ParticleSystem _shockFX;

    [Header("Pop Text")]
    [SerializeField] GameObject _popTextPrefab;

    [Header("Hit FX")]
    [SerializeField] GameObject _primaryHitFX;
    [SerializeField] GameObject _criticalHitFX;
    [SerializeField] GameObject _crystalHitFX;

    // Start is called before the first frame update
    void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originMat = _sr.material;
    }

    public void CreatePopText(string text, Color color)
    {
        float xOffset = Random.Range(-1, 1);
        float yOffset = Random.Range(1, 2);

        Vector3 posOffset = new Vector3(xOffset, yOffset, 0);

        GameObject newText = Instantiate(_popTextPrefab, transform.position + posOffset, Quaternion.identity);
        newText.GetComponent<TextMeshPro>().color = color;
        newText.GetComponent<TextMeshPro>().text = text;
    }

    public void CreatePopText (string text) => CreatePopText(text, Color.white);


    /// <summary>
    /// 闪烁特效 切换材质实现
    /// </summary>
    /// <returns></returns>
    public IEnumerator FlashFX()
    {
        _sr.material = flashMat;
        //Color _backGround = _sr.color;
        //_sr.color = Color.white;

        yield return new WaitForSeconds(_flashDuration);

        _sr.material = _originMat;
        //_sr.color = _backGround;
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

        _chillFX.Stop();
        _shockFX.Stop();
        _igniteFX.Stop();
        _sr.color = Color.white;
    }

    #region Ailment


    public void ChangeToChillFX(float second)
    {
        _sr.color = chillColor;
        _chillFX.Play();
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToShockFX(float second)
    {
        _sr.color = shockColor;
        _shockFX.Play();
        Invoke(nameof(CancelColorChange), second);
    }

    public void ChangeToIgniteFX(float second)
    {
        _igniteFX.Play();
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

    public void CreateHitFX(Transform target, HitType hitType)
    {
        GameObject hitFX = _primaryHitFX;

        Vector2 position = new(target.position.x + Random.Range(-0.5f, 0.5f), target.position.y + Random.Range(-0.5f, 0.5f));
        Vector3 rotation = new(0, 0, Random.Range(-90, 90));

        switch (hitType)
        {
            case HitType.Critical: 
                { 
                    hitFX = _criticalHitFX;
                    float yRotation = 0;
                    float zRotation = Random.Range(-45, 45);

                    if (GetComponent<Entity>().faceDir == -1) yRotation = 180;
                    rotation = new Vector3(0, yRotation, zRotation);
                } 
                break;
            case HitType.Crystal: 
                { 
                    hitFX = _crystalHitFX; 
                } 
                break;
            default: break;
        }

        GameObject newHitFX = Instantiate(hitFX, position, Quaternion.identity);
        newHitFX.transform.Rotate(rotation);

        Destroy(newHitFX, 1);

    }

}
