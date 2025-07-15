using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    private int damage = 10;
    void Start()
    {
        
    }
    void Update()
    {
        // Di chuyển bullet xuống dưới
        float moveSpeed = 4f * Time.deltaTime;
        transform.position += new Vector3(0, -moveSpeed, 0);

        // Kiểm tra nếu 
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Gây damage cho player nếu cần
            //other.GetComponent<PlayerControllerEndLess>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Missile"))
        {
            // Tạo hiệu ứng nổ nếu có
            //if (GameManager.instance?.explosionEffect != null)
            //{
            //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
            //    Destroy(explosion, 1f);
            //}

            if (GameManagerEndless.instance != null)
            {
                GameManagerEndless.instance.AddScore(1);
            }
            Destroy(gameObject);

        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            return;
        }
    }
}