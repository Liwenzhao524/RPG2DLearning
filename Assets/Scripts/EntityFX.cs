using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public IEnumerator FlashFX()
    {
        sr.material = flashMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originMat;
    }
}
