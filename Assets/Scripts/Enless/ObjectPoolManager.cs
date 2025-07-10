using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://viblo.asia/p/toi-uu-voi-object-pooling-pattern-trong-unity-EoW4oRyBVml
/// Sử dụng link trên để tìm hiểu cho object pooling trong unity kết hợp sử dụng singleton pattern
/// </summary>

public class ObjectPoolManager : MonoBehaviour
{
    //public static ObjectPoolManager Instance { get; private set; }

    public GameObject objectPrefab;
    public int poolSize = 20;
    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        //if (Instance == null) Instance = this;
        //else Destroy(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // Có thể mở rộng pool nếu cần
            var obj = Instantiate(objectPrefab, transform.position, Quaternion.identity);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
