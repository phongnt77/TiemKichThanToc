using Assets.Scripts.Enless.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless instance;

    [Header("Enemy Prefab Settings")]
    [SerializeField]
    private ObjectPoolManager enemyPool;
    public Transform enemySpawnPoint;
    //public float enemyDestroyTime = 5f;
    public float enemySpeed = 2f;
    private float enemySpawnTimer = 0f;
    public float enemySpawnInterval = 2f; // Thời gian giữa các lần spawn enemy
    private int activeEnemyCount = 0;
    public int maxEnemyOnScreen = 3; 

    [Header("Particle Effects")]
    public GameObject explosionEffect;
    public GameObject muzzleFlash;

    [Header("Panels (Drag from Hierarchy)")]
    public GameObject PauseMenu;
    public GameObject GameOverMenu;

    [Header("Score UI (Drag from Hierarchy)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI pauseScoreText;
    public TextMeshProUGUI gameOverScoreText;

    private int score = 0;
    private bool isGameStarted = false;
    private bool isGameOver = false;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    private bool bossSpawned = false;

    [Header("Asteroid Settings")]
    //public GameObject asteroidPrefab;
    public Transform asteroidSpawnPoint;
    public float asteroidSpeed = 5f;
    [SerializeField]
    private ObjectPoolManager asteroidPool;
    public float asteroidSpawnInterval = 2.0f;
    private float asteroidSpawnTimer = 0.0f;

    [Header("Timer Settings")]
    private float elapsedTime = 0f;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "SceneEndLess")
        {
            Destroy(gameObject);
            return;
        }
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Cập nhật thời gian chơi
        elapsedTime += Time.deltaTime;

        // Asteroid logic giữ nguyên
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer >= asteroidSpawnInterval)
        {
            GetAsteroid();
            asteroidSpawnTimer = 0f;
        }

        // Enemy spawn logic mới
        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer >= enemySpawnInterval)
        {
            int enemyCount = GetEnemyCountByTime(elapsedTime);
            GetEnemyBatch(enemyCount);
            enemySpawnTimer = 0f;
        }

        //quản lý thời gian
        TimeManagement();
    }

    public void StartGame()
    {
        isGameStarted = true;
        score = 0;
        //UpdateScoreUI();
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        Time.timeScale = 1f; // Resume game time
    }

    public void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f; // Pause game time
        GameOverMenu.SetActive(true);
        //UpdateGameOverUI();
    }

    public void PauseGame()
    {
        if (isGameStarted && !isGameOver)
        {
            Time.timeScale = 0f; // Pause game time
            PauseMenu.SetActive(true);
            //UpdatePauseScoreUI();
        }
    }

    public void GetAsteroid()
    {
        Asteriod asteroid = asteroidPool.GetObject().GetComponent<Asteriod>();
        asteroid.asteroidPool = asteroidPool;
        asteroid.transform.position = asteroidSpawnPoint.position;
    }

    public void GetEnemyBatch(int number)
    {
        for (int i = 0; i < number; i++)
        {
            if (activeEnemyCount >= maxEnemyOnScreen)
                break;

            EnemyControllerEndless enemy = enemyPool.GetObject().GetComponent<EnemyControllerEndless>();
            enemy.enemyPool = enemyPool;

            enemy.transform.position = enemySpawnPoint.position;
            enemy.transform.position += new Vector3(i * 2f, 0f, 0f); // Tạo khoảng cách giữa các enemy
            activeEnemyCount++;
            enemy.OnEnemyDie = OnEnemyDieHandler; // Gán callback khi enemy die
        }
    }

    private int GetEnemyCountByTime(float time)
    {
        int count = 3 + Mathf.FloorToInt(time / 30f); // Bắt đầu từ 3, tăng dần
        return Mathf.Clamp(count, 3, 10); // Giới hạn từ 3 đến 10
    }

    public void TimeManagement()
    {
        if (timerText != null)
        {
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void OnEnemyDieHandler()
    {
        activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
    }
}
