using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    // 存未使用的
    private Stack<GameObject> dataStack = new Stack<GameObject>();

    // 存正使用的
    private List<GameObject> usedList = new List<GameObject>();

    // 根对象 布局管理
    private GameObject rootObj;

    // 获取容器中可用对象数量
    public int Count => dataStack.Count;

    public int UsedCount => usedList.Count;
    public int MaxNum { get; set; }

    /// <summary>
    /// 初始化构造函数
    /// </summary>
    /// <param name="root">缓存池父对象</param>
    /// <param name="name">父对象的名字</param>
    public PoolData(GameObject root, string name, GameObject usedObj, int initialMax)
    {
        MaxNum = initialMax;
        if (PoolManager.isOpenLayout)
        {
            rootObj = new GameObject(name);
            // 自动设置父子关系
            rootObj.transform.SetParent(root.transform);
        }

        PushUsedList(usedObj);
    }

    /// <summary>
    /// 弹出对象
    /// </summary>
    public GameObject Pop()
    {
        GameObject obj;

        // 未使用->正使用
        if (Count > 0)
        {
            obj = dataStack.Pop();
            usedList.Add(obj);
        }
        // 正使用->未使用
        else
        {
            obj = usedList[0];
            usedList.RemoveAt(0);
            usedList.Add(obj);
        }

        obj.SetActive(true);

        if (PoolManager.isOpenLayout)
            obj.transform.SetParent(null);

        return obj;
    }

    /// <summary>
    /// 放入对象
    /// </summary>
    public void Push(GameObject obj)
    {
        obj.SetActive(false);

        if (PoolManager.isOpenLayout)
            obj.transform.SetParent(rootObj.transform);
        // 通过栈记录对应的对象数据
        dataStack.Push(obj);
        // 不再使用 从记录容器中移除
        usedList.Remove(obj);
    }

    /// <summary>
    /// 压入到正使用容器
    /// </summary>
    /// <param name="obj"></param>
    public void PushUsedList(GameObject obj)
    {
        usedList.Add(obj);
    }
}

/// <summary>
/// 缓存池(对象池)模块 管理器
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    //根对象
    private GameObject poolObj;

    public static bool isOpenLayout = false;

    private PoolManager() { }

    /// <summary>
    /// 拿东西的方法
    /// </summary>
    /// <param name="name">抽屉容器的名字</param>
    /// <returns>从缓存池中取出的对象</returns>
    public GameObject GetObj(string name, int initNum = 10)
    {
        GameObject obj;

        if(!poolDic.ContainsKey(name) ||
            (poolDic[name].Count == 0 && poolDic[name].UsedCount < initNum))
        {
            obj = CreateNewObj(name);

            if (!poolDic.ContainsKey(name))
                poolDic.Add(name, new PoolData(poolObj, name, obj, initNum));
            else
                poolDic[name].PushUsedList(obj);
        }

        PoolData poolData = poolDic[name];
        // 没有可用对象
        if (poolData.Count == 0)
        {
            // 动态扩容
            if (poolData.UsedCount >= poolData.MaxNum)
            {
                poolData.MaxNum *= 2;
            }
            // TODO：长时间不用考虑回退长度？
            obj = CreateNewObj(name);
            poolData.PushUsedList(obj);
        }
        // 数量超上限 
        else
        {
            obj = poolDic[name].Pop();
        }

        return obj;
    }

    private static GameObject CreateNewObj (string name)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        // 重命名
        obj.name = name;
        return obj;
    }


    /// <summary>
    /// 放入对象
    /// </summary>
    /// <param name="name">抽屉（对象）的名字</param>
    /// <param name="obj">希望放入的对象</param>
    public void PushObj(GameObject obj)
    {
        if (poolObj == null && isOpenLayout)
            poolObj = new GameObject("Pool");

        poolDic[obj.name].Push(obj);

    }

    /// <summary>
    /// 切场景时清除数据 
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
