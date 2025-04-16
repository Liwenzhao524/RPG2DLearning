using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class PopTextFX : MonoBehaviour
{
    TextMeshPro _text => GetComponent<TextMeshPro>();
    
    protected float _speed = 1;
    protected float _fadeSpeed = 5;
    protected float _colorFadeSpeed = 10;
    [SerializeField] float _lifeTime; 

    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        LuaManager.Instance.DoLuaFile("Hotfix_PopText");
        _timer = _lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), _speed * Time.deltaTime);

        if (_timer <= 0)
        {
            ColorTrans();
        }
    }

    [Hotfix]
    private void ColorTrans ()
    {
        float alpha = _text.color.a - _colorFadeSpeed * Time.deltaTime;

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);

        if (_text.color.a < 50)
            _speed = _fadeSpeed;

        if (_text.color.a < 0)
            Destroy(gameObject);
    }
}
