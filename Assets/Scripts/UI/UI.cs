using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] GameObject _characterUI;
    [SerializeField] GameObject _skillTreeUI;
    [SerializeField] GameObject _craftUI;
    [SerializeField] GameObject _optionUI;
    [SerializeField] GameObject _gameUI;

    [Header("EndScreen")]
    [SerializeField] UI_DarkScreen _fadeScreen;
    [SerializeField] GameObject _endText;
    [SerializeField] GameObject _restartButton;

    bool _allowSFX;

    private void Awake ()
    {
        SwitchToMenu(_skillTreeUI);
        _fadeScreen.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);

        craftWindow.gameObject.SetActive(false);

        SwitchToMenu(_gameUI);
        _allowSFX = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKey(_characterUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKey(_skillTreeUI);

        if(Input.GetKeyDown(KeyCode.B))
            SwitchWithKey(_craftUI);

        if(Input.GetKeyDown(KeyCode.O))
            SwitchWithKey(_optionUI);
    }


    public void SwitchWithKey(GameObject menu)
    {
        if(menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            _gameUI.SetActive(true);
            return;
        }

        SwitchToMenu(menu);
    }

    public void SwitchToMenu(GameObject menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_DarkScreen>() != null;
            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
            if(_allowSFX) AudioManager.instance.PlaySFX("MenuOpen");
        }
        else 
            _gameUI.SetActive(true);

    }

    public void SwitchToEndScreen ()
    {
        _fadeScreen.FadeIn();
        StartCoroutine(EndScreen());
    }

    IEnumerator EndScreen ()
    {
        yield return new WaitForSeconds(1);
        _endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _restartButton.SetActive(true);
    }

    public void RestartGame() => GameManager.instance.RestartScene();
}
