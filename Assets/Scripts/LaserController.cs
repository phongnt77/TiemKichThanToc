using UnityEngine;

public class LaserController : MonoBehaviour
{
    private ParticleSystem ps;
    public float laserDuration = 1f; // Th?i gian laser t?n t?i
    private float timer;
    public int damage = 50; // S�t th??ng g�y ra

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        timer = laserDuration;
        if (ps != null)
        {
            ps.Play(); // B?t ??u ph�t particle khi t?o
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (ps != null && !ps.IsAlive())
            {
                Destroy(gameObject); // Ph� h?y khi particle k?t th�c
            }
        }
        // C?p nh?t v? tr� (n?u c?n di chuy?n)
        transform.position = transform.parent?.position ?? transform.position; // Theo firePoint
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}