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
            Destroy(collision.gameObject); // Hủy Enemy (EnemyController sẽ tạo Explosion)
            Destroy(gameObject); // Hủy Missile
        }
    }
}
