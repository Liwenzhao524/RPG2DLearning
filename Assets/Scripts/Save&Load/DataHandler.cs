using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

/// <summary>
/// ���л� ��ȡ�ļ�
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

    public void SaveByBF(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // BinaryFormatter���л�
            BinaryFormatter formatter = new BinaryFormatter();
            string dataToStore;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                formatter.Serialize(memoryStream, data);
                byte[] bytes = memoryStream.ToArray();
                dataToStore = Convert.ToBase64String(bytes); // תΪBase64�ַ���
            }

            if (_isEncrypt)
                dataToStore = EncryptDecrypt(dataToStore); 

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(dataToStore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Save Error: {e.Message}");
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

    public GameData LoadByBF ()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadData = null;

        try
        {
            if (File.Exists(fullPath))
            {
                string dataToLoad;
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    dataToLoad = reader.ReadToEnd();
                }

                if (_isEncrypt)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                // Base64���벢�����л�
                byte[] bytes = Convert.FromBase64String(dataToLoad);
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    loadData = (GameData)formatter.Deserialize(memoryStream);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Load Error: {e.Message}");
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
