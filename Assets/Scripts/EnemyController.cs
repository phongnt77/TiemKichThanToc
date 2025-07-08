using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        // Di chuyển Enemy xuống dưới
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Nếu Enemy ra khỏi vùng chơi (dưới cùng màn hình), tự huỷ
        if (transform.position.y < -6f) // -6f tuỳ chỉnh theo vùng chơi/camera
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            // Tạo hiệu ứng nổ nếu có
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            // Phá hủy đạn
            Destroy(other.gameObject);
            // Phá hủy Enemy
            Destroy(gameObject);

            // Cộng điểm nếu GameManager tồn tại
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(10);
            }
        }
        else if (other.CompareTag("Player"))
        {
            // Gây damage cho Player
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(100); // Số damage tuỳ chỉnh
            }

            // Tạo hiệu ứng nổ nếu có
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            // Phá hủy Enemy
            Destroy(gameObject);
        }
    }
}