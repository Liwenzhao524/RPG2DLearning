using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���루�����е����ݣ�����
/// </summary>
public class PoolData
{
    //�����洢�����еĶ��� ��¼����û��ʹ�õĶ���
    Stack<GameObject> _dataStack = new();

    //������¼ʹ���еĶ���� 
    List<GameObject> _usedList = new();

    //�������� ������ͬʱ���ڵĶ�������޸���
    int _maxNum;

    //��������� �������в��ֹ���Ķ���
    GameObject _rootObj;

    //��ȡ�������Ƿ��ж���
    public int Count => _dataStack.Count;

    public int UsedCount => _usedList.Count;

    /// <summary>
    /// ����ʹ���ж�������������������бȽ� С�ڷ���true ��Ҫʵ����
    /// </summary>
    public bool NeedCreate => _usedList.Count < _maxNum;

    /// <summary>
    /// ��ʼ�����캯��
    /// </summary>
    /// <param name="root">���ӣ�����أ�������</param>
    /// <param name="name">���븸���������</param>
    public PoolData(GameObject root, string name, GameObject usedObj)
    {
        //��������ʱ �Żᶯ̬���� �������ӹ�ϵ
        if(PoolMgr.isOpenLayout)
        {
            //�������븸����
            _rootObj = new GameObject(name);
            //�͹��Ӹ����������ӹ�ϵ
            _rootObj.transform.SetParent(root.transform);
        }

        //��������ʱ �ⲿ�϶��ǻᶯ̬����һ�������
        //����Ӧ�ý����¼�� ʹ���еĶ���������
        PushUsedList(usedObj);

        PoolObj poolObj = usedObj.GetComponent<PoolObj>();
        if (poolObj == null)
        {
            Debug.LogError("��Ϊʹ�û���ع��ܵ�Ԥ����������PoolObj�ű� ����������������");
            return;
        }
        //��¼��������ֵ
        _maxNum = poolObj.maxNum;
    }

    /// <summary>
    /// �ӳ����е������ݶ���
    /// </summary>
    /// <returns>��Ҫ�Ķ�������</returns>
    public GameObject Pop()
    {
        //ȡ������
        GameObject obj;

        if (Count > 0)
        {
            //��û�е���������ȡ��ʹ��
            obj = _dataStack.Pop();
            //����Ҫʹ���� Ӧ��Ҫ��ʹ���е�������¼��
            _usedList.Add(obj);
        }
        else
        {
            //ȡ0�����Ķ��� ����ľ���ʹ��ʱ����Ķ���
            obj = _usedList[0];
            //���Ұ�����ʹ���ŵĶ������Ƴ�
            _usedList.RemoveAt(0);
            //��������Ҫ�ó�ȥ�ã���������Ӧ�ð����ּ�¼�� ʹ���е�������ȥ 
            //������ӵ�β�� ��ʾ �Ƚ��µĿ�ʼ
            _usedList.Add(obj);
        }

        //�������
        obj.SetActive(true);
        //�Ͽ����ӹ�ϵ
        if (PoolMgr.isOpenLayout)
            obj.transform.SetParent(null);

        return obj;
    }

    /// <summary>
    /// ��������뵽���������
    /// </summary>
    /// <param name="obj"></param>
    public void Push(GameObject obj)
    {
        //ʧ��������Ķ���
        obj.SetActive(false);
        //�����Ӧ����ĸ������� �������ӹ�ϵ
        if (PoolMgr.isOpenLayout)
            obj.transform.SetParent(_rootObj.transform);
        //ͨ��ջ��¼��Ӧ�Ķ�������
        _dataStack.Push(obj);
        //��������Ѿ�����ʹ���� Ӧ�ð����Ӽ�¼�������Ƴ�
        _usedList.Remove(obj);
    }


    /// <summary>
    /// ������ѹ�뵽ʹ���е������м�¼
    /// </summary>
    /// <param name="obj"></param>
    public void PushUsedList(GameObject obj)
    {
        _usedList.Add(obj);
    }
}

/// <summary>
/// �������ֵ䵱������ʽ�滻ԭ�� �洢�������
/// </summary>
public abstract class PoolObjectBase { }

/// <summary>
/// ���ڴ洢 ���ݽṹ�� �� �߼��� �����̳�mono�ģ�������
/// </summary>
/// <typeparam name="T"></typeparam>
public class PoolObject<T> : PoolObjectBase where T : class
{
    public Queue<T> poolObjs = new();
}

/// <summary>
/// ��Ҫ�����õ� ���ݽṹ�ࡢ�߼��� ������Ҫ�̳иýӿ�
/// </summary>
public interface IPoolObject
{
    /// <summary>
    /// �������ݵķ���
    /// </summary>
    void ResetInfo();
}

/// <summary>
/// �����(�����)ģ�� ������
/// </summary>
public class PoolMgr : BaseManager<PoolMgr>
{
    //�������������г��������
    //ֵ ��ʵ����ľ���һ�� �������
    Dictionary<string, PoolData> _poolDic = new();

    /// <summary>
    /// ���ڴ洢���ݽṹ�ࡢ�߼������� ���ӵ��ֵ�����
    /// </summary>
    Dictionary<string, PoolObjectBase> _poolObjectDic = new();

    //���Ӹ�����
    GameObject _poolObj;

    //�Ƿ������ֹ���
    public static bool isOpenLayout = true;

    private PoolMgr() {

        //���������Ϊ�� �ʹ���
        if (_poolObj == null && isOpenLayout)
            _poolObj = new GameObject("Pool");

    }

    /// <summary>
    /// �ö����ķ���
    /// </summary>
    /// <param name="name">��������������</param>
    /// <returns>�ӻ������ȡ���Ķ���</returns>
    public GameObject GetObj(string name)
    {
        //���������Ϊ�� �ʹ���
        if (_poolObj == null && isOpenLayout)
            _poolObj = new GameObject("Pool");

        GameObject obj;

        #region �������������޺���߼��ж�
        if(!_poolDic.ContainsKey(name) ||
            (_poolDic[name].Count == 0 && _poolDic[name].NeedCreate))
        {
            //��̬��������
            //û�е�ʱ�� ͨ����Դ���� ȥʵ������һ��GameObject
            obj = Object.Instantiate(Resources.Load<GameObject>(name));
            //����ʵ���������Ķ��� Ĭ�ϻ������ֺ����һ��(Clone)
            //�������������� �����������
            obj.name = name;

            //��������
            if(!_poolDic.ContainsKey(name))
                _poolDic.Add(name, new PoolData(_poolObj, name, obj));
            else//ʵ���������Ķ��� ��Ҫ��¼��ʹ���еĶ���������
                _poolDic[name].PushUsedList(obj);
        }
        //���������ж��� ���� ʹ���еĶ��������� ֱ��ȥȡ������
        else
        {
            obj = _poolDic[name].Pop();
        }

        #endregion


        #region û�м��� ����ʱ���߼�
        ////�г��� ���� ������ �ж��� ��ȥֱ����
        //if (_poolDic.ContainsKey(name) && _poolDic[name].Count > 0)
        //{
        //    //����ջ�еĶ��� ֱ�ӷ��ظ��ⲿʹ��
        //    obj = _poolDic[name].Pop();
        //}
        ////���򣬾�Ӧ��ȥ����
        //else
        //{
        //    //û�е�ʱ�� ͨ����Դ���� ȥʵ������һ��GameObject
        //    obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
        //    //����ʵ���������Ķ��� Ĭ�ϻ������ֺ����һ��(Clone)
        //    //�������������� �����������
        //    obj.name = name;
        //}
        #endregion
        return obj;
    }

    /// <summary>
    /// ��ȡ�Զ�������ݽṹ����߼������ �����̳�Mono�ģ�
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <returns></returns>
    public T GetObj<T>(string nameSpace = "") where T:class,IPoolObject,new()
    {
        //���ӵ����� �Ǹ������������������ ������������
        string poolName = nameSpace + "_" + typeof(T).Name;
        //�г���
        if(_poolObjectDic.ContainsKey(poolName))
        {
            PoolObject<T> pool = _poolObjectDic[poolName] as PoolObject<T>;
            //���ӵ����Ƿ��п��Ը��õ�����
            if(pool.poolObjs.Count > 0)
            {
                //�Ӷ�����ȡ������ ���и���
                T obj = pool.poolObjs.Dequeue() as T;
                return obj;
            }
            //���ӵ����ǿյ�
            else
            {
                //���뱣֤�����޲ι��캯��
                T obj = new();
                return obj;
            }
        }
        else//û�г���
        {
            T obj = new();
            return obj;
        }
        
    }

    /// <summary>
    /// ��������з������
    /// </summary>
    /// <param name="name">���루���󣩵�����</param>
    /// <param name="obj">ϣ������Ķ���</param>
    public void PushObj(GameObject obj)
    {
        #region ��Ϊʧ�� ���ӹ�ϵ�������� ��������д��� ���Բ���Ҫ�ٴ�����Щ������
        ////��֮��Ŀ�ľ���Ҫ�Ѷ�����������
        ////������ֱ���Ƴ����� ���ǽ�����ʧ�� һ������� �õ�ʱ���ټ�����
        ////�������ַ�ʽ�������԰Ѷ���ŵ���Ļ�⿴�����ĵط�
        //obj.SetActive(false);

        ////��ʧ��Ķ���Ҫ��������еĶ��� ������������Ϊ ���ӣ�����أ�������
        //obj.transform.SetParent(_poolObj.transform);
        #endregion

        //û�г��� ��������
        //if (!_poolDic.ContainsKey(obj.name))
        //    _poolDic.Add(obj.name, new PoolData(_poolObj, obj.name));

        //�����뵱�зŶ���
        _poolDic[obj.name].Push(obj);

        ////������ڶ�Ӧ�ĳ������� ֱ�ӷ�
        //if(_poolDic.ContainsKey(name))
        //{
        //    //��ջ�����룩�з������
        //    _poolDic[name].Push(obj);
        //}
        ////���� ��Ҫ�ȴ������� �ٷ�
        //else
        //{
        //    //�ȴ�������
        //    _poolDic.Add(name, new Stack<GameObject>());
        //    //�������������
        //    _poolDic[name].Push(obj);
        //}
    }

    /// <summary>
    /// ���Զ������ݽṹ����߼��� ���������
    /// </summary>
    /// <typeparam name="T">��Ӧ����</typeparam>
    public void PushObj<T>(T obj, string nameSpace = "") where T:class,IPoolObject
    {
        //�����Ҫѹ��null���� �ǲ��������
        if (obj == null)
            return;
        //���ӵ����� �Ǹ������������������ ������������
        string poolName = nameSpace + "_" + typeof(T).Name;
        //�г���
        PoolObject<T> pool;
        if (_poolObjectDic.ContainsKey(poolName))
            //ȡ������ ѹ�����
            pool = _poolObjectDic[poolName] as PoolObject<T>;
        else//û�г���
        {
            pool = new PoolObject<T>();
            _poolObjectDic.Add(poolName, pool);
        }
        //�ڷ��������֮ǰ �����ö��������
        obj.ResetInfo();
        pool.poolObjs.Enqueue(obj);
    }

    /// <summary>
    /// ��������������ӵ��е����� 
    /// ʹ�ó��� ��Ҫ�� �г���ʱ
    /// </summary>
    public void ClearPool()
    {
        _poolDic.Clear();
        _poolObj = null;
        _poolObjectDic.Clear();
    }
}
