using UnityEngine;

public class BossMissileController : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        // ??t v?n t?c r?i th?ng xu?ng khi ??n ???c t?o ra
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * speed;
        Debug.Log("BossMissile Spawned at: " + transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            GameManager.instance.GameOver();
        }
    }
}
