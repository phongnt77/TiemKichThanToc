using UnityEngine;
using UnityEngine.UI; // Cho Slider
using TMPro; // Thêm cho TextMeshPro

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float missileLifeTime = 5f;

    [Header("Muzzle Effect")]
    public Transform muzzleSpawnPoint;

    [Header("Health")]
    public int maxHealth = 1000;
    private int currentHealth;

    [Header("UI")]
    public Slider healthBar; // Tham chiếu đến Slider Health Bar
    public TMP_Text healthText; // Thay Text bằng TMP_Text cho TextMeshPro

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, y, 0) * speed * Time.deltaTime;
        transform.Translate(move);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.5f, 2.5f);
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
        transform.position = pos;
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (missilePrefab != null && missileSpawnPoint != null)
            {
                GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                Destroy(missile, missileLifeTime);
            }

            if (GameManager.instance != null && GameManager.instance.muzzleFlash != null && muzzleSpawnPoint != null)
            {
                GameObject muzzle = Instantiate(GameManager.instance.muzzleFlash, muzzleSpawnPoint.position, Quaternion.identity);
                Destroy(muzzle, missileLifeTime);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
        else
        {
            Debug.Log("Player HP: " + currentHealth);
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(100);
        }
        else if (collision.gameObject.CompareTag("BossMissile"))
        {
            TakeDamage(50);
        }
        else if (collision.gameObject.CompareTag("Laser"))
        {
            TakeDamage(50);
        }
    }

}
