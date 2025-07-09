using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 25f;

    void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Missile hit: " + other.name + " | Tag: " + other.tag);
        if (other.CompareTag("Enemy"))
        {
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameManager.instance?.AddScore(10);
        }
        else if (other.CompareTag("Boss"))
        {
            other.GetComponent<BossController>()?.TakeDamage(50);
            Destroy(gameObject);
        }
    }
}