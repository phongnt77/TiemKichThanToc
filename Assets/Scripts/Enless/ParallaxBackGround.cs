using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    //[SerializeField]
    //private float moveSpeed;
    //float backgroundImageHeight;
    //void Start()
    //{
    //    Sprite sprite = GetComponent<SpriteRenderer>().sprite;
    //    backgroundImageHeight = sprite.texture.height / sprite.pixelsPerUnit;
    //    Debug.Log(backgroundImageHeight);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    float moveY = moveSpeed * Time.deltaTime;
    //    transform.position += new Vector3(0, moveY);

    //    if (Mathf.Abs(transform.position.y) - backgroundImageHeight > 0)
    //    {
    //        transform.position = new Vector3(transform.position.x,0f);
    //    }
    //}

    //public float speed = 4f;
    //private Vector3 StartPosition;

    // void Start()
    //{
    //    StartPosition = transform.position;
    //}

    // void Update()
    //{
    //    transform.Translate(Vector3.down * speed * Time.deltaTime);
    //    if (transform.position.y < -200f)
    //    {
    //        transform.position = StartPosition;
    //    }
    //}

    [SerializeField] private float moveSpeed = 2f;
    private float backgroundHeight;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        backgroundHeight = sr.bounds.size.y;
    }

    void Update()
    {
        // Di chuyển background xuống theo chiều dọc
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        // Nếu background ra khỏi camera view ở phía dưới thì reset lên trên
        //float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;
        if ( transform.position.y <= -210f)
        {
            //float cameraTop = mainCamera.transform.position.y + mainCamera.orthographicSize;
            transform.position = new Vector3(transform.position.x,backgroundHeight, transform.position.z);
        }
    }
}
