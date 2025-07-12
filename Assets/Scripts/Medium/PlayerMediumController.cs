using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMediumController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera cam;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject canvasMenu;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    void Start()
    {
        cam = Camera.main;
        healthSlider.maxValue = 100;
        healthSlider.value = 100;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        Vector3 newPos = transform.position + (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        Vector3 minBounds = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 maxBounds = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        newPos.x = Mathf.Clamp(newPos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        newPos.y = Mathf.Clamp(newPos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        transform.position = newPos;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10);
            Destroy(collision.gameObject);
        }
    }
    private void TakeDamage(int damage)
    {
        healthSlider.value -= damage;

        if (healthSlider.value <= 0)
        {
            Die();
        }
    }

    private void Die()
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
            Destroy(tempAudio, 1.3f);
        }

        if (gameOverScoreText != null)
        {
            int finalScore = MediumManager.Instance?.score ?? 0;
            gameOverScoreText.text = "" + finalScore;
        }

        Destroy(gameObject);
        Time.timeScale = 0f;
        canvasMenu.SetActive(true);
    }
    public void Heal(float amount)
    {
        healthSlider.value += amount;
        healthSlider.value = Mathf.Clamp(healthSlider.value, 0, healthSlider.maxValue);
    }
}

