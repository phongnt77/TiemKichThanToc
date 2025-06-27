using UnityEngine;

public class BeamController : MonoBehaviour
{
    public float duration = 2f; // Th?i gian Beam t?n t?i

    void Start()
    {
        Destroy(gameObject, duration); // T? h?y sau duration
        Debug.Log("Beam started at: " + transform.position + ", will destroy in: " + duration + " seconds");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.name + ", Tag: " + other.tag + " at: " + transform.position);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by Beam confirmed at: " + transform.position + ", Object: " + other.gameObject);
            if (GameManager.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManager.instance.explosionEffect, other.transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
                Debug.Log("Explosion spawned at: " + other.transform.position);
            }
            if (other.gameObject != null)
            {
                Destroy(other.gameObject); // H?y player
                Debug.Log("Player destroyed by Beam at: " + Time.time);
            }
            else
            {
                Debug.LogError("other.gameObject is null during collision!");
            }
            if (GameManager.instance != null)
            {
                Debug.Log("Calling GameOver() at: " + Time.time);
                GameManager.instance.GameOver();
            }
            else
            {
                Debug.LogError("GameManager.instance is null!");
            }
        }
        else
        {
            Debug.LogWarning("Collision with non-Player object: " + other.name);
        }
    }
}