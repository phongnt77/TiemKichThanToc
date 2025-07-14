using UnityEngine;

public class EnemyHardController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * moveSpeed;
    }
}
