using UnityEngine;

public class BossMissileController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        Debug.Log("BossMissile Spawned at: " + transform.position);
        // Không gán linearVelocity ở đây, để BossController xử lý
    }

   void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player"))
        {
        // Gọi TakeDamage trên Player, KHÔNG destroy player ở đây
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(50); // hoặc damage tùy bạn
        }
        Destroy(gameObject); // Chỉ destroy BossMissile
        }
    }
}