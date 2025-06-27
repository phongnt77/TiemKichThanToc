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
    public Transform[] firePoints; // M?ng các ?i?m b?n cho ??n và Beam
    public float fireRate = 2f;
    private float fireTimer;

    [Header("Ultimate Attack - Beam")]
    public float ultimateDelay = 10f;
    public GameObject beamPrefab;
    public float beamDuration = 2f;
    private float ultimateTimer;
    private bool isFiringUltimate = false; // Tr?ng thái b?n Ultimate
    private Vector3 ultimateStartPosition; // V? trí ban ??u khi b?n Ultimate

    void Start()
    {
        currentHealth = maxHealth;
        startPos = transform.position;
        fireTimer = 0f;
        ultimateTimer = 0f;
        Debug.Log("BossController started at: " + transform.position + " with ultimateDelay: " + ultimateDelay);
    }

    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (isFiringUltimate)
            {
                // Khóa v? trí khi b?n Ultimate
                transform.position = ultimateStartPosition;
                Debug.Log("Boss locked at: " + ultimateStartPosition + " at time: " + Time.time);
            }
            else
            {
                Move();
            }
            ShootMissile();
            CheckUltimate();
        }
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

        if (fireTimer <= 0f && !isFiringUltimate && firePoints != null && firePoints.Length > 0)
        {
            Transform firePoint = firePoints[Random.Range(0, firePoints.Length)];
            if (firePoint != null)
            {
                GameObject missile = Instantiate(bossMissilePrefab, firePoint.position, Quaternion.identity);
                Debug.Log("Boss Missile spawned at: " + firePoint.position + " at time: " + Time.time);

                BossMissileController missileCtrl = missile.GetComponent<BossMissileController>();
                if (missileCtrl != null)
                {
                    missileCtrl.missileSpeed = 5f;
                    missile.transform.up = Vector3.down;
                }
                else
                {
                    Debug.LogError("BossMissile missing BossMissileController!");
                }
            }
            else
            {
                Debug.LogError("Fire point is null!");
            }

            fireTimer = fireRate;
        }
    }

    void CheckUltimate()
    {
        ultimateTimer += Time.deltaTime;
        Debug.Log("Ultimate timer: " + ultimateTimer + " / " + ultimateDelay + " | Conditions: beamPrefab=" + (beamPrefab != null) + ", firePoints=" + (firePoints != null) + ", Length=" + (firePoints?.Length ?? 0) + ", !isFiringUltimate=" + !isFiringUltimate);
        if (ultimateTimer >= ultimateDelay && beamPrefab != null && firePoints != null && firePoints.Length > 0 && !isFiringUltimate)
        {
            FireUltimate();
            ultimateTimer = 0f;
            Debug.Log("Boss Ultimate Beam triggered at time: " + Time.time);
        }
    }

    void FireUltimate()
    {
        if (beamPrefab != null && firePoints != null)
        {
            isFiringUltimate = true;
            ultimateStartPosition = transform.position;
            Debug.Log("Firing Ultimate from position: " + ultimateStartPosition + " with beamDuration: " + beamDuration);
            foreach (Transform firePoint in firePoints)
            {
                if (firePoint != null)
                {
                    SpawnBeam(firePoint, Vector3.down);
                }
            }
            Invoke("EndUltimate", beamDuration);
        }
        else
        {
            Debug.LogError("beamPrefab or firePoints is null! beamPrefab: " + (beamPrefab != null) + ", firePoints: " + (firePoints != null));
        }
    }

    void SpawnBeam(Transform firePoint, Vector3 direction)
    {
        GameObject beam = Instantiate(beamPrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Beam spawned at: " + firePoint.position + " with direction: " + direction + " | Beam active: " + beam.activeSelf);

        beam.transform.up = direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        beam.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        BeamController beamCtrl = beam.GetComponent<BeamController>();
        if (beamCtrl != null)
        {
            beamCtrl.duration = beamDuration;
            Debug.Log("BeamController found, duration set to: " + beamDuration);
        }
        else
        {
            Debug.LogWarning("BeamController not found, using Destroy with duration: " + beamDuration);
            Destroy(beam, beamDuration);
        }
    }

    void EndUltimate()
    {
        isFiringUltimate = false;
        Debug.Log("Ultimate finished, resuming normal behavior at time: " + Time.time + " at position: " + transform.position);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (GameManager.instance?.explosionEffect != null)
        {
            GameObject explosion = Instantiate(GameManager.instance.explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f);
        }
        Destroy(gameObject);
        if (GameManager.instance != null)
            GameManager.instance.AddScore(50);
    }
}