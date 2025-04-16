using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class TransformJobs : MonoBehaviour
{
    public bool useJob;
    public bool useTask;
    public int dataCount = 100;
    public int batchCount = 4; // Task����
    // ���ڴ洢transform��NativeArray
    private TransformAccessArray m_TransformsAccessArray;
    private NativeArray<Vector3> m_Velocities;
    private List<Vector3> m_Positions;

    private PositionUpdateJob m_Job;
    private JobHandle m_PositionJobHandle;
    private GameObject[] sphereGameObjects;
    //[BurstCompile]
    struct PositionUpdateJob : IJobParallelForTransform
    {
        // ��ÿ����������һ���ٶ�
        [ReadOnly]
        public NativeArray<Vector3> velocity;

        public float deltaTime;

        // ʵ��IJobParallelForTransform�Ľṹ����Execute�����ڶ����������Ի�ȡ��Transform
        public void Execute (int i, TransformAccess transform)
        {
            transform.position += velocity[i] * deltaTime;
        }
    }

    void Start ()
    {
        m_Velocities = new NativeArray<Vector3>(dataCount, Allocator.Persistent);

        // �ô�������һ������,��Ϊ���Ƶ�ģ��
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // �ر���Ӱ ��ײ��
        var renderer = sphere.GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        var collider = sphere.GetComponent<Collider>();
        collider.enabled = false;

        // ����transform������,��������transform��Native Array
        var transforms = new Transform[dataCount];
        sphereGameObjects = new GameObject[dataCount];
        int row = (int)Mathf.Sqrt(dataCount);
        // ����1W����
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < row; j++)
            {
                var go = GameObject.Instantiate(sphere);
                Vector3 position = new Vector3(j - 6, i - 4, 0);
                go.transform.position = position;
                sphereGameObjects[i * row + j] = go;
                transforms[i * row + j] = go.transform;
                m_Velocities[i * row + j] = new Vector3(0.5f * j, 0.5f * j, 0);
                if(useTask)
                    m_Positions[i * row + j] = position;
            }
        }
        Destroy(sphere);
        m_TransformsAccessArray = new TransformAccessArray(transforms);
    }

    void Update ()
    {
        float startTime = Time.realtimeSinceStartup;
        if (useJob)
        {
            // ʵ����һ��job,��������
            m_Job = new PositionUpdateJob()
            {
                deltaTime = Time.deltaTime,
                velocity = m_Velocities,
            };

            m_PositionJobHandle = m_Job.Schedule(m_TransformsAccessArray);
            Debug.Log(("Use Job:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        }
        // Task����ֱ�Ӳ���Transform��ѡ�����ÿ֡Position�ٸ�ֵ
        else if (useTask)
        {
            float deltaTime = Time.deltaTime;
            int actualBatchCount = Mathf.Clamp(batchCount, 1, dataCount);
            int batchSize = dataCount / actualBatchCount;
            List<Task> tasks = new List<Task>();

            // ���м�����λ��
            for (int batch = 0; batch < actualBatchCount; batch++)
            {
                int start = batch * batchSize;
                int end = ( batch == actualBatchCount - 1 ) ? dataCount : start + batchSize;
                tasks.Add(Task.Run(() =>
                {
                    for (int i = start; i < end; i++)
                    {
                        m_Positions[i] += m_Velocities[i] * deltaTime;
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray()); 

            for (int i = 0; i < dataCount; i++)
            {
                sphereGameObjects[i].transform.position = m_Positions[i];
            }

            Debug.Log(("Use Task:" + ( Time.realtimeSinceStartup - startTime ) * 1000f) + "ms");
        }
        else
        {
            for (int i = 0; i < dataCount; ++i)
            {
                sphereGameObjects[i].transform.position += m_Velocities[i] * Time.deltaTime;
            }
            Debug.Log(("Not Use Job:"+ (Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
        }

    }

    // ��֤��ǰ֡��Jobִ�����
    private void LateUpdate ()
    {
        m_PositionJobHandle.Complete();
    }

    // OnDestroy���ͷ�NativeArray���ڴ�
    private void OnDestroy ()
    {
        if (m_TransformsAccessArray.isCreated)
            m_TransformsAccessArray.Dispose();
        if (m_Velocities.IsCreated)
            m_Velocities.Dispose();
    }
}
