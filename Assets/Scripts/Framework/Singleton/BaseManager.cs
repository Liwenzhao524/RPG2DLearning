using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ����ģʽ����
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
                        //���÷���õ��޲�˽�еĹ��캯�� �����ڶ����ʵ����
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null);
                        if (info != null)
                            _instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("û�ж�Ӧ�Ĺ��캯��");
                    }
                }
            }
            return _instance;
        }
    }
}
