using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePoolData
{
    private Stack<GameObject> dataStack = new Stack<GameObject>();
    private List<GameObject> usedList = new List<GameObject>();
    private GameObject rootObj;
    // ¼ÇÂ¼Image³ß´ç
    private Dictionary<GameObject, Vector2> sizeDict = new Dictionary<GameObject, Vector2>();

    public int Count => dataStack.Count;
    public int UsedCount => usedList.Count;
    public int MaxNum { get; set; }

    public ImagePoolData (GameObject root, string name, GameObject usedObj, int initialMax)
    {
        MaxNum = initialMax;
        if (ImagePoolManager.isOpenLayout)
        {
            rootObj = new GameObject(name);
            rootObj.transform.SetParent(root.transform);
        }
        PushUsedList(usedObj);
    }

    public GameObject Pop ()
    {
        GameObject obj;

        if (Count > 0)
        {
            obj = dataStack.Pop();
            usedList.Add(obj);
        }
        else
        {
            obj = usedList[0];
            usedList.RemoveAt(0);
            usedList.Add(obj);
        }

        RestoreSize(obj);
        if (ImagePoolManager.isOpenLayout)
            obj.transform.SetParent(null);

        return obj;
    }

    public void Push (GameObject obj)
    {
        StoreSize(obj);
        if (ImagePoolManager.isOpenLayout)
            obj.transform.SetParent(rootObj.transform);

        dataStack.Push(obj);
        usedList.Remove(obj);
    }

    public void PushUsedList (GameObject obj)
    {
        StoreSize(obj);
        usedList.Add(obj);
    }

    /// <summary>
    /// »Ö¸´³ß´ç
    /// </summary>
    /// <param name="obj"></param>
    private void StoreSize (GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt != null && !sizeDict.ContainsKey(obj))
        {
            sizeDict[obj] = rt.sizeDelta;
        }
        rt.sizeDelta = Vector2.zero;
    }

    /// <summary>
    /// ´æÍ¼Æ¬Ô­³ß´ç
    /// </summary>
    /// <param name="obj"></param>
    private void RestoreSize (GameObject obj)
    {
        if (sizeDict.TryGetValue(obj, out Vector2 size))
        {
            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.sizeDelta = size;
        }
    }
}

public class ImagePoolManager : BaseManager<ImagePoolManager>
{
    private Dictionary<string, ImagePoolData> poolDic = new Dictionary<string, ImagePoolData>();
    private GameObject poolObj;
    public static bool isOpenLayout = false;

    public GameObject GetImage (string name, int initialMax = 50)
    {
        GameObject obj;

        if (!poolDic.ContainsKey(name))
        {
            obj = CreateNewImage(name);
            poolDic.Add(name, new ImagePoolData(poolObj, name, obj, initialMax));
            return obj;
        }

        ImagePoolData poolData = poolDic[name];
        if (poolData.Count == 0)
        {
            if (poolData.UsedCount >= poolData.MaxNum)
            {
                poolData.MaxNum *= 2;
            }
            obj = CreateNewImage(name);
            poolData.PushUsedList(obj);
        }
        else
        {
            obj = poolData.Pop();
        }

        return obj;
    }

    private GameObject CreateNewImage (string name)
    {
        GameObject newObj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        newObj.name = name;
        Image img = newObj.GetComponent<Image>();
        return newObj;
    }

    public void PushImage (GameObject obj)
    {
        if (poolObj == null && isOpenLayout)
            poolObj = new GameObject("ImagePool");

        string key = obj.name;
        if (poolDic.ContainsKey(key))
        {
            poolDic[key].Push(obj);
        }
    }

    public void ClearPool ()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
