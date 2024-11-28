using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����Monoģ�������
/// </summary>
public class MonoMgr : SingletonAutoMono<MonoMgr>
{
    private event UnityAction _updateEvent;
    private event UnityAction _fixedUpdateEvent;
    private event UnityAction _lateUpdateEvent;

    /// <summary>
    /// ���Update֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddUpdateListener(UnityAction updateFun)
    {
        _updateEvent += updateFun;
    }

    /// <summary>
    /// �Ƴ�Update֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void RemoveUpdateListener(UnityAction updateFun)
    {
        _updateEvent -= updateFun;
    }

    /// <summary>
    /// ���FixedUpdate֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddFixedUpdateListener(UnityAction updateFun)
    {
        _fixedUpdateEvent += updateFun;
    }
    /// <summary>
    /// �Ƴ�FixedUpdate֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void RemoveFixedUpdateListener(UnityAction updateFun)
    {
        _fixedUpdateEvent -= updateFun;
    }

    /// <summary>
    /// ���LateUpdate֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddLateUpdateListener(UnityAction updateFun)
    {
        _lateUpdateEvent += updateFun;
    }

    /// <summary>
    /// �Ƴ�LateUpdate֡���¼�������
    /// </summary>
    /// <param name="updateFun"></param>
    public void RemoveLateUpdateListener(UnityAction updateFun)
    {
        _lateUpdateEvent -= updateFun;
    }


    private void Update()
    {
        _updateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        _fixedUpdateEvent?.Invoke();
    }

    private void LateUpdate()
    {
        _lateUpdateEvent?.Invoke();
    }
}
