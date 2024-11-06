using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// 序列化 存取文件
/// </summary>
public class DataHandler
{
    string _dataDirPath = "";
    string _dataFileName = "";

    string codeWord = "lwz";

    bool _isEncrypt;

    public DataHandler(string dataDirPath, string dataFileName, bool isEncrypt)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _isEncrypt = isEncrypt;
    }

    public void Save (GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if(_isEncrypt) dataToStore = EncryptDecrypt(dataToStore);

            using FileStream sw = new(fullPath, FileMode.Create);
            using StreamWriter sw2 = new(sw);
            sw2.Write(dataToStore);
        }
        catch (System.Exception)
        {
            Debug.LogError("Errow on save gamedata");
        }
    }

    public GameData Load ()
    {
        string fullPath = Path .Combine(_dataDirPath, _dataFileName);

        GameData loadData = null;

        try
        {
            if(File.Exists(fullPath))
            {
                string dataToLoad = "";
                using FileStream sw = new(fullPath, FileMode.Open);
                using StreamReader sr = new(sw);
                dataToLoad = sr.ReadToEnd();

                if(_isEncrypt) dataToLoad = EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
        }
        catch (System.Exception)
        {
            Debug.LogError("Errow on load gamedata");
        }

        return loadData;
    }

    public void DeleteData ()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)( data[i] ^ codeWord[i % codeWord.Length] );
        }

        return modifiedData;
    }

}
