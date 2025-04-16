using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class UploadAB
{
#if UNITY_EDITOR
    [MenuItem("AB For HotFix/Upload AB")]
#endif
    private static void UploadAllABFile()
    {
        // 获取文件夹信息
        DirectoryInfo directory = Directory.CreateDirectory(Application.dataPath + "/Resources/AB");
        FileInfo[] fileInfos = directory.GetFiles();

        foreach (FileInfo info in fileInfos)
        {
            if (info.Extension == "" ||
                info.Extension == ".txt")
            {
                FtpUploadFile(info.FullName, info.Name);
            }
        }
    }

    private async static void FtpUploadFile(string filePath, string fileName)
    {
        await Task.Run(() =>
        {
            try
            {
                // 创建FTP连接 设置通信凭证
                FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://10.102.1.38/" + fileName)) as FtpWebRequest;
                NetworkCredential n = new NetworkCredential("LWZ", "LWZ123");
                // FTP设置
                req.Credentials = n;
                req.Proxy = null;
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.UseBinary = true;
                // FTP流
                Stream upLoadStream = req.GetRequestStream();
                // 文件流写入
                using (FileStream file = File.OpenRead(filePath))
                {
                    byte[] bytes = new byte[2048];
                    int contentLength = file.Read(bytes, 0, bytes.Length);

                    while (contentLength != 0)
                    {
                        // 写入到上传流
                        upLoadStream.Write(bytes, 0, contentLength);
                        contentLength = file.Read(bytes, 0, bytes.Length);
                    }
                    file.Close();
                    upLoadStream.Close();
                }

                Debug.Log(fileName + "上传成功");
            }
            catch (Exception ex)
            {
                Debug.Log(fileName + "上传失败" + ex.Message);
            }
        });
       
    }
}
