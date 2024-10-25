using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    public override void ShowToolTip (string text)
    {
        description.text = text;
        base.ShowToolTip(text);
    }

    public override void HideToolTip()
    {
        description.text = "";
        base.HideToolTip();
    }
}
