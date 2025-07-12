using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#region No development and use on this script, just try a loading screen for Endless Mode

public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager Instance;
    public GameObject loadingScreenObject;
    public Slider loadingBar;
    private void Awake()
    {
       if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadEndlessMode()
    {
        loadingScreenObject.SetActive(true);
        StartCoroutine(LoadSceneAsync("ScenceEndLess"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }

        loadingBar.value = 1f;
        yield return new WaitForSeconds(0.3f);  // Cho cảm giác mượt
        asyncLoad.allowSceneActivation = true;
    }
}
#endregion