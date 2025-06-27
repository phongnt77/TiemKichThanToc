using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
<<<<<<< Updated upstream
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        // Thay Vector3.left bằng Vector3.down
        Debug.Log("Enemy moving down at position: " + transform.position);
=======
        // Di chuyển xuống liên tục
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        // Giới hạn chỉ theo trục x
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.5f, 2.5f);
        transform.position = pos;

        // Hủy enemy khi ra khỏi màn hình (dưới y = -5f)
        if (pos.y < -5f)
        {
            Destroy(gameObject);
        }
>>>>>>> Stashed changes
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            Debug.Log("Enemy collided with Missile"); // Debug để kiểm tra va chạm

            if (GameManager.instance != null && GameManager.instance.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
                Debug.Log("Explosion created at: " + transform.position);
            }

            Destroy(collision.gameObject); // Hủy Missile
            Destroy(gameObject); // Hủy Enemy

            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(10);
                Debug.Log("Score increased by 10, new score: " + GameManager.instance.GetScore());
            }
        }
    }
}
