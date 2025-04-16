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

    // �洢Զ��AB����Ϣ �ͱ��ؽ��жԱ�
    private Dictionary<string, ABInfo> remoteABInfo = new Dictionary<string, ABInfo>();

    // �洢����AB����Ϣ ��Զ����Ϣ�Ա�
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    // �������б�
    private List<string> downLoadList = new List<string>();

    /// <summary>
    /// ����Ƿ��ȸ���
    /// </summary>
    /// <param name="overCallBack"></param>
    /// <param name="updateInfoCallBack"></param>
    public void CheckUpdate(UnityAction<bool> overCallBack, UnityAction<string> updateInfoCallBack)
    {
        remoteABInfo.Clear();
        localABInfo.Clear();
        downLoadList.Clear();

        // ����Զ����Դ�Ա��ļ�
        DownLoadABCompareFile((isOver) =>
        {
            if (isOver)
            {
                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");
;
                GetRemoteABCompareFileInfo(remoteInfo, remoteABInfo);

                // ���ر�����Դ�Ա��ļ�
                GetLocalABCompareFileInfo((isOver)=> {
                    if (isOver)
                    {
                        // �ԱȲ�����AB��
                        foreach (string abName in remoteABInfo.Keys)
                        {
                            // ����û��ͬ��AB��
                            if (!localABInfo.ContainsKey(abName))
                                downLoadList.Add(abName);
                            // ������ͬ��AB��
                            else
                            {
                                // �Ա�md5�� �ж��Ƿ���Ҫ����
                                if (localABInfo[abName].md5 != remoteABInfo[abName].md5)
                                    downLoadList.Add(abName);
                                // �Ƴ�������Զ��û�е�����
                                localABInfo.Remove(abName);
                            }
                        }
                        // ���ش������б��е�����AB��
                        DownLoadABFile((isOver) =>
                        {
                            if (isOver)
                            {
                                //���±��ضԱ��ļ�
                                File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt", remoteInfo);
                            }
                        }, updateInfoCallBack);
                    }
                });
            }
        });
    }

    /// <summary>
    /// ����AB���Ա��ļ�
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

        // �ص�
        overCallBack?.Invoke(isOver);
    }

    /// <summary>
    /// ����Զ��AB���Ա��ļ���Ϣ
    /// </summary>
    public void GetRemoteABCompareFileInfo(string info, Dictionary<string, ABInfo> ABInfo)
    {
        string[] strs = info.Split('|');
        string[] infos = null;
        for (int i = 0; i < strs.Length; i++)
        {
            infos = strs[i].Split(' ');
            // ��¼ÿһ��Զ��AB������Ϣ �����Ա�
            ABInfo.Add(infos[0], new ABInfo(infos[0], infos[1], infos[2]));
        }
    }

    /// <summary>
    /// ��������AB���Ա��ļ���Ϣ
    /// </summary>
    public void GetLocalABCompareFileInfo(UnityAction<bool> overCallBack)
    {
        StartCoroutine(GetLocalABCompareFileInfo(Application.persistentDataPath + "/ABCompareInfo.txt", overCallBack));

        overCallBack(true);
    }

    private IEnumerator GetLocalABCompareFileInfo(string filePath, UnityAction<bool> overCallBack)
    {
        //ͨ�� UnityWebRequest ȥ���ر����ļ�
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
    /// ���ش������б��е�AB���ļ�
    /// </summary>
    /// <param name="overCallBack"></param>
    /// <param name="updatePro"></param>
    public async void DownLoadABFile(UnityAction<bool> overCallBack, UnityAction<string> updatePro)
    {
        string localPath = Application.persistentDataPath + "/";
        bool isOver = false;

        // ��¼�����ص��ļ�
        List<string> tempList = new List<string>();
        // �����ص�������
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
                    //���سɹ���¼����
                    tempList.Add(downLoadList[i]);
                }
            }
            for (int i = 0; i < tempList.Count; i++)
                downLoadList.Remove(tempList[i]);

            --reDownLoadMaxNum;
        }

        // �ص� �����ⲿ�Ƿ��������
        overCallBack(downLoadList.Count == 0);
    }

    /// <summary>
    /// FTP�����ļ�
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="localPath"></param>
    /// <returns></returns>
    private bool DownLoadFile(string fileName, string localPath)
    {
        try
        {
            // ͬ�ϴ� ����FTP����
            FtpWebRequest req = FtpWebRequest.Create(new Uri("ftp://192.168.50.49/" + fileName)) as FtpWebRequest;
            NetworkCredential n = new NetworkCredential("LWZ", "LWZ123");

            req.Credentials = n;
            req.Proxy = null;
            req.KeepAlive = false;
            // �˴�����Ϊ����
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
            print(fileName + "����ʧ��" + ex.Message);
            return false;
        }

    }


    private void OnDestroy()
    {
        instance = null;
    }

    /// <summary>
    /// AB����Ϣ��
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


