using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    private float elapsedTime = 0f;



    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;
        int seconds = Mathf.FloorToInt(elapsedTime %60);
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
