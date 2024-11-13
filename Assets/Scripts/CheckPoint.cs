using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator _anim => GetComponent<Animator>();
    public string id;
    public bool isActive;

    [ContextMenu("Generate CheckPoint ID")]
    private void GenerateID ()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null && !isActive)
        {
            AudioManager.instance.PlaySFX("CheckPointUnlock");
            ActiveCheckPoint();
        }
    }

    public void ActiveCheckPoint ()
    {
        isActive = true;
        _anim.SetBool("Active", true);
    }
}
