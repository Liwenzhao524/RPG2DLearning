using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载式 继承Mono的单例模式基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMono<T>: MonoBehaviour where T:MonoBehaviour
{
    private static T _instance;

    public static T instance
    {
        get
        {
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this as T;

        //DontDestroyOnLoad(gameObject);
    }
}
