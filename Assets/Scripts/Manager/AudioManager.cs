using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>
{
    Dictionary<string, AudioSource> _sfx = new();
    Dictionary<string, AudioSource> _bgm = new();

    [SerializeField] int _sfxMinDistance;
    public bool isBGMPlay;
    string _bgmName = string.Empty;

    private void Start ()
    {
        LoadAudioSource();
    }

    private void LoadAudioSource ()
    {
        Transform sfx = transform.GetChild(0);
        Transform bgm = transform.GetChild(1);
        for (int i = 0; i < sfx.childCount; i++)
        {
            _sfx.Add(sfx.GetChild(i).name, sfx.GetChild(i).GetComponent<AudioSource>());
        }

        for (int i = 0; i < bgm.childCount; i++)
        {
            _bgm.Add(bgm.GetChild(i).name, bgm.GetChild(i).GetComponent<AudioSource>());
        }
    }

    private void Update ()
    {
        if (isBGMPlay == false)
            StopAllBGM();
        else
        {
            if (_bgm.TryGetValue(_bgmName, out AudioSource value) && value.isPlaying == false)
                PlayRandomBGM();
        }
    }

    #region Sound Effect

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="target">如要应用衰减时，输入音源位置</param>
    public void PlaySFX (string name, Transform target = null)
    {
        if (target != null)
        {
            float distance = Vector2.Distance(target.position, PlayerManager.instance.player.transform.position);
            if (distance > _sfxMinDistance)
                return;
        }

        if (_sfx.TryGetValue(name, out var audio))
        {
            audio.pitch = Random.Range(0.8f, 1.2f);
            audio.Play();
        }

    }

    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="name">如不传 默认停止全部音效</param>
    public void StopSFX (string name = null)
    {
        if (name == null)
        {
            foreach (var pair in _sfx)
            {
                pair.Value.Stop();
            }
            return;
        }

        if (_sfx.TryGetValue(name, out var audio))
            audio.Stop();


    }

    #endregion

    #region BGM

    public void PlayRandomBGM ()
    {
        if (_bgm.Count == 0) return;
        string _bgmName = _bgm.Keys.ElementAt(Random.Range(0, _bgm.Count));

        PlayBGM(_bgmName);
    }

    public void PlayBGM (string name)
    {
        _bgmName = name;
        StopAllBGM();

        _bgm[_bgmName].Play();
    }

    private void StopAllBGM ()
    {
        if (_bgm.Count == 0) return;
        foreach (var pair in _bgm)
        {
            pair.Value.Stop();
        }
    }

    #endregion

}
