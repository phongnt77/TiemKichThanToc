using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public float backgroundHeight = 10f;

    void Update()
    {
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        if (transform.position.y < -backgroundHeight)
        {
            transform.position += new Vector3(0, backgroundHeight * 2, 0);
        }
    }
}