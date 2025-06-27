using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private float height;
    private Transform[] layers;
    private int layerCount;

    void Start()
    {
        // L?y SpriteRenderer v� k�ch th??c height
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("BackgroundScroll: No SpriteRenderer found on " + gameObject.name);
            return;
        }
        height = spriteRenderer.bounds.size.y;

        // T?o m?ng ?? l?u c�c layer
        layerCount = 2;
        layers = new Transform[layerCount];
        layers[0] = transform;

        // T?o layer th? 2 ngay d??i layer ??u ti�n
        GameObject secondLayer = Instantiate(gameObject, transform.position + new Vector3(0, height, 0), Quaternion.identity);
        secondLayer.name = gameObject.name + "_Clone"; // ??t t�n ?? d? nh?n di?n
        secondLayer.GetComponent<BackgroundScroll>().enabled = false; // T?t script tr�n b?n sao
        layers[1] = secondLayer.transform;
        secondLayer.transform.SetParent(transform.parent);

        Debug.Log("BackgroundScroll initialized with height: " + height);
    }

    void Update()
    {
        // Di chuy?n t?t c? layer xu?ng
        for (int i = 0; i < layerCount; i++)
        {
            if (layers[i] != null)
            {
                layers[i].Translate(Vector3.down * scrollSpeed * Time.deltaTime);

                // N?u layer ra kh?i m�n h�nh, di chuy?n n� l�n tr�n
                Vector3 pos = layers[i].position;
                if (pos.y <= -height)
                {
                    layers[i].Translate(Vector3.up * height * 2f);
                    Debug.Log("Layer " + i + " reset to top at position: " + pos);
                }
            }
        }
    }
}