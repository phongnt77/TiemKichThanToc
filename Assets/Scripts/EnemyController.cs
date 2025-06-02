using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        // Thay Vector3.left bằng Vector3.down
        Debug.Log("Enemy moving down at position: " + transform.position);
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
