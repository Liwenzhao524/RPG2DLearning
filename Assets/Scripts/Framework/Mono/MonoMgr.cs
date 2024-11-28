using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 公共Mono模块管理器
/// </summary>
public class MonoMgr : SingletonAutoMono<MonoMgr>
{
    private event UnityAction _updateEvent;
    private event UnityAction _fixedUpdateEvent;
    private event UnityAction _lateUpdateEvent;

    /// <summary>
    /// 添加Update帧更新监听函数
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddUpdateListener(UnityAction updateFun)
    {
        _updateEvent += updateFun;
    }

    /// <summary>
    /// 移除Update帧更新监听函数
    /// </summary>
    /// <param name="updateFun"></param>
    public void RemoveUpdateListener(UnityAction updateFun)
    {
        _updateEvent -= updateFun;
    }

    /// <summary>
    /// 添加FixedUpdate帧更新监听函数
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddFixedUpdateListener(UnityAction updateFun)
    {
        _fixedUpdateEvent += updateFun;
    }
    /// <summary>
    /// 移除FixedUpdate帧更新监听函数
    /// </summary>
    /// <param name="updateFun"></param>
    public void RemoveFixedUpdateListener(UnityAction updateFun)
    {
        _fixedUpdateEvent -= updateFun;
    }

    /// <summary>
    /// 添加LateUpdate帧更新监听函数
    /// </summary>
    /// <param name="updateFun"></param>
    public void AddLateUpdateListener(UnityAction updateFun)
    {
        _lateUpdateEvent += updateFun;
    }

    /// <summary>
    /// 移除LateUpdate帧更新监听函数
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
