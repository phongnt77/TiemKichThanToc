using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed;

    [SerializeField]
    private Renderer backgroundRenderer;

    private void Update()
    {
        backgroundRenderer.material.mainTextureOffset += new Vector2(0f, scrollSpeed * Time.deltaTime);
    }
}