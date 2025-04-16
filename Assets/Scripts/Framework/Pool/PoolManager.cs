using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    // ��δʹ�õ�
    private Stack<GameObject> dataStack = new Stack<GameObject>();

    // ����ʹ�õ�
    private List<GameObject> usedList = new List<GameObject>();

    // ������ ���ֹ���
    private GameObject rootObj;

    // ��ȡ�����п��ö�������
    public int Count => dataStack.Count;

    public int UsedCount => usedList.Count;
    public int MaxNum { get; set; }

    /// <summary>
    /// ��ʼ�����캯��
    /// </summary>
    /// <param name="root">����ظ�����</param>
    /// <param name="name">�����������</param>
    public PoolData(GameObject root, string name, GameObject usedObj, int initialMax)
    {
        MaxNum = initialMax;
        if (PoolManager.isOpenLayout)
        {
            rootObj = new GameObject(name);
            // �Զ����ø��ӹ�ϵ
            rootObj.transform.SetParent(root.transform);
        }

        PushUsedList(usedObj);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public GameObject Pop()
    {
        GameObject obj;

        // δʹ��->��ʹ��
        if (Count > 0)
        {
            obj = dataStack.Pop();
            usedList.Add(obj);
        }
        // ��ʹ��->δʹ��
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
    /// �������
    /// </summary>
    public void Push(GameObject obj)
    {
        obj.SetActive(false);

        if (PoolManager.isOpenLayout)
            obj.transform.SetParent(rootObj.transform);
        // ͨ��ջ��¼��Ӧ�Ķ�������
        dataStack.Push(obj);
        // ����ʹ�� �Ӽ�¼�������Ƴ�
        usedList.Remove(obj);
    }

    /// <summary>
    /// ѹ�뵽��ʹ������
    /// </summary>
    /// <param name="obj"></param>
    public void PushUsedList(GameObject obj)
    {
        usedList.Add(obj);
    }
}

/// <summary>
/// �����(�����)ģ�� ������
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    //������
    private GameObject poolObj;

    public static bool isOpenLayout = false;

    private PoolManager() { }

    /// <summary>
    /// �ö����ķ���
    /// </summary>
    /// <param name="name">��������������</param>
    /// <returns>�ӻ������ȡ���Ķ���</returns>
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
        // û�п��ö���
        if (poolData.Count == 0)
        {
            // ��̬����
            if (poolData.UsedCount >= poolData.MaxNum)
            {
                poolData.MaxNum *= 2;
            }
            // TODO����ʱ�䲻�ÿ��ǻ��˳��ȣ�
            obj = CreateNewObj(name);
            poolData.PushUsedList(obj);
        }
        // ���������� 
        else
        {
            obj = poolDic[name].Pop();
        }

        return obj;
    }

    private static GameObject CreateNewObj (string name)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        // ������
        obj.name = name;
        return obj;
    }


    /// <summary>
    /// �������
    /// </summary>
    /// <param name="name">���루���󣩵�����</param>
    /// <param name="obj">ϣ������Ķ���</param>
    public void PushObj(GameObject obj)
    {
        if (poolObj == null && isOpenLayout)
            poolObj = new GameObject("Pool");

        poolDic[obj.name].Push(obj);

    }

    /// <summary>
    /// �г���ʱ������� 
    /// </summary>
    public void ClearPool()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
