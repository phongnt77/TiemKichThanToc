using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button previousButton;

    public Button nextButton;

    private int currentPage = 1;

    public List<GameObject> contentPages;

    public Button mediumButton;

    public Button hardButton;

    public void HandleHowToPlayButton()
    {
        previousButton.interactable = false;
        nextButton.interactable = true;
    }

    public void HandleNextPage()
    {
        currentPage++;

        if (currentPage == 2)
        {
            previousButton.interactable = true;
        }
        else if (currentPage == 5)
        {
            nextButton.interactable = false;
        }
        HandleActiveContentPage();
    }

    public void HandlePreviousPage()
    {
        currentPage--;

        if (currentPage == 1)
        {
            previousButton.interactable = false;
        }
        else if (currentPage == 4)
        {
            nextButton.interactable = true;
        }
        HandleActiveContentPage();
    }

    private void HandleActiveContentPage()
    {
        for (int i = 0; i < contentPages.Count; i++)
        {
            contentPages[i].SetActive(i == (currentPage - 1));
        }
    }

    public void HandlePlayOptions()
    {
        string level = PlayerPrefs.GetString("Level");
        if (string.IsNullOrEmpty(level))
        {
            mediumButton.interactable = false;
            hardButton.interactable = false;
        }
        else if (level.Equals("Easy"))
        {
            mediumButton.interactable = true;
            hardButton.interactable = false;
        }
        else
        {
            hardButton.interactable = true;
        }
    }

    public void ResetPageIndex()
    {
        currentPage = 1;
    }

    public void HandleEasyButton()
    {
        SceneManager.LoadScene("SceneEasy");
    }
    public void HandleMediumButton()
    {
        SceneManager.LoadScene("SceneMedium");
    }
    public void HandleHardButton()
    {
        SceneManager.LoadScene("SceneHard");
    }
    public void HandleEndlessButton()
    {
        SceneManager.LoadScene("SceneEndLess");
    }
    public void HandleQuitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        PlayerPrefs.DeleteKey("Level");
        // If running in application build use
        // Application.Quit();
    }
}
