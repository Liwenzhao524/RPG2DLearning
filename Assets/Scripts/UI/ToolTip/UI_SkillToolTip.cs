using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] TextMeshProUGUI _skillName;
    [SerializeField] TextMeshProUGUI _skillCost;

    public override void ShowToolTip (string skilldescription, string skillname = null, string skillcost = null)
    {
        _skillName.text = skillname;
        description.text = skilldescription;
        _skillCost.text = "Skill Cost: " + skillcost;
        base.ShowToolTip(skilldescription, skillname);
    }

    public override void HideToolTip ()
    {
        base.HideToolTip();
    }
}
