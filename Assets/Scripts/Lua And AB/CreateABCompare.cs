using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

public class CreateABCompare
{
#if UNITY_EDITOR
    [MenuItem("AB For HotFix/Create Compare File")]
#endif
    public static void CreateABCompareFile()
    {
        // �ļ�
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/Resources/AB");
        FileInfo[] fileInfos = directory.GetFiles();

        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            if(info.Extension == "")
            {
                //Debug.Log("�ļ�����" + info.Name);
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                
                abCompareInfo += '|';
            }
            //Debug.Log("�ļ�����" + info.Name);
            //Debug.Log("�ļ�·����" + info.FullName);
            //Debug.Log("�ļ���׺��" + info.Extension);
            //Debug.Log("�ļ���С��" + info.Length);
        }
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);

        //Debug.Log(abCompareInfo);

        // �洢�Ա��ļ���Ϣ
        File.WriteAllText(Application.dataPath + "/Resources/AB/ABCompareInfo.txt", abCompareInfo);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private static string GetMD5(string filePath)
    {
        // �ļ�����using���
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            // ����MD5��
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Info = md5.ComputeHash(file);

            file.Close();

            // �����ַ���
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
                sb.Append(md5Info[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
