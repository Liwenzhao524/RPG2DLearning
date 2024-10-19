using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    public Entity _entity;
    public CharacterStats _stat;
    public RectTransform _rect;
    public Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponentInParent<Entity>();
        _rect = GetComponent<RectTransform>();
        _slider = GetComponentInChildren<Slider>();
        _stat = _entity.stats;

        _entity.OnFlip += FlipUI;
        _stat.HPChange += UpdateHealthUI;

        UpdateHealthUI();
    }
     
    private void UpdateHealthUI()
    {
        _slider.maxValue = _entity.stats.GetMaxHP(); 
        _slider.value = _entity.stats.currentHP;
    }

    private void FlipUI()
    {
        _rect.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        _entity.OnFlip -= FlipUI;
        _stat.HPChange -= UpdateHealthUI;
    }

}
