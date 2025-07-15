using TMPro;
using UnityEngine;

public class BossControllerEndless : MonoBehaviour
{
    [Header("HP Settings")]
    public int maxHP = 1000;
    private int currentHP;

    [SerializeField] private HealthBar healthBar;

    [Header("Bullet")]
    [SerializeField] private GameObject shootBulletPrefab;      // Prefab đạn thường
    [SerializeField] private GameObject circleBulletPrefab;     // Prefab đạn vòng tròn
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 5f;

    [Header("Timers")]
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private float shootCircleInterval = 15f;

    [Header("Movement Settings")]
    [SerializeField] private Vector2 targetPosition = new Vector2(0.75f, 30.1f);
    [SerializeField] private float moveSpeed = 5f;
    private bool hasReachedTarget = false;

    private float shootTimer = 0f;
    private float shootCircleTimer = 0f;

    public System.Action OnBossDie;

    void Start()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);

        shootTimer = shootInterval;
        shootCircleTimer = shootCircleInterval;
    }

    void Update()
    {
        if (!hasReachedTarget)
        {
            // Di chuyển về vị trí đích
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) <= 0.01f)
            {
                hasReachedTarget = true;
            }
        }
        shootTimer -= Time.deltaTime;
        shootCircleTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }

        if (shootCircleTimer <= 0f)
        {
            ShootCircle(0f);   // Bắn vòng 1
            ShootCircle(22.5f); // Bắn vòng 2 lệch 22.5 độ
            ShootCircle(35.5f);
            shootCircleTimer = shootCircleInterval;
        }
    }

    void Shoot()
    {
        if (shootBulletPrefab && firePoint)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            // Instantiate đạn
            GameObject bullet = Instantiate(shootBulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                Vector2 direction;

                // Nếu tìm được Player thì nhắm về vị trí hiện tại của Player
                if (playerObj != null)
                {
                    Vector2 targetPosition = playerObj.transform.position;
                    direction = (targetPosition - (Vector2)firePoint.position).normalized;
                }
                else
                {
                    // Nếu không có Player thì bắn thẳng xuống
                    direction = Vector2.down;
                }

                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }


    void ShootCircle(float startAngle = 0f)
    {
        int bulletCount = 8;
        float angleStep = 360f / bulletCount;
        float angle = startAngle;

        for (int i = 0; i < bulletCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 direction = new Vector2(dirX, dirY).normalized;

            GameObject bullet = Instantiate(circleBulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = direction * bulletSpeed;

            angle += angleStep;
        }
    }


    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        healthBar.SetHealth(currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss died!");
        OnBossDie?.Invoke();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Missile"))
        {
            TakeDamage(Bullet.damage);
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("Player"))
        {
            //PlayerControllerEndLess player = collision.GetComponent<PlayerControllerEndLess>();
            //if (player != null)
            //{
            //    player.TakeDamage(10);
            //}
            Destroy(collision.gameObject);
        }
    }
}
