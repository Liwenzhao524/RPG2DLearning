using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour, ISaveManager
{
    [SerializeField] Slider _bgmSlider; 
    [SerializeField] Slider _sfxSlider;
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] float _multiplier;

    public void SetBGMVolume()
    {
        _audioMixer.SetFloat("bgmVolume", Mathf.Log10(_bgmSlider.value) * _multiplier);
    }

    public void SetSFXVolume ()
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(_sfxSlider.value) * _multiplier);
    }

    public void LoadGame (GameData gameData)
    {
        _bgmSlider.value = gameData.bgmValue;
        _sfxSlider.value = gameData.sfxValue;
        SetBGMVolume();
        SetSFXVolume();
    }

    public void SaveGame (ref GameData gameData)
    {
        gameData.bgmValue = _bgmSlider.value;
        gameData.sfxValue = _sfxSlider.value;
    }

}
