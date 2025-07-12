using TMPro;
using UnityEngine;

public class EasyManager : MonoBehaviour
{
    public static EasyManager Instance;

    public int score = 0;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Victory Settings")]
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private TextMeshProUGUI victoryScoreText;
    [SerializeField] private int scoreToWin = 500;

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

        if (score >= scoreToWin)
        {
            WinGame();
            PlayerPrefs.SetString("Level", "Easy");
            PlayerPrefs.Save();
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void WinGame()
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
