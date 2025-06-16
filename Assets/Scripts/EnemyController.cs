using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            Destroy(collision.gameObject);
            Destroy(gameObject);

            GameManager.instance?.AddScore(10);
        }
    }
}
