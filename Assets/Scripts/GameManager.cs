using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Enemy Prefab Settings")]
    public GameObject enemyPrefab; // Đây là Prefab, cần kéo từ Project window
    public float minInstantiateValue;
    public float maxInstantiateValue;
    public float enemyDestroyTime = 5f;

    [Header("Particle Effects")]
    public GameObject explosionEffect; // Đây là Prefab, cần kéo từ Project window
    public GameObject muzzleFlash;     // Đây là Prefab, cần kéo từ Project window

    [Header("Panels (Drag from Hierarchy)")]
    public GameObject StartMenu;       // Đây là GameObject trong scene, kéo từ Hierarchy
    public GameObject PauseMenu;       // Đây là GameObject trong scene, kéo từ Hierarchy
    public GameObject GameOverMenu;    // Đây là GameObject trong scene, kéo từ Hierarchy

    [Header("Score UI (Drag from Hierarchy)")]
    public TextMeshProUGUI scoreText;             // Đây là Text trong scene, kéo từ Hierarchy
    public TextMeshProUGUI pauseScoreText;        // Đây là Text trong scene, kéo từ Hierarchy
    public TextMeshProUGUI gameOverScoreText;     // Đây là Text trong scene, kéo từ Hierarchy

    private int score = 0;
    private bool isGameStarted = false; // Khai báo và gán giá trị ban đầu
    private bool isGameOver = false;    // Khai báo và gán giá trị ban đầu

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Đảm bảo chỉ có một GameManager trong scene
        }
    }

    private void Start()
    {
        // Kiểm tra các trường bắt buộc
        if (!StartMenu || !PauseMenu || !GameOverMenu)
        {
            Debug.LogError("Một hoặc nhiều panel (StartMenu, PauseMenu, GameOverMenu) chưa được gán trong GameManager!");
        }

        if (!scoreText || !pauseScoreText || !gameOverScoreText)
        {
            Debug.LogError("Một hoặc nhiều Text UI (scoreText, pauseScoreText, gameOverScoreText) chưa được gán trong GameManager!");
        }

        if (!enemyPrefab || !explosionEffect || !muzzleFlash)
        {
            Debug.LogError("Một hoặc nhiều Prefab (enemyPrefab, explosionEffect, muzzleFlash) chưa được gán trong GameManager!");
        }

        // Khởi tạo trạng thái ban đầu
        StartMenu.SetActive(true);
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        Time.timeScale = 0f;
        UpdateScoreUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGameButton(true);
        }
    }

    void InstantiateEnemy()
    {
        if (enemyPrefab == null || !isGameStarted || isGameOver) return;

        Vector3 enemyPos = new Vector3(Random.Range(-5f, 5f), 6f, 0f);
        GameObject enemy = Instantiate(enemyPrefab, enemyPos, Quaternion.identity); // Sử dụng rotation mặc định của Prefab
        Destroy(enemy, enemyDestroyTime);
        Debug.Log("Enemy instantiated at " + enemyPos);
    }

    public void StartGameButton()
    {
        if (StartMenu != null)
        {
            StartMenu.SetActive(false);
        }
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
        Time.timeScale = 1f;
        // Bắt đầu tạo Enemy sau khi game bắt đầu
        isGameStarted = true;
        InvokeRepeating("InstantiateEnemy", 1f, 1f);
    }

    public void PauseGameButton(bool isPaused)
    {
        if (isPaused)
        {
            if (PauseMenu != null)
            {
                PauseMenu.SetActive(true);
            }
            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(false);
            }
            Time.timeScale = 0f;
            UpdateScoreUI();
        }
        else
        {
            if (PauseMenu != null)
            {
                PauseMenu.SetActive(false);
            }
            if (scoreText != null)
            {
                scoreText.gameObject.SetActive(true);
            }
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void AddScore(int points)
    {
        if (!isGameOver) // Chỉ tăng điểm khi game chưa kết thúc
        {
            score += points;
            UpdateScoreUI();
        }
    }

    public int GetScore() // Thêm phương thức này
    {
        return score;
    }

    public void GameOver()
    {
        if (isGameOver) return; // Tránh gọi nhiều lần

        isGameOver = true;

        if (GameOverMenu != null)
        {
            GameOverMenu.SetActive(true);
        }
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
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
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("ScoreText chưa được gán trong GameManager!");
        }

        if (pauseScoreText != null)
        {
            pauseScoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("PauseScoreText chưa được gán trong GameManager!");
        }

        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogError("GameOverScoreText chưa được gán trong GameManager!");
        }
    }
}