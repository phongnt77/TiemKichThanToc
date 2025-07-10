using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerEndless : MonoBehaviour
{
    public static GameManagerEndless instance;

    [Header("Enemy Prefab Settings")]
    public GameObject enemyPrefab;
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime = 5f;

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
        //if (!isGameOver && isGameStarted)
        //{
        asteroidSpawnTimer += Time.deltaTime;
        if (asteroidSpawnTimer >= asteroidSpawnInterval)
        {
            GetAsteroid();
            asteroidSpawnTimer = 0f;
        }
        //}
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
}
