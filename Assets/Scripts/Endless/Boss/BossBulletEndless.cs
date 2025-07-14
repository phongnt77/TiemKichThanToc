using UnityEngine;

public class BossBulletEndless : MonoBehaviour
{
    [SerializeField] private int damage = 50; // damage sẽ từ từ tăng theo thời gian
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float moveSpeed = 4f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        damage += (int)(Time.deltaTime * 5); // Increase damage over time

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControllerEndLess>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Missile"))
        {
            if (GameManagerEndless.instance != null)
            {
                GameManagerEndless.instance.AddScore(2);
            }
            Destroy(gameObject);
        }
    }
}
