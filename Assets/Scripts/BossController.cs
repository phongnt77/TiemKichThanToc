using System.Reflection;
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

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;
        fireTimer = fireRate;
    }

    void Update()
    {
        Move();
        ShootMissile();
    }

    void Move()
    {
        Vector3 pos = transform.position;
        if (movingRight)
            pos.x += moveSpeed * Time.deltaTime;
        else
            pos.x -= moveSpeed * Time.deltaTime;

        transform.position = pos;

        if (Mathf.Abs(pos.x - startPos.x) >= moveRange)
            movingRight = !movingRight;
    }

    void ShootMissile()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                Vector2 direction = (player.transform.position - firePoint.position).normalized;
                GameObject missile = Instantiate(bossMissilePrefab, firePoint.position, Quaternion.identity);

                Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
                rb.linearVelocity = direction * 5f;
            }

            fireTimer = fireRate;
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
        Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.instance.GameOver(); // ho?c g?i Victory n?u c?n
    }
}
