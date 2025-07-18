using UnityEngine;

public class EnemyMediumController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * moveSpeed;
    }
}
