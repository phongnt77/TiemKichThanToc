using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://viblo.asia/p/toi-uu-voi-object-pooling-pattern-trong-unity-EoW4oRyBVml
/// Sử dụng link trên để tìm hiểu cho object pooling trong unity kết hợp sử dụng singleton pattern
/// </summary>

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    public GameObject bulletPrefab;
    public int poolSize = 20;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetBullet()
    {
        if (pool.Count > 0)
        {
            var bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            // Có thể mở rộng pool nếu cần
            var bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
    }
}
