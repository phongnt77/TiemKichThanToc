using UnityEngine;
using UnityEngine.UI;

public class BossHardController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHealth = 10000;
    public Slider healthSlider;
    private int currentHealth;
    private float minX, maxX;
    private Vector3 direction = Vector3.right;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;

    void Start()
    {
        currentHealth = maxHealth;
        // Luôn tìm Slider khi boss xuất hiện
        if (healthSlider == null)
        {
            GameObject sliderObj = GameObject.FindWithTag("BossHealthBar");
            if (sliderObj != null)
            {
                healthSlider = sliderObj.GetComponent<Slider>();
            }
        }
        // Khi boss xuất hiện, bật thanh máu và set giá trị
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
            healthSlider.gameObject.SetActive(true); // Hiện thanh máu khi boss xuất hiện
            Debug.Log("[Boss] Đã hiện thanh máu ở Start");
        }
        Debug.Log("[Boss] Đã tìm thấy Slider: " + (healthSlider != null));

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
        Debug.Log("Boss bị trúng đạn, máu còn: " + currentHealth);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
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
        HardManager.Instance?.OnBossDefeated();
        PlayerPrefs.SetString("Level", "Hard");
        PlayerPrefs.Save();
        // Khi boss chết, ẩn thanh máu
        if (healthSlider != null)
        {
            Debug.Log("[Boss] Destroy health bar");
            Destroy(healthSlider.gameObject); // Hủy thanh máu khi boss chết
        }
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
