using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// ����ģʽ���� ��ҪĿ���Ǳ����������� ��������ʵ�ֵ���ģʽ����
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseManager<T> where T:class//,new()
{
    private static T _instance;

    //�жϵ���ģʽ���� �Ƿ�Ϊnull
    protected bool InstanceisNull => _instance == null;

    //���ڼ����Ķ���
    protected static readonly object lockObj = new object();

    //���Եķ�ʽ
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
                        //���÷���õ��޲�˽�еĹ��캯�� �����ڶ����ʵ����
                        Type type = typeof(T);
                        ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null,
                                                                    Type.EmptyTypes,
                                                                    null);
                        if (info != null)
                            _instance = info.Invoke(null) as T;
                        else
                            Debug.LogError("û�еõ���Ӧ���޲ι��캯��");

                        //_instance = Activator.CreateInstance(typeof(T), true) as T;
                    }
                }
            }
            return _instance;
        }
    }
}
