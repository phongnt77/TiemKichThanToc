using UnityEngine;

public class BoostHPItem : MonoBehaviour
{
    public int healAmount = 20; // HP hồi lại cho Player
    public ObjectPoolManager pool;

    void Update()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.y > 1.1f || viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
        {
            //ObjectPoolManager.Instance.ReturnObject(gameObject);
            pool.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hồi máu cho player nếu có PlayerHealth script
            PlayerControllerEndLess playerHealth = other.GetComponent<PlayerControllerEndLess>();
            if (playerHealth != null)
            {
                playerHealth.currentHealth = Mathf.Min(
                    playerHealth.currentHealth + healAmount,
                    playerHealth.maxHealth
                );
                playerHealth.healthBar.SetHealth(playerHealth.currentHealth); // Update thanh máu
            }


            // Trả về pool
            pool.ReturnObject(gameObject);
        }
    }
}
