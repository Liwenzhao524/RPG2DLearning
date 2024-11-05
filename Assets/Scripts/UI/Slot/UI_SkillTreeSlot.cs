using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public bool unlocked {  get; private set; }
    UI _mainUI;
    PlayerStats _stats;

    [SerializeField] string _skillName;
    [TextArea]
    [SerializeField] string _skillDescription;
    [SerializeField] int _skillCost;

    [SerializeField] UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] UI_SkillTreeSlot[] shouldBeLocked;
    Image _skillImage;

    // Start is called before the first frame update
    void Start()
    {
        _skillImage = GetComponent<Image>();
        _mainUI = GetComponentInParent<UI>();
        _stats = PlayerManager.instance.player.stats as PlayerStats;

        if (GetComponent<Button>().onClick.GetPersistentEventCount() == 0)
            GetComponent<Button>().onClick.AddListener(UnlockSkill);
    }

    private void OnValidate ()
    {
        gameObject.name = "Skill - " + _skillName;
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
                Debug.Log("You Can Only Choose One Upgrade!");
                return;
            }
        }

        if (!_stats.HasEnoughMoney(_skillCost)) return;

        unlocked = true;

        _skillImage.color = Color.white;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        _mainUI.skillToolTip.ShowToolTip(_skillDescription, _skillName, _skillCost.ToString());
        _mainUI.skillToolTip.transform.position = _mainUI.skillToolTip.SetToolTipPosition(eventData);
    }


    public void OnPointerExit (PointerEventData eventData)
    {
        _mainUI.skillToolTip.HideToolTip();
    }

}
