using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(DelayAndLoadScene());
    }

    IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
