using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ӳ��������
/// </summary>
public class ParallaxBackGround : MonoBehaviour
{
    GameObject _cam;
    float _xPosition;
    float _length;  // ����ͼƬ����

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

        // ʵ�ֱ�����������ƶ�
        if (movedDis > _xPosition + _length) _xPosition += _length;
        else if(movedDis < _xPosition - _length) _xPosition -= _length;
    }
}
