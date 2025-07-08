using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float backgroundHeight = 10f; // Chiều cao của background (theo đơn vị world)

    void Update()
    {
        // Di chuyển background xuống dưới
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Nếu background ra khỏi màn hình dưới, đưa nó lên trên
        if (transform.position.y < -backgroundHeight)
        {
            transform.position += new Vector3(0, backgroundHeight * 2, 0);
        }
    }
}