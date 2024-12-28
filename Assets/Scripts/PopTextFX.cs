using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopTextFX : MonoBehaviour
{
    TextMeshPro _text => GetComponent<TextMeshPro>();

    float _speed = 1;
    float _fadeSpeed = 5;
    float _colorFadeSpeed = 10;
    [SerializeField] float _lifeTime; 

    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = _lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), _speed * Time.deltaTime);

        if (_timer <= 0)
        {
            float alpha = _text.color.a - _colorFadeSpeed * Time.deltaTime;

            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);

            if (_text.color.a < 50)
                _speed = _fadeSpeed;

            if (_text.color.a < 0)
                Destroy(gameObject);
        }
    }
}
