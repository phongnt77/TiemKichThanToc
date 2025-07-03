using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "ScenceHard")
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

    private void Start()
    {
        if (PauseMenu != null) PauseMenu.SetActive(false);
        if (GameOverMenu != null) GameOverMenu.SetActive(false);

        if (!pauseScoreText || !gameOverScoreText || !scoreText)
            Debug.LogWarning("Một số Text UI chưa được gán trong GameManager!");

        if (!enemyPrefab || !explosionEffect || !muzzleFlash)
            Debug.LogWarning("Một số Prefab chưa được gán trong GameManager!");

        UpdateScoreUI();

        // Bắt đầu game luôn
        StartGameButton();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameButton(true);
        }

        if (!bossSpawned && GetScore() >= 10)
        {
            Debug.Log("Spawning boss at score: " + GetScore());
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
            bossSpawned = true;
        }
    }

    void InstantiateEnemy()
    {
        if (enemyPrefab == null || !isGameStarted || isGameOver) return;

        Vector3 enemyPos = new Vector3(Random.Range(-5f, 5f), 6f, 0f);
        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity);
        Destroy(enemy, enemyDestroyTime);
    }

    public void StartGameButton()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
        Time.timeScale = 1f;
        isGameStarted = true;
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
    }

    public void PauseGameButton(bool isPaused)
    {
        if (isPaused)
        {
            if (PauseMenu != null) PauseMenu.SetActive(true);
            if (scoreText != null) scoreText.gameObject.SetActive(false);
            Time.timeScale = 0f;
            UpdateScoreUI();
        }
        else
        {
            if (PauseMenu != null) PauseMenu.SetActive(false);
            if (scoreText != null) scoreText.gameObject.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            score += points;
            UpdateScoreUI();
        }
    }

    public int GetScore() => score;

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        if (GameOverMenu != null) GameOverMenu.SetActive(true);
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        Time.timeScale = 0f;
        CancelInvoke("InstantiateEnemy");
        UpdateScoreUI();
    }

    public void RestartGame()
    {
        isGameStarted = false;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (pauseScoreText != null)
            pauseScoreText.text = "Score: " + score;
        if (gameOverScoreText != null)
            gameOverScoreText.text = "Score: " + score;
    }
}
