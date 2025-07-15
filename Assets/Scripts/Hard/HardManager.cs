using TMPro;
using UnityEngine;

public class HardManager : MonoBehaviour
{
    public static HardManager Instance;

    public int score = 0;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Victory Settings")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private TextMeshProUGUI victoryScoreText;

    private int bossDefeated = 0;
    private int totalBoss = 2;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    // Hàm này ???c g?i khi 1 boss b? tiêu di?t
    public void OnBossDefeated()
    {
        bossDefeated++;
        if (bossDefeated >= totalBoss)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        Time.timeScale = 0f;

        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);
        }

        if (victoryScoreText != null)
        {
            victoryScoreText.text = "" + score;
        }
    }
}
