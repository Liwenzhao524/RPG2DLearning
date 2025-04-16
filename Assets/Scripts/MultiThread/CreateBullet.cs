using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBullet : MonoBehaviour
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public float minSpeed = 5f;
    public float maxSpeed = 10f;
    public int bulletCount = 100;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnBullet_Custom();
        }
    }

    void SpawnBullet_Custom ()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 设置随机运动
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                float randomSpeed = Random.Range(minSpeed, maxSpeed);
                rb.velocity = randomDirection * randomSpeed;
            }

            // 10秒后销毁
            Destroy(bullet, 10f);
        }
    }
}
