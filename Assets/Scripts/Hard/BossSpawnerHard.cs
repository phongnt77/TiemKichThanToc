using UnityEngine;
using UnityEngine.UI;

// Script này dùng để sinh (spawn) hai boss trong chế độ Hard của game.
// Boss 1 xuất hiện sau 1 phút kể từ khi bắt đầu game.
// Boss 2 xuất hiện sau 2 phút, nhưng chỉ bắt đầu hoạt động khi Boss 1 đã chết.
public class BossSpawnerHard : MonoBehaviour
{
    // Tham chiếu đến prefab của Boss 1 (kéo thả trong Inspector)
    [SerializeField] private GameObject bossPrefab;
    // Tham chiếu đến prefab của Boss 2 (kéo thả trong Inspector)
    [SerializeField] private GameObject bossPrefab2;
    [Header("Health Bar Prefabs & Canvas")]
    public GameObject bossHealthBarPrefab1; // Prefab thanh máu cho Boss 1
    public GameObject bossHealthBarPrefab2; // Prefab thanh máu cho Boss 2
    public Canvas canvas;
    [Header("Enemy Spawner")]
    public EnemySpawnerHard enemySpawner; // Kéo EnemySpawnerHard vào đây

    // Lưu trữ instance của Boss 1 và Boss 2 sau khi được sinh ra
    private GameObject boss1Instance;
    private GameObject boss2Instance;
    private GameObject boss2HealthBarInstance;
    // Đánh dấu đã sinh boss 2 hay chưa
    private bool boss2Spawned = false;

    void Start()
    {
        // Sinh Boss 1 sau 1 phút (60 giây)
        Invoke(nameof(SpawnBoss1), 10f);
        // Sinh Boss 2 sau 2 phút (120 giây), nhưng sẽ bị vô hiệu hóa
        Invoke(nameof(SpawnBoss2), 20f);
    }

    // Hàm này dùng để sinh Boss 1 tại vị trí phía trên màn hình
    void SpawnBoss1()
    {
        Camera cam = Camera.main;
        // Tính toán vị trí ở giữa phía trên màn hình (theo toạ độ thế giới)
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 10f));
        topCenter.y -= 1f;
        topCenter.z = 0f;
        // Tạo ra Boss 1
        boss1Instance = Instantiate(bossPrefab, topCenter, Quaternion.identity);
        // Spawn health bar cho boss 1
        if (bossHealthBarPrefab1 != null && canvas != null)
        {
            GameObject healthBar1 = Instantiate(bossHealthBarPrefab1, canvas.transform);
            Slider slider1 = healthBar1.GetComponent<Slider>();
            boss1Instance.GetComponent<BossHardController>().healthSlider = slider1;
        }
        // Tăng độ khó khi Boss 1 xuất hiện
        if (enemySpawner != null)
        {
            enemySpawner.IncreaseDifficulty();
        }
        Debug.Log("Boss 1 đã được sinh ra");
    }

    // Hàm này dùng để sinh Boss 2 tại vị trí phía trên màn hình, nhưng sẽ bị vô hiệu hóa ngay khi sinh ra
    void SpawnBoss2()
    {
        Camera cam = Camera.main;
        Vector3 topCenter = cam.ViewportToWorldPoint(new Vector3(0.5f, 1f, cam.nearClipPlane + 10f));
        topCenter.y -= 1f;
        topCenter.z = 0f;
        // Tạo ra Boss 2 và set inactive (chưa hoạt động)
        boss2Instance = Instantiate(bossPrefab2, topCenter, Quaternion.identity);
        boss2Instance.SetActive(false); // Chỉ hoạt động khi boss 1 chết
        // Spawn health bar cho boss 2 và set inactive
        if (bossHealthBarPrefab2 != null && canvas != null)
        {
            boss2HealthBarInstance = Instantiate(bossHealthBarPrefab2, canvas.transform);
            boss2HealthBarInstance.SetActive(false);
            Slider slider2 = boss2HealthBarInstance.GetComponent<Slider>();
            boss2Instance.GetComponent<BossHardController>().healthSlider = slider2;
        }
        // Tăng độ khó khi Boss 2 xuất hiện
        if (enemySpawner != null)
        {
            enemySpawner.IncreaseDifficulty();
        }
        boss2Spawned = true;
        Debug.Log("Boss 2 đã được sinh ra");
    }

    void Update()
    {
        // Khi boss 1 chết, kích hoạt boss 2 và thanh máu của boss 2 (nếu đã spawn)
        if (boss2Spawned && boss2Instance != null && boss1Instance == null && !boss2Instance.activeSelf)
        {
            boss2Instance.SetActive(true);
            if (boss2HealthBarInstance != null)
                boss2HealthBarInstance.SetActive(true);
            Debug.Log("Boss 2 và thanh máu đã được kích hoạt");
        }
    }
}
