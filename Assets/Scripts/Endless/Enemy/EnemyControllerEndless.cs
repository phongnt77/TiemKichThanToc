
using UnityEngine;

namespace Assets.Scripts.Enless.Enemy
{
    public class EnemyControllerEndless : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private Sprite[] sprites;

        [HideInInspector] public ObjectPoolManager enemyPool;
        private Rigidbody2D rb;

        public float moveSpeed = 3f;

        [Header("Shooting")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float shootInterval = 2f;
        [SerializeField] private float bulletSpeed = 8f;
        [SerializeField] private Transform firePoint;
        private float shootTimer;

        // Thêm biến đếm số lần bắn
        private int shootCount = 0;
        private int maxShootCount = 5;

        public System.Action OnEnemyDie;


        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();

            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

            //float pushX = Random.Range(-1f, 1f);
            //float pushY = Random.Range(-1f, 1f);
            //rb.linearVelocity = new Vector2(pushX, pushY) * 1f;

            shootTimer = shootInterval;
        }

        void Update()
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            float moveY = GameManagerEndless.instance.enemySpeed * Time.deltaTime;
            transform.position += new Vector3(0, -moveY, 0);

            // Kiểm tra nếu Enemy ra
            transform.LookAt(transform.position);

            ShootAtPlayer();

            if (viewPos.y > 1.1f || viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
            {
                enemyPool.ReturnObject(gameObject);
                OnEnemyDie?.Invoke();
            }
        }

        private void ShootAtPlayer()
        {
            // Chỉ bắn nếu chưa đủ 5 lần
            if (shootCount >= maxShootCount)
                return;

            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null && bulletPrefab != null && firePoint != null)
                {
                    Vector2 direction = (playerObj.transform.position - firePoint.position).normalized;
                    // Tạo đạn và thiết lập vận tốc
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                    // Xoay viên đạn 90 độ theo trục Z vì sprite của viên đạn có thể được thiết kế theo hướng khác
                    bullet.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

                    Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                    if (bulletRb != null)
                    {
                        bulletRb.linearVelocity = direction * bulletSpeed;
                    }
                    shootCount++; // Tăng biến đếm sau mỗi lần bắn
                }
                shootTimer = shootInterval;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                // Gây damage cho Player
                PlayerControllerEndLess player = other.GetComponent<PlayerControllerEndLess>();
                //if (player != null)
                //{
                //    player.TakeDamage(50); // Số damage tuỳ chỉnh
                //}

                // Tạo hiệu ứng nổ nếu có
                //if (GameManager.instance?.explosionEffect != null)
                //{
                //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                //    Destroy(explosion, 1f);
                //}
                Debug.Log("Enemy hit by Player");
                // Phá hủy Enemy
                enemyPool.ReturnObject(gameObject);

                OnEnemyDie?.Invoke();// Gọi callback khi enemy die
            }
            else if (other.gameObject.CompareTag("Missile"))
            {
                // Tạo hiệu ứng nổ nếu có
                //if (GameManager.instance?.explosionEffect != null)
                //{
                //    GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
                //    Destroy(explosion, 1f);
                //}
                Debug.Log("Enemy hit by Missile");
                // Phá hủy Enemy
                enemyPool.ReturnObject(gameObject);

                OnEnemyDie?.Invoke();// Gọi callback khi enemy die
            }
            else if (other.gameObject.CompareTag("Enemy"))
            {
                Vector2 direction = (transform.position - other.transform.position).normalized;
                transform.position += (Vector3)direction * 1f; // dịch nhẹ ra ngoài mỗi frame
            }
        }
    }
}
