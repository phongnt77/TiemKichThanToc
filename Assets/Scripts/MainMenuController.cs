using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void HandleEasyButton()
    {
        SceneManager.LoadScene("ScenceEasy");
    }
    public void HandleMediumButton()
    {
        SceneManager.LoadScene("ScenceMedium");
    }
    public void HandleHardButton()
    {
        SceneManager.LoadScene("ScenceHard");
    }
    public void HandleEndlessButton()
    {
        SceneManager.LoadScene("ScenceEndLess");
    }
}
