using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI skillName;

    public override void ShowToolTip (string skilldescription, string skillname = null)
    {
        skillName.text = skillname;
        description.text = skilldescription;
        base.ShowToolTip(skilldescription, skillname);
    }

    public override void HideToolTip ()
    {
        base.HideToolTip();
    }
}
