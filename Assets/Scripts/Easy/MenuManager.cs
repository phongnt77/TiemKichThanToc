using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI pauseScoreText;

    void Start()
    {
        Time.timeScale = 1f;
        healthSlider.interactable = false;
    }

    public void HandlePauseButton()
    {
        Time.timeScale = 0f;
        int currentScore = EasyManager.Instance.score;
        pauseScoreText.text = "" + currentScore;
    }

    public void HandlePlayButton()
    {
        Time.timeScale = 1f;
    }

    public void HandleHomeButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void HandlePlayButtonForGameOver()
    {
        SceneManager.LoadScene("SceneEasy");
    }

    public void HandleNextButton()
    {
        SceneManager.LoadScene("SceneMedium");
    }
}
