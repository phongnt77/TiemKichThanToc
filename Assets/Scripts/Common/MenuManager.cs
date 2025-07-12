using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI pauseScoreText;
    [SerializeField] private TextMeshProUGUI pauseScoreTextHard;
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

    public void HandlePauseButtonHard()
    {
        Time.timeScale = 0f;
        int currentScore = MediumManager.Instance.score;
        pauseScoreTextHard.text = "" + currentScore;
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
    public void HandlePlayButtonForGameOverMedium()
    {
        SceneManager.LoadScene("SceneMedium");
    }

    public void HandleNextButtonMedium()
    {
        SceneManager.LoadScene("SceneHard");
    }
}
