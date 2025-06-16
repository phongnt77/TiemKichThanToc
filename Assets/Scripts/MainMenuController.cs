using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ScenceHard");
    }

    public void ShowHowToPlay()
    {
        // T?y b?n: hi?n popup ho?c chuy?n scene kh?c
        Debug.Log("Show how to play...");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
