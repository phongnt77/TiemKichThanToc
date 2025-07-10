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
    void Awake()
    {
        inputActions = new PlayerInputAction();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Bullet.performed += ctx => isFiring = true;
        inputActions.Player.Bullet.canceled += ctx => isFiring = false;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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
        inputActions.Disable();
    }

    void MovePlayer()
    {
        Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
        transform.position += move * speed * Time.deltaTime;

        // clamp the player's position to the camera's viewport
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

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            //Die();
        }
    }
}
