using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
   public GameObject objectToSpawn; // The object to spawn
    public float spawnInterval = 2.0f; // Time interval between spawns
    private float spawnTimer = 0.0f; // Timer to track spawn intervals

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnObject();
            spawnTimer = 0.0f;
        }
    }

    private void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
