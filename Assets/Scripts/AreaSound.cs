using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] string _soundName;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        AudioManager.instance.PlaySFX(_soundName);
    }
    
    private void OnTriggerExit2D (Collider2D collision)
    {
        AudioManager.instance.StopSFX(_soundName);
    }
}
