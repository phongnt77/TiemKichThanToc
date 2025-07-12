using UnityEngine;

public class BossMediumController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHealth = 10000;

    private int currentHealth;
    private float minX, maxX;
    private Vector3 direction = Vector3.right;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;

    void Start()
    {
        currentHealth = maxHealth;

        Camera cam = Camera.main;
        Vector3 leftEdge = cam.ViewportToWorldPoint(new Vector3(0f, 1f, cam.nearClipPlane + 10f));
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 1f, cam.nearClipPlane + 10f));

        minX = leftEdge.x + 0.5f;
        maxX = rightEdge.x - 0.5f;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        if (transform.position.x <= minX || transform.position.x >= maxX)
        {
            direction *= -1;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionEffect != null)
        {
            GameObject explosionEffectDes = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionEffectDes, 0.5f);
        }

        if (explosionSound != null)
        {
            GameObject tempAudio = new GameObject("ExplosionSound");
            AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
            audioSource.clip = explosionSound;
            audioSource.Play();
            Destroy(tempAudio, 1f);
        }
        MediumManager.Instance?.WinGame();
        PlayerPrefs.SetString("Level", "Hard");
        PlayerPrefs.Save();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(100);
            Destroy(other.gameObject);
        }
    }
}
