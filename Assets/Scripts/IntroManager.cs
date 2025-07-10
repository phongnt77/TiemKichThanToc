using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool hasSkippedOrEnded = false;
    public TextMeshProUGUI tmpText;
    public float blinkInterval = 0.01f;
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        StartCoroutine(Blink());
    }

    void Update()
    {
        if (!hasSkippedOrEnded && (Input.anyKeyDown || Input.GetMouseButtonDown(0)))
        {
            hasSkippedOrEnded = true;
            StopVideoAndSkip();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!hasSkippedOrEnded)
        {
            hasSkippedOrEnded = true;
            StartCoroutine(DelayAndLoadScene());
        }
    }

    void StopVideoAndSkip()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(1);
    }

    IEnumerator DelayAndLoadScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(2f);

        tmpText.text = "Nhấn phím bất kì";
        tmpText.gameObject.SetActive(true);

        while (true)
        {
            tmpText.enabled = false;
            yield return new WaitForSeconds(blinkInterval);

            tmpText.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

}
