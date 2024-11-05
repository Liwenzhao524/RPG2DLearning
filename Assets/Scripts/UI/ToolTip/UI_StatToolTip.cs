using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    public override void ShowToolTip (string description, string name = null, string append = null)
    {
        base.description.text = description;
        base.ShowToolTip(description);
    }

    public override void HideToolTip()
    {
        description.text = "";
        base.HideToolTip();
    }
}
