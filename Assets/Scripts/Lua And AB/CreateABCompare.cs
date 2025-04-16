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
        // 文件
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/Resources/AB");
        FileInfo[] fileInfos = directory.GetFiles();

        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            if(info.Extension == "")
            {
                //Debug.Log("文件名：" + info.Name);
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                
                abCompareInfo += '|';
            }
            //Debug.Log("文件名：" + info.Name);
            //Debug.Log("文件路径：" + info.FullName);
            //Debug.Log("文件后缀：" + info.Extension);
            //Debug.Log("文件大小：" + info.Length);
        }
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);

        //Debug.Log(abCompareInfo);

        // 存储对比文件信息
        File.WriteAllText(Application.dataPath + "/Resources/AB/ABCompareInfo.txt", abCompareInfo);
#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }

    private static string GetMD5(string filePath)
    {
        // 文件流和using配合
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            // 生成MD5码
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Info = md5.ComputeHash(file);

            file.Close();

            // 返回字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
                sb.Append(md5Info[i].ToString("x2"));

            return sb.ToString();
        }
    }
}
