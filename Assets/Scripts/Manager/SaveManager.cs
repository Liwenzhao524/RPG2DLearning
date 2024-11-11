using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    GameData _gameData;

    /// <summary>
    /// 所有含需保存数据（即继承ISaveManager接口）的物体
    /// </summary>
    List<ISaveManager> _saveList;
    DataHandler _dataHandler;
    [SerializeField] string _saveFileName = "SaveData.txt";
    [SerializeField] bool _isEcrypt;

    private void Awake ()
    {
        if (instance == null)
            instance = this;
    }

    private void Start ()
    {
        _saveList = FindAllSave();
        _dataHandler = new(Application.persistentDataPath, _saveFileName, _isEcrypt);
        LoadGame();
    }

    [ContextMenu("Delete Savedata")]
    public void DeleteSaveData ()
    {
        _dataHandler = new(Application.persistentDataPath, _saveFileName, _isEcrypt);
        _dataHandler.DeleteData();
    }

    public bool HasSavedData() => _dataHandler.Load() != null;

    public void NewGame ()
    {
        _gameData = new GameData();
    }

    public void LoadGame ()
    {
        _gameData = _dataHandler.Load();

        if(_gameData == null)
        {
            Debug.Log("No Game Data Found");
            NewGame();
        }

        foreach (ISaveManager s in _saveList)
        {
            s.LoadGame(_gameData);
        }

    }

    public void SaveGame ()
    {
        foreach (ISaveManager s in _saveList)
        {
            s.SaveGame(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit ()
    {
        SaveGame();
    }

    List<ISaveManager> FindAllSave ()
    {
        IEnumerable<ISaveManager> save = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

        return new List<ISaveManager> (save);
    }
}
