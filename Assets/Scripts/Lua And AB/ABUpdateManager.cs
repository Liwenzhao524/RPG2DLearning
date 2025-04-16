using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ABUpdateManager : MonoBehaviour
{
    private static ABUpdateManager instance;

    public static ABUpdateManager Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject("ABUpdateManager");
                instance = obj.AddComponent<ABUpdateManager>();
            }
            return instance;
        }
    }

    // 存储远端AB包信息 和本地进行对比
    private Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();

    // 存储本地AB包信息 和远端信息对比
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    // 待下载列表
    private List<string> downLoadList = new List<string>();

    /// <summary>
    /// 检测是否热更新
    /// </summary>
    /// <param name="overCallBack"></param>
    /// <param name="updateInfoCallBack"></param>
    public void CheckUpdate(UnityAction<bool> overCallBack, UnityAction<string> updateInfoCallBack)
    {
        remoteABInfo.Clear();
        localABInfo.Clear();
        downLoadList.Clear();

        // 加载远端资源对比文件
        DownLoadABCompareFile((isOver) =>
        {
            if (isOver)
            {
                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");
;
                GetRemoteABCompareFileInfo(remoteInfo, remoteABInfo);

                // 加载本地资源对比文件
                GetLocalABCompareFileInfo((isOver)=> {
                    if (isOver)
                    {
                        // 对比并下载AB包
                        foreach (string abName in remoteABInfo.Keys)
                        {
                            // 本地没有同名AB包
                            if (!localABInfo.ContainsKey(abName))
                                downLoadList.Add(abName);
                            // 本地有同名AB包
                            else
                            {
                                // 对比md5码 判断是否需要更新
                                if (localABInfo[abName].md5 != remoteABInfo[abName].md5)
                                    downLoadList.Add(abName);
                                // 移除本地中远端没有的内容
                                localABInfo.Remove(abName);
                            }
                        }
                        // 下载待更新列表中的所有AB包
                        DownLoadABFile((isOver) =>
                        {
                            if (isOver)
                            {
                                //更新本地对比文件
                                File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", remoteInfo);
                            }
                        }, updateInfoCallBack);
                    }
                });
            }
        });
    }

    /// <summary>
    /// 下载AB包对比文件
    /// </summary>
    /// <param name="overCallBack"></param>
    public async void DownLoadABCompareFile(UnityAction<bool> overCallBack)
    {
        bool isOver = false;
        int reDownLoadMaxNum = 5;
        string localPath = Application.persistentDataPath;
        while (!isOver && reDownLoadMaxNum > 0)
        {
            await Task.Run(() => {
                isOver = DownLoadFile("ABCompareInfo.txt", localPath + "/ABCompareInfo_TMP.txt");
            });
            --reDownLoadMaxNum;
        }

        // 回调
        overCallBack?.Invoke(isOver);
    }

    /// <summary>
    /// 解析远端AB包对比文件信息
    /// </summary>
    public void GetRemoteABCompareFileInfo(string info, Dictionary<string, ABInfo> ABInfo)
    {
        string[] strs = info.Split('|');
        string[] infos = null;
        for (int i = 0; i < strs.Length; i++)
        {
            infos = strs[i].Split(' ');
            // 记录每一个远端AB包的信息 用来对比
            ABInfo.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));
        }
    }

    /// <summary>
    /// 解析本地AB包对比文件信息
    /// </summary>
    public void GetLocalABCompareFileInfo(UnityAction<bool> overCallBack)
    {
        StartCoroutine(GetLocalABCompareFileInfo(Application.persistentDataPath + "/ABCompareInfo.txt", overCallBack));

        overCallBack(true);
    }

    private IEnumerator GetLocalABCompareFileInfo(string filePath, UnityAction<bool> overCallBack)
    {
        //通过 UnityWebRequest 去加载本地文件
        UnityWebRequest req = UnityWebRequest.Get(filePath);
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.Success)
        {
            GetRemoteABCompareFileInfo(req.downloadHandler.text, localABInfo);
            overCallBack(true);
        }
        else
            overCallBack(false);
    }

    /// <summary>
    /// 下载待下载列表中的AB包文件
    /// </summary>
    /// <param name="overCallBack"></param>
    /// <param name="updatePro"></param>
    public async void DownLoadABFile(UnityAction<bool> overCallBack, UnityAction<string> updatePro)
    {
        string localPath = Application.persistentDataPath + "/";
        bool isOver = false;

        // 记录已下载的文件
        List<string> tempList = new List<string>();
        // 重下载的最大次数
        int reDownLoadMaxNum = 5;
        int downLoadOverNum = 0;
        int downLoadMaxNum = downLoadList.Count;

        while (downLoadList.Count > 0 && reDownLoadMaxNum > 0)
        {
            for (int i = 0; i < downLoadList.Count; i++)
            {
                isOver = false;
                await Task.Run(() => {
                    isOver = DownLoadFile(downLoadList[i], localPath + downLoadList[i]);
                });
                if (isOver)
                {
                    updatePro(++downLoadOverNum + "/" +downLoadMaxNum);
                    //下载成功记录下来
                    tempList.Add(downLoadList[i]);
                }
            }
            for (int i = 0; i < tempList.Count; i++)
                downLoadList.Remove(tempList[i]);

            --reDownLoadMaxNum;
        }

        // 回调 告诉外部是否下载完成
        overCallBack(downLoadList.Count == 0);
    }

    /// <summary>
    /// FTP下载文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localPath"></param>
    /// <returns></returns>
    private bool DownLoadFile(string fileName, string localPath)
    {
        try
        {
            // 同上传 创建FTP连接
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.50.49/" + fileName)) as FtpWebRequest;
            NetworkCredential n = new NetworkCredential("LWZ", "LWZ123");

            req.Credentials = n;
            req.Proxy = null;
            req.KeepAlive = false;
            // 此处设置为下载
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;

            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downLoadStream = res.GetResponseStream();
            using (FileStream file = File.Create(localPath))
            {
                byte[] bytes = new byte[2048];
                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);

                while (contentLength != 0)
                {
                    file.Write(bytes, 0, contentLength);
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }

                file.Close();
                downLoadStream.Close();

                return true;
            }
        }
        catch (Exception ex)
        {
            print(fileName + "下载失败" + ex.Message);
            return false;
        }

    }


    private void OnDestroy()
    {
        instance = null;
    }

    /// <summary>
    /// AB包信息类
    /// </summary>
    public class ABInfo
    {
        public string name;
        public long size;
        public string md5;

        public ABInfo(string name, string size, string md5)
        {
            this.name = name;
            this.size = long.Parse(size);
            this.md5 = md5;
        }
    }
}


