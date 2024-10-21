using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    SpriteRenderer _sr;
    KeyCode _hotKey;
    TextMeshProUGUI _tmpUI;

    Transform _enemy;
    Skill_Blackhole_Controller _blackhole;

    /// <summary>
    /// 初始化位置 UI
    /// </summary>
    /// <param name="_hotKey"></param>
    /// <param name="_enemy"></param>
    /// <param name="_blackhole"></param>
    public void SetUpHotKey(KeyCode _hotKey, Transform _enemy, Skill_Blackhole_Controller _blackhole)
    {
        _sr = GetComponent<SpriteRenderer>();
        _tmpUI = GetComponentInChildren<TextMeshProUGUI>();

        this._hotKey = _hotKey;
        this._enemy = _enemy;
        this._blackhole = _blackhole;
        _tmpUI.text = this._hotKey.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        // 按下对应按键 UI消失 敌人加入List
        if(Input.GetKeyDown(_hotKey))
        {
            _blackhole.AddEnemyToList(_enemy);
            _tmpUI.color = Color.clear;
            _sr.color = Color.clear;
        }
    }
}
