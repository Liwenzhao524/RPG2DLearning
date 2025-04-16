using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 知识点
/// 1.AB包相关的API
/// 2.单例模式
/// 3.委托——>Lambda表达式
/// 4.协程
/// 5.字典
/// </summary>
public class ABManager : BaseManager<ABManager>
{
    // 主包
    private AssetBundle mainAB = null;
    // 依赖 配置文件
    private AssetBundleManifest manifest = null;
    // 避免重复
    private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    private string Path
    {
        get
        {
            return Application.streamingAssetsPath + "/";
        }
    }

    private string MainABName
    {
        get
        {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
            return "PC";
#endif
        }
    }

    /// <summary>
    /// 加载AB包
    /// </summary>
    /// <param name="abName"></param>
    public void LoadAB( string abName )
    {
        // 加载AB包
        if (mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(Path + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        // 获取依赖相关信息
        AssetBundle ab = null;
        string[] strs = manifest.GetAllDependencies(abName);
        // 加载依赖
        for (int i = 0; i < strs.Length; i++)
        {
            if (!abDic.ContainsKey(strs[i]))
            {
                ab = AssetBundle.LoadFromFile(Path + strs[i]);
                abDic.Add(strs[i], ab);
            }
        }
        // 加载主包
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(Path + abName);
            abDic.Add(abName, ab);
        }
    }

    //  同步加载
    public Object LoadRes(string abName, string resName)
    {
        LoadAB(abName);
        Object obj = abDic[abName].LoadAsset(resName);
        if (obj is GameObject)
            return GameObject.Instantiate(obj);
        else
            return obj;
    }
    // 根据泛型指定类型
    public T LoadRes<T>(string abName, string resName) where T:Object
    {
        LoadAB(abName);

        T obj = abDic[abName].LoadAsset<T>(resName);
        if (obj is GameObject)
            return GameObject.Instantiate(obj);
        else
            return obj;
    }
    
    // 异步加载
    public void LoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        StartCoroutine(ReallyLoadResAsync(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync(string abName, string resName, UnityAction<Object> callBack)
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync(resName);
        yield return abr;

        // 加载结束后 通过委托 传递给外部
        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset));
        else
            callBack(abr.asset);
    }

    // 根据泛型 异步加载
    public void LoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T:Object
    {
        StartCoroutine(ReallyLoadResAsync<T>(abName, resName, callBack));
    }
    private IEnumerator ReallyLoadResAsync<T>(string abName, string resName, UnityAction<T> callBack) where T : Object
    {
        LoadAB(abName);
        AssetBundleRequest abr = abDic[abName].LoadAssetAsync<T>(resName);
        yield return abr;

        if (abr.asset is GameObject)
            callBack(Instantiate(abr.asset) as T);
        else
            callBack(abr.asset as T);
    }

    // 卸载
    public void UnLoad(string abName)
    {
        if( abDic.ContainsKey(abName) )
        {
            abDic[abName].Unload(false);
            abDic.Remove(abName);
        }
    }

    //所有包卸载
    public void ClearAB()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        abDic.Clear();
        mainAB = null;
        manifest = null;
    }
}
