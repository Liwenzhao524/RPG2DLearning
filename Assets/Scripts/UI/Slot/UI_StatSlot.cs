using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UI _mainUI => GetComponentInParent<UI>();

    [SerializeField] StatsType statType;
    [SerializeField] TextMeshProUGUI statName;
    [SerializeField] TextMeshProUGUI statValue;

    [TextArea]
    [SerializeField] string statDescription;
     
    private void OnValidate ()
    {
        gameObject.name = "Stat - " + statType.ToString();

        if (statName != null) statName.text = statType.ToString();

    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStatUI();
    }

    public void UpdateStatUI ()
    {
        if (PlayerManager.instance.player.TryGetComponent<PlayerStats>(out var playerStats))
        {
            statValue.text = playerStats.GetStatsByType(statType).GetValue().ToString();

            if (statType == StatsType.ATK)
                statValue.text = playerStats.GetATK().ToString();

            if (statType == StatsType.maxHP)
                statValue.text = playerStats.GetMaxHP().ToString();

            if (statType == StatsType.evasion)
                statValue.text = playerStats.GetEvasion().ToString();

            if (statType == StatsType.magicResist)
                statValue.text = playerStats.GetMagicResist().ToString();

            if (statType == StatsType.critChance)
                statValue.text = playerStats.GetCritChance().ToString() + "%";

            if (statType == StatsType.critDamage)
                statValue.text = ( playerStats.GetCritDamage() * 100 ).ToString() + "%";

        }

    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        _mainUI.statToolTip.ShowToolTip(statDescription);
    }

    public void OnPointerExit (PointerEventData eventData)
    {
       _mainUI.statToolTip.HideToolTip();
    }
}
