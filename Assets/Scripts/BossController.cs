using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 1000;
    private int currentHealth;

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

    [Header("Laser Attack")]
    public GameObject laserPrefab; // Tham chiếu đến LaserParticle.prefab
    public float laserFireRate = 5f; // Chu kỳ bắn laser
    private float laserTimer;
    private bool isShootingLaser = false;

    private int hitCount;

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;
        fireTimer = fireRate;
        laserTimer = laserFireRate;
        isShootingLaser = false;
        hitCount = 0;
        Debug.Log("✅ BossController has started");
    }

    void Update()
    {
         if (!isShootingLaser)
            Move();
        ShootMissile();
        ShootLaser();
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

  void ShootLaser()
{
    laserTimer -= Time.deltaTime;
    if (laserTimer <= 0f && laserPrefab != null && firePoint != null && !isShootingLaser)
    {
        Debug.Log("🔔 Đến thời điểm Boss bắn Laser!");
        StartCoroutine(ShootLaserAndPause());
        laserTimer = laserFireRate;
    }
}

    IEnumerator ShootLaserAndPause()
{
    isShootingLaser = true;
    Debug.Log("💥 Boss bắt đầu bắn Laser!");

    // Tạo Laser
    GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
    Debug.Log("💥 Laser đã được tạo tại: " + firePoint.position);

    // Gắn laser vào firePoint (nếu muốn nó di chuyển theo Boss)
    laser.transform.parent = firePoint;
    laser.transform.localRotation = Quaternion.Euler(0, 0, 90);

    // Lấy thời gian tồn tại của laser
    float laserTime = 1.5f; // Giá trị mặc định
    LaserController laserCtrl = laser.GetComponent<LaserController>();
    if (laserCtrl != null)
        laserTime = laserCtrl.laserDuration;

    // Đứng yên đúng thời gian laser tồn tại
    yield return new WaitForSeconds(laserTime);

    isShootingLaser = false;
    Debug.Log("✅ Boss tiếp tục di chuyển sau khi bắn Laser!");
}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hitCount++;

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (hitCount >= 5)
        {
            Victory();
        }
    }

    void Die()
    {
        Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.instance.GameOver();
    }

    void Victory()
    {
        if (GameManager.instance != null)
        {
            Debug.Log("Victory! You won by hitting the Boss 5 times!");
            GameManager.instance.Victory();
        }
        Destroy(gameObject);
    }
}