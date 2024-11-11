using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] string _sceneName = "MainScene";
    [SerializeField] GameObject _continueButton;
    [SerializeField] UI_DarkScreen _darkScreen;

    private void Start ()
    {
        if(SaveManager.instance.HasSavedData() == false)
            _continueButton.SetActive(false);
    }

    public void ContinueGame ()
    {
        StartCoroutine(LoadGameWithDarkScreen(1.5f));
    }

    public void NewGame ()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadGameWithDarkScreen(1.5f));
    }

    public void ExitGame ()
    {
        Debug.Log("Exit");
        //Application.Quit();
    }

    IEnumerator LoadGameWithDarkScreen (float delay)
    {
        _darkScreen.FadeIn();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(_sceneName);
    }
}
