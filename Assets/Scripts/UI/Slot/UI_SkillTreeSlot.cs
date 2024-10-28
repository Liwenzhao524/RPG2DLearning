using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool unlocked;
    UI _mainUI;

    [SerializeField] string skillName;
    [TextArea]
    [SerializeField] string skillDescription;

    [SerializeField] UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] UI_SkillTreeSlot[] shouldBeLocked;
    Image _skillImage;

    // Start is called before the first frame update
    void Start()
    {
        _skillImage = GetComponent<Image>();
        _mainUI = GetComponentInParent<UI>();

        GetComponent<Button>().onClick.AddListener(UnlockSkill);
    }

    private void OnValidate ()
    {
        gameObject.name = "Skill - " + skillName;
    }

    public void UnlockSkill ()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (!shouldBeUnlocked[i].unlocked)
            {
                Debug.Log(shouldBeUnlocked[i].name + " should be unlocked");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked)
            {
                Debug.Log(shouldBeLocked[i].name + " should be locked");
                return;
            }
        }

        unlocked = true;
        _skillImage.color = Color.white;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        _mainUI.skillToolTip.ShowToolTip(skillDescription, skillName);
        _mainUI.skillToolTip.transform.position = SetToolTipPosition(eventData);
    }


    public void OnPointerExit (PointerEventData eventData)
    {
        _mainUI.skillToolTip.HideToolTip();
    }

    /// <summary>
    /// 根据Slot位置 调整ToolTip位置
    /// </summary>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public Vector2 SetToolTipPosition (PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position;

        float xOffset = 0;

        if (mousePos.x > Screen.width / 2 ) xOffset = - Screen.width / 4;
        else xOffset = Screen.width / 4;

        Vector2 newPos = new(mousePos.x + xOffset, mousePos.y + 100);
        return newPos;
    }

}
