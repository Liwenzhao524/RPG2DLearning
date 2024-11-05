using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI description;

    /// <summary>
    /// 根据Slot位置 调整ToolTip位置
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public Vector2 SetToolTipPosition (PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        float xOffset = 0;

        if (mousePos.x > Screen.width / 2) xOffset = -Screen.width / 6;
        else xOffset = Screen.width / 6;

        Vector2 newPos = new(mousePos.x + xOffset, mousePos.y + 100);
        return newPos;
    }

    public virtual void ShowToolTip (ItemData_Equipment item)
    {
        gameObject.SetActive(true);
    }

    public virtual void ShowToolTip(string description, string name = null, string append = null)
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
