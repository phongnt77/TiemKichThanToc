using UnityEngine;

public class EnemyEasyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * moveSpeed;
    }
}
