using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ±³¾°ÊÓ²î¼°±³¾°¸úËæ
/// </summary>
public class ParallaxBackGround : MonoBehaviour
{
    GameObject _cam;
    float _xPosition;
    float _length;  // ±³¾°Í¼Æ¬³¤¶È

    [SerializeField] float parallaxEffect;
    private void Start()
    {
        _cam = GameObject.Find("Main Camera");
        _xPosition = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float movedDis = _cam.transform.position.x * (1 - parallaxEffect);
        float moveDis = _cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(_xPosition + moveDis, transform.position.y);

        // ÊµÏÖ±³¾°¸úËæÍæ¼ÒÒÆ¶¯
        if (movedDis > _xPosition + _length) _xPosition += _length;
        else if(movedDis < _xPosition - _length) _xPosition -= _length;
    }
}
