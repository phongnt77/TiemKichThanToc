using UnityEngine;

public class Asteriod : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] sprites;

    [HideInInspector] public ObjectPoolManager asteroidPool;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];


        float pushX = Random.Range(-1f, 1f);
        float pushY = Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(pushX, pushY) * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        float moveY = GameManagerEndless.instance.asteroidSpeed * Time.deltaTime;
        transform.position += new Vector3(0, -moveY, 0);
        transform.rotation *= Quaternion.Euler(Vector3.forward * 25f * Time.deltaTime);


        if (viewPos.y > 1.1f || viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
        {
            //ObjectPoolManager.Instance.ReturnObject(gameObject);
            asteroidPool.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Asteroid hitted by player");

            // Tạo hiệu ứng nổ nếu có
            //if (GameManager.instance?.explosionEffect != null)
            //{
            //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
            //    Destroy(explosion, 1f);
            //}

            asteroidPool.ReturnObject(gameObject);
        }
        else if (other.gameObject.CompareTag("Missile"))
        {
            // Tạo hiệu ứng nổ nếu có
            //if (GameManager.instance?.explosionEffect != null)
            //{
            //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
            //    Destroy(explosion, 1f);
            //}

            Debug.Log("Asteroid hitted by missile");

            asteroidPool.ReturnObject(gameObject);
        }
        //else if (other.gameObject.CompareTag("Bullet"))
        //{
        //    // Tạo hiệu ứng nổ nếu có
        //    //if (GameManager.instance?.explosionEffect != null)
        //    //{
        //    //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
        //    //    Destroy(explosion, 1f);
        //    //}
        //    Debug.Log("Asteroid hitted by bullet");
        //    asteroidPool.ReturnObject(gameObject);

        //}
    }
}
