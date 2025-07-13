using Assets.Scripts.Enless.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless instance;

    [Header("Enemy Prefab Settings")]
    [SerializeField] private ObjectPoolManager enemyPool;
    public Transform enemySpawnPoint;
    public float enemySpeed = 2f;
    private float enemySpawnTimer = 0f;
    public float enemySpawnInterval = 2f;
    private int activeEnemyCount = 0;
    public int maxEnemyOnScreen = 3;

    [Header("Particle Effects")]
    public GameObject explosionEffect;
    public GameObject muzzleFlash;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private GameObject gameOverCanvas;
    //[SerializeField] private int scoreToWin = 500;
    [SerializeField] private TextMeshProUGUI pauseScoreText;
    [SerializeField] private TextMeshProUGUI victoryScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameWinScoreText;

    
    [Header("Notification UI")]
    [SerializeField] private GameObject bossDeathNotification;

    private int score = 0;
    private bool isGameStarted = false;
    private bool isGameOver = false;

    [Header("Boss Settings")]
    [SerializeField] private GameObject[] bossPrefabs;
    public Transform bossSpawnPoint;
    private bool bossSpawned = false;
    private int currentBossIndex = 0;
    private float bossSpawnTimer = 0f;
    private float bossSpawnInterval = 60f;
    private GameObject currentBoss;

    [Header("Asteroid Settings")]
    public Transform asteroidSpawnPoint;
    public float asteroidSpeed = 5f;
    [SerializeField] private ObjectPoolManager asteroidPool;
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

    void Start() 
    { 
        StartGame();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        asteroidSpawnTimer += Time.deltaTime;
        enemySpawnTimer += Time.deltaTime;

        if (asteroidSpawnTimer >= asteroidSpawnInterval)
        {
            GetAsteroid();
            asteroidSpawnTimer = 0f;
        }

        if (!bossSpawned && enemySpawnTimer >= enemySpawnInterval)
        {
            int enemyCount = GetEnemyCountByTime(elapsedTime);
            GetEnemyBatch(enemyCount);
            enemySpawnTimer = 0f;
        }

        if (!bossSpawned)
        {
            bossSpawnTimer += Time.deltaTime;
            if (bossSpawnTimer >= bossSpawnInterval && currentBossIndex < bossPrefabs.Length && activeEnemyCount == 0)
            {
                SpawnBoss();
                bossSpawnTimer = 0f;
            }
        }

        TimeManagement();
    }

    public void StartGame()
    {
        isGameStarted = true;
        score = 0;
        UpdateScoreUI();

        canvasMenu.SetActive(false);
        gameOverCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Debug.Log("Restart called");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
        UpdateScoreUI();
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        isGameStarted = false;
        isGameOver = false;
        score = 0;
        UpdateScoreUI();
        Time.timeScale = 1f;
    }

    public void EndGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        canvasMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        canvasMenu.SetActive(false);
    }

    public void GetAsteroid()
    {
        Asteriod asteroid = asteroidPool.GetObject().GetComponent<Asteriod>();
        asteroid.asteroidPool = asteroidPool;
        asteroid.transform.position = asteroidSpawnPoint.position;
    }

    public void GetEnemyBatch(int number)
    {
        Vector3 origin = enemySpawnPoint.position;
        SpawnPattern pattern = GetRandomPattern();

        int canSpawn = Mathf.Min(number, maxEnemyOnScreen - activeEnemyCount);

        for (int i = 0; i < canSpawn; i++)
        {
            EnemyControllerEndless enemy = enemyPool.GetObject().GetComponent<EnemyControllerEndless>();
            enemy.enemyPool = enemyPool;
            enemy.OnEnemyDie = OnEnemyDieHandler;
            enemy.transform.position = GetSpawnPosition(pattern, origin, i, canSpawn);
            activeEnemyCount++;
        }
    }

    private Vector3 GetSpawnPosition(SpawnPattern pattern, Vector3 origin, int index, int total)
    {
        switch (pattern)
        {
            case SpawnPattern.ZigZag:
                float zigX = origin.x + index * 5f;
                float zigY = origin.y + ((index % 2 == 0) ? 2f : -2f);
                return new Vector3(zigX, zigY, origin.z);
            case SpawnPattern.VFormation:
                int half = total / 2;
                int offset = index - half;
                float vX = origin.x + offset * 5.5f;
                float vY = origin.y - Mathf.Abs(offset) * 4f;
                return new Vector3(vX, vY, origin.z);
            default:
                float x = origin.x + index * 7.5f;
                return new Vector3(x, origin.y, origin.z);
        }
    }

    private SpawnPattern GetRandomPattern()
    {
        float rand = Random.value;
        if (rand < 0.33f) return SpawnPattern.Default;
        else if (rand < 0.66f) return SpawnPattern.ZigZag;
        else return SpawnPattern.VFormation;
    }

    private void OnEnemyDieHandler()
    {
        activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
    }

    private int GetEnemyCountByTime(float time)
    {
        int count = 3 + Mathf.FloorToInt(time / 30f);
        return Mathf.Clamp(count, 3, 6);
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

    public int GetScore() => score;

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            UpdateScoreUI();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null) scoreText.text = "" + score;
        if (pauseScoreText != null) pauseScoreText.text = "" + score;
        if (gameOverScoreText != null) gameOverScoreText.text = "" + score;
    }

    private void SpawnBoss()
    {
        if (currentBossIndex >= bossPrefabs.Length) return;

        GameObject boss = Instantiate(bossPrefabs[currentBossIndex], bossSpawnPoint.position, Quaternion.identity);
        BossControllerEndless bossController = boss.GetComponent<BossControllerEndless>();
        bossController.OnBossDie = OnBossDieHandler;
        bossSpawned = true;
        currentBoss = boss;
    }

    private void OnBossDieHandler()
    {
        Debug.Log($"Boss {currentBossIndex + 1} defeated!");
        AddScore(100);
        if (bossDeathNotification != null)
        {
            bossDeathNotification.SetActive(true);
            Invoke(nameof(HideBossNotification), 2f); // Ẩn sau 2 giây
        }

        currentBossIndex++;
        bossSpawned = false;
        currentBoss = null;

        if (currentBossIndex >= 3)
        {
            Time.timeScale = 0f;
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
            }
        }
    }

    private void HideBossNotification()
    {
        if (bossDeathNotification != null)
            bossDeathNotification.SetActive(false);
    }

}

public enum SpawnPattern
{
    Default,
    ZigZag,
    VFormation
}