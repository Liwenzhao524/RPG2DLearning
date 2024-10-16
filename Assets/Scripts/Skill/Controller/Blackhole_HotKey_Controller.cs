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
        if(Input.GetKeyDown(hotKey))
        {
            blackhole.AddEnemyToList(enemy);
            tmpUI.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
