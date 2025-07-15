using UnityEngine;
using UnityEngine.UI;

// Script này dùng ?? sinh (spawn) hai boss trong ch? ?? Hard c?a game.
// Boss 1 xu?t hi?n sau 1 phút k? t? khi b?t ??u game.
// Boss 2 xu?t hi?n sau 2 phút, nh?ng ch? b?t ??u ho?t ??ng khi Boss 1 ?ã ch?t.
public class BossSpawnerHard : MonoBehaviour
{
    // Tham chi?u ??n prefab c?a Boss 1 (kéo th? trong Inspector)
    [SerializeField] private GameObject bossPrefab;
    // Tham chi?u ??n prefab c?a Boss 2 (kéo th? trong Inspector)
    [SerializeField] private GameObject bossPrefab2;
    [Header("Health Bar Prefabs & Canvas")]
    public GameObject bossHealthBarPrefab1; // Prefab thanh máu cho Boss 1
    public GameObject bossHealthBarPrefab2; // Prefab thanh máu cho Boss 2
    public Canvas canvas;
    [Header("Enemy Spawner")]
    public EnemySpawnerHard enemySpawner; // Kéo EnemySpawnerHard vào ?ây

    // L?u tr? instance c?a Boss 1 và Boss 2 sau khi ???c sinh ra
    private GameObject boss1Instance;
    private GameObject boss2Instance;
    private GameObject boss2HealthBarInstance;
    // ?ánh d?u ?ã sinh boss 2 hay ch?a
    private bool boss2Spawned = false;

    void Start()
    {
        // Sinh Boss 1 sau 1 phút (60 giây)
        Invoke(nameof(SpawnBoss1), 10f);
        // Sinh Boss 2 sau 2 phút (120 giây), nh?ng s? b? vô hi?u hóa
        Invoke(nameof(SpawnBoss2), 20f);
    }

    // Hàm này dùng ?? sinh Boss 1 t?i v? trí phía trên màn hình
    void SpawnBoss1()
    {
        Camera cam = Camera.main;
        // Tính toán v? trí ? gi?a phía trên màn hình (theo to? ?? th? gi?i)
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 10f));
        topCenter.y -= 1f;
        topCenter.z = 0f;
        // T?o ra Boss 1
        boss1Instance = Instantiate(bossPrefab, topCenter, Quaternion.identity);
        // Spawn health bar cho boss 1
        if (bossHealthBarPrefab1 != null && canvas != null)
        {
            GameObject healthBar1 = Instantiate(bossHealthBarPrefab1, canvas.transform);
            Slider slider1 = healthBar1.GetComponent<Slider>();
            boss1Instance.GetComponent<BossHardController>().healthSlider = slider1;
        }
        // T?ng ?? khó khi Boss 1 xu?t hi?n
        if (enemySpawner != null)
        {
            enemySpawner.IncreaseDifficulty();
        }
        Debug.Log("Boss 1 ?ã ???c sinh ra");
    }

    // Hàm này dùng ?? sinh Boss 2 t?i v? trí phía trên màn hình, nh?ng s? b? vô hi?u hóa ngay khi sinh ra
    void SpawnBoss2()
    {
        Camera cam = Camera.main;
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 10f));
        topCenter.y -= 1f;
        topCenter.z = 0f;
        // T?o ra Boss 2 và set inactive (ch?a ho?t ??ng)
        boss2Instance = Instantiate(bossPrefab2, topCenter, Quaternion.identity);
        boss2Instance.SetActive(false); // Ch? ho?t ??ng khi boss 1 ch?t
        // Spawn health bar cho boss 2 và set inactive
        if (bossHealthBarPrefab2 != null && canvas != null)
        {
            boss2HealthBarInstance = Instantiate(bossHealthBarPrefab2, canvas.transform);
            boss2HealthBarInstance.SetActive(false);
            Slider slider2 = boss2HealthBarInstance.GetComponent<Slider>();
            boss2Instance.GetComponent<BossHardController>().healthSlider = slider2;
        }
        // T?ng ?? khó khi Boss 2 xu?t hi?n
        if (enemySpawner != null)
        {
            enemySpawner.IncreaseDifficulty();
        }
        boss2Spawned = true;
        Debug.Log("Boss 2 ?ã ???c sinh ra");
    }

    void Update()
    {
        // Khi boss 1 ch?t, kích ho?t boss 2 và thanh máu c?a boss 2 (n?u ?ã spawn)
        if (boss2Spawned && boss2Instance != null && boss1Instance == null && !boss2Instance.activeSelf)
        {
            boss2Instance.SetActive(true);
            if (boss2HealthBarInstance != null)
                boss2HealthBarInstance.SetActive(true);
            Debug.Log("Boss 2 và thanh máu ?ã ???c kích ho?t");
        }
    }
}
