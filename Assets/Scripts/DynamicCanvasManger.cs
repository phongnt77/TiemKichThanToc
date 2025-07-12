using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCanvasManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
