using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    PlayerStats _stats;
    SkillManager _skill;
    Slider _HPSlider;

    [SerializeField] Image _dashImage;
    [SerializeField] Image _crystalImage;
    [SerializeField] Image _parryImage;
    [SerializeField] Image _blackholeImage;
    [SerializeField] Image _flaskImage;

    [SerializeField] TextMeshProUGUI _currentMoney;
    [SerializeField] float _increaseRate = 10000;
    float _moneyAmount;
    // Start is called before the first frame update
    void Start()
    {
        _stats = PlayerManager.instance.player.stats as PlayerStats;
        _skill = SkillManager.instance;
        _HPSlider = transform.GetChild(0).GetComponent<Slider>();

        if (_stats != null)
            _stats.HPChange += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update ()
    {
        UpdateMoneyUI();

        _currentMoney.text = "Money: " + ( (int)_moneyAmount ).ToString("#,#");

        if (_skill.dashUse)
        {
            SetCoolDown(_dashImage);
            _skill.dashUse = false;
        }

        if (_skill.crystalUse)
        {
            SetCoolDown(_crystalImage);
            _skill.crystalUse = false;
        }

        if (_skill.parryUse)
        {
            SetCoolDown(_parryImage);
            _skill.parryUse = false;
        }

        if (_skill.blackholeUse)
        {
            SetCoolDown(_blackholeImage);
            _skill.blackholeUse = false;
        }

        if (Inventory.instance.flaskUse)
        {
            SetCoolDown(_flaskImage);
            Inventory.instance.flaskUse = false;
        }

        CheckCoolDown(_dashImage, _skill.dash.coolDown);
        CheckCoolDown(_crystalImage, _skill.crystal.coolDown);
        CheckCoolDown(_blackholeImage, _skill.blackhole.coolDown);
        CheckCoolDown(_parryImage, _skill.parry.coolDown);
        CheckCoolDown(_flaskImage, Inventory.instance.flaskCoolDown);
    }

    private void UpdateMoneyUI ()
    {
        if (_moneyAmount < _stats.ReturnCurrentMoney())
            _moneyAmount += Time.deltaTime * _increaseRate;
        else
            _moneyAmount = _stats.ReturnCurrentMoney();
    }

    private void UpdateHealthUI ()
    {
        _HPSlider.maxValue = _stats.GetMaxHP();
        _HPSlider.value = _stats.currentHP;
    }

    void SetCoolDown(Image image)
    {
        if (image.fillAmount <= 0)
            image.fillAmount = 1;
    }

    void CheckCoolDown(Image image, float coolDown)
    {
        if(image.fillAmount > 0)
        {
            image.fillAmount -= 1 / coolDown * Time.deltaTime;
        }
    }
}
