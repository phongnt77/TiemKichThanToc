using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _miniumSpawnTime = 0.3f;
    [SerializeField] private float _maxSpawnTime = 0.5f;

    private float _timeUntilSpawn;

    private void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0)
        {
            SpawnEnemyAtTop();
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_miniumSpawnTime, _maxSpawnTime);
    }

    private void SpawnEnemyAtTop()
    {
        float randomX = Random.Range(0.09f, 0.95f);
        float spawnY = 1.2f;

        Vector3 viewportPos = new Vector3(randomX, spawnY, Camera.main.nearClipPlane + 10f);
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(viewportPos);
        spawnPosition.z = 0f;

        GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);

        enemy.AddComponent<AutoDestroyWhenOffscreen>();
    }

    public class AutoDestroyWhenOffscreen : MonoBehaviour
    {
        void Update()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.y < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

