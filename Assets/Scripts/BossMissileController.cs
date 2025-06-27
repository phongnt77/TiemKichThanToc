using UnityEngine;

public class BossMissileController : MonoBehaviour
{
    public float missileSpeed = 5f;

    void Update()
    {
        if (Time.timeScale > 0) // Ch? di chuy?n khi game không b? pause
        {
            transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
            if (transform.position.y < -6f) // H?y khi ra kh?i màn hình
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameManager.instance.GameOver();
        }
    }
}