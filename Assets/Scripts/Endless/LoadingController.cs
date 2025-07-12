using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Enless
{
    public static class LoadingData
    {
        public static string SceneToLoad;
    }

    public class LoadingController : MonoBehaviour
    {
        public Slider loadingBar; 

        void Start() 
        { 
            StartCoroutine(LoadSceneAsync(LoadingData.SceneToLoad)); 
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            while (asyncLoad.progress < 0.7f)
            {
                loadingBar.value = asyncLoad.progress;
                yield return null;
            }
            float fakeProgress = 0.5f;
            while (fakeProgress < 1f)
            {
                fakeProgress += Time.deltaTime * 0.5f;  // Tốc độ tăng slider (tuỳ chỉnh)
                loadingBar.value = fakeProgress;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f); // Cho cảm giác mượt thêm chút
            asyncLoad.allowSceneActivation = true;
        }
    }
}
