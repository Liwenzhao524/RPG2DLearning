using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ±³¾°ÊÓ²î¼°±³¾°¸úËæ
/// </summary>
public class ParallaxBackGround : MonoBehaviour
{
    GameObject cam;
    float xPosition;
    float length;  // ±³¾°Í¼Æ¬³¤¶È

    [SerializeField] float parallaxEffect;
    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float movedDis = cam.transform.position.x * (1 - parallaxEffect);
        float moveDis = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + moveDis, transform.position.y);

        // ÊµÏÖ±³¾°¸úËæÍæ¼ÒÒÆ¶¯
        if (movedDis > xPosition + length) xPosition += length;
        else if(movedDis < xPosition - length) xPosition -= length;
    }
}
