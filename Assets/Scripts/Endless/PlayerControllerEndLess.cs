using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerEndLess : MonoBehaviour
{
    private Vector2 moveInput;
    private PlayerInputAction inputActions;
    public float speed = 10f;

    public float fireRate = 0.2f; // 5 viên/giây
    private float fireTimer = 0f;
    private bool isFiring = false;

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [SerializeField]
    private ObjectPoolManager bulletPool;

    private static PlayerControllerEndLess instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        inputActions = new PlayerInputAction();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        Debug.Log("moveInput performed: " + moveInput);

        inputActions.Player.Bullet.performed += ctx => isFiring = true;
        inputActions.Player.Bullet.canceled += ctx => isFiring = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //moveInput = Vector2.zero;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        Debug.Log("Player Position: " + transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        HandleShooting();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        if (inputActions != null)
            inputActions.Disable();
    }

    void MovePlayer()
    {
        //Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
        //transform.position += move * speed * Time.deltaTime;

        //// clamp the player's position to the camera's viewport
        //Vector3 clampedPosition = Camera.main.WorldToViewportPoint(transform.position);
        //clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0.05f, 0.95f);
        //clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0.05f, 0.95f);
        //transform.position = Camera.main.ViewportToWorldPoint(clampedPosition);
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
            transform.position += move * speed * Time.deltaTime;
        }


        Vector3 clampedPosition = Camera.main.WorldToViewportPoint(transform.position);
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0.05f, 0.95f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(clampedPosition);
    }

    void HandleShooting()
    {
        if (isFiring)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
        else
        {
            fireTimer = fireRate; // Đảm bảo bắn ngay khi nhấn lại
        }
    }

    void Shoot()
    {
        GameObject bulletObj = bulletPool.GetObject();
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.bulletPool = bulletPool; // Gán bulletPool cho viên đạn để nó có thể trả về pool 

        bulletObj.transform.position = transform.position;
        bulletObj.transform.rotation = Quaternion.identity;

        //// Bỏ qua va chạm giữa bullet và player
        //Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        //Collider2D playerCollider = GetComponent<Collider2D>();
        //if (bulletCollider != null && playerCollider != null)
        //{
        //    Physics2D.IgnoreCollision(bulletCollider, playerCollider, true);
        //}
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        if (GameManagerEndless.instance != null)
        {
            GameManagerEndless.instance.GameOver();
        }
        Debug.Log("Player died");
        // Thực hiện các hành động khi player chết, ví dụ: hiển thị
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player hit by Enemy");
            TakeDamage(50);
        }
        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Player hit by Asteroid");
            TakeDamage(10);
        }
        else if (collision.gameObject.CompareTag("BulletEnemy"))
        {
            Debug.Log("Player hit by BulletEnemy");
            TakeDamage(15);
        }
        else if (collision.gameObject.CompareTag("BossMissile"))
        {
            TakeDamage(20);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            TakeDamage(100);
        }
    }
}
