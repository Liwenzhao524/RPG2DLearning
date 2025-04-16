using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 单例模式基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseManager<T> : MonoBehaviour where T:class
{
    private static T _instance;

    protected bool InstanceisNull => _instance == null;

    protected static readonly object lockObj = new();

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                lock (lockObj)
                {
                    if (_instance == null)
                    {
                        //利用反射得到无参私有的构造函数 来用于对象的实例化
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null);
                        if (info != null)
                            _instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("没有对应的构造函数");
                    }
                }
            }
            return _instance;
        }
    }
}
