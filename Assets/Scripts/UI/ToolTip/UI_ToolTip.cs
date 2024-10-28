using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI description;

    public virtual void ShowToolTip (ItemData_Equipment item)
    {
        gameObject.SetActive(true);
    }

    public virtual void ShowToolTip(string description, string name = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void HideToolTip (ItemData_Equipment item)
    {
        gameObject.SetActive(false);
    }

    public virtual void HideToolTip ()
    {
        gameObject.SetActive(false);
    }
}
