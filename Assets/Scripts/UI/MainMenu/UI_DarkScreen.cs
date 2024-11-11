using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DarkScreen : MonoBehaviour
{
    Animator _anim => GetComponent<Animator>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void FadeIn ()
    {
        _anim.SetTrigger("FadeIn");
    }

    public void FadeOut ()
    {
        _anim.SetTrigger("FadeOut");
    }
}
