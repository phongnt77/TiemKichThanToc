using UnityEngine;

public class BossSpawnerHard : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;

    void Start()
    {
        Invoke(nameof(SpawnBoss), 30f);
    }

    void SpawnBoss()
    {
        Camera cam = Camera.main;
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 10f));
        topCenter.y -= 1f;
        topCenter.z = 0f;

        Instantiate(bossPrefab, topCenter, Quaternion.identity);
    }
}
