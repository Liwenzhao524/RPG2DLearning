using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    SpriteRenderer sr;
    KeyCode hotKey;
    TextMeshProUGUI tmpUI;

    Transform enemy;
    Skill_Blackhole_Controller blackhole;

    /// <summary>
    /// 初始化位置 UI
    /// </summary>
    /// <param name="_hotKey"></param>
    /// <param name="_enemy"></param>
    /// <param name="_blackhole"></param>
    public void SetUpHotKey(KeyCode _hotKey, Transform _enemy, Skill_Blackhole_Controller _blackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        tmpUI = GetComponentInChildren<TextMeshProUGUI>();

        hotKey = _hotKey;
        enemy = _enemy;
        blackhole = _blackhole;
        tmpUI.text = hotKey.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        // 按下对应按键 UI消失 敌人加入List
        if(Input.GetKeyDown(hotKey))
        {
            blackhole.AddEnemyToList(enemy);
            tmpUI.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
