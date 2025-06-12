using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float missileSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * missileSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            // Nếu còn enemy thì chưa cho bắn boss
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                collision.gameObject.GetComponent<BossController>()?.TakeDamage(50); // 50 là sát thương tạm
                Destroy(gameObject);
            }
        }
    }
}
