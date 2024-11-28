using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// 单例模式基类 主要目的是避免代码的冗余 方便我们实现单例模式的类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseManager<T> where T:class//,new()
{
    private static T _instance;

    //判断单例模式对象 是否为null
    protected bool InstanceisNull => _instance == null;

    //用于加锁的对象
    protected static readonly object lockObj = new object();

    //属性的方式
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
                        //_instance = new T();
                        //利用反射得到无参私有的构造函数 来用于对象的实例化
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null);
                        if (info != null)
                            _instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("没有得到对应的无参构造函数");

                        //_instance = Activator.CreateInstance(typeof(T), true) as T;
                    }
                }
            }
            return _instance;
        }
    }
}
