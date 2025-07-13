using UnityEngine;

public class ItemSpawnerController : MonoBehaviour
{
    [SerializeField] private ObjectPoolManager itemPool;
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private float spawnY = 6f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    private void SpawnItem()
    {
        BoostHPItem item = itemPool.GetObject().GetComponent<BoostHPItem>();
        item.pool = itemPool; 
        // Tạo vị trí ngẫu nhiên trong viewport (X trong khoảng 0.1 đến 0.9)
        float randomX = Random.Range(0.1f, 0.9f);
        Vector3 spawnWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(randomX, 1f, 0));
        spawnWorldPos.z = 0f;
        spawnWorldPos.y = spawnY;

        item.transform.position = spawnWorldPos;
    }
}
