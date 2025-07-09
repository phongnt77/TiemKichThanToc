using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossController : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float moveRange = 3f;
    private Vector3 startPos;
    private bool movingRight = true;

    [Header("Attack")]
    public GameObject bossMissilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    private float fireTimer;

    // Xóa hitCount

    public BossUIController bossUI;

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;
        fireTimer = fireRate;
        Debug.Log("✅ BossController has started");

        // Tự động tìm BossUIController nếu chưa gán
        if (bossUI == null)
        {
            bossUI = FindObjectOfType<BossUIController>();
        }
        if (bossUI != null)
        {
            bossUI.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        Move();
        ShootMissile();
    }

    void Move()
    {
        Vector3 pos = transform.position;
        pos.x += (movingRight ? 1 : -1) * moveSpeed * Time.deltaTime;
        transform.position = pos;

        if (Mathf.Abs(pos.x - startPos.x) >= moveRange)
            movingRight = !movingRight;
    }

    void ShootMissile()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f && bossMissilePrefab != null && firePoint != null)
        {
            Vector2 direction = Vector2.down;
            GameObject missile = Instantiate(bossMissilePrefab, firePoint.position, Quaternion.identity);
            missile.transform.up = direction;
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = direction * 5f;
            Debug.Log("🚀 Boss bắn đạn từ FirePoint: " + firePoint.position);
            fireTimer = fireRate;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        if (bossUI != null)
        {
            bossUI.SetHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            Victory();
        }
    }

    void Die()
    {
        Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.instance.GameOver();

        if (bossUI != null && bossUI.healthBar != null)
            bossUI.healthBar.gameObject.SetActive(false);

        // Hiện Pop-up "You Win"
        GameObject winPopup = GameObject.Find("WinPopup");
        if (winPopup != null)
            winPopup.SetActive(true);
    }

    void Victory()
    {
        Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.instance.Victory();
        if (bossUI != null && bossUI.healthBar != null)
            bossUI.healthBar.gameObject.SetActive(false);
        if (bossUI != null && bossUI.healthText != null)
            bossUI.healthText.gameObject.SetActive(false);

        // Hiện GameWinMenu
        GameObject winMenu = GameObject.Find("GameWinMenu");
        if (winMenu != null)
            winMenu.SetActive(true);
    }
}