using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private float healAmount = 10f;
    [SerializeField] private AudioClip collectSound;

    void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.y < 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerEasyController player = other.GetComponent<PlayerEasyController>();
            if (player != null)
            {
                player.Heal(healAmount);
            }

            if (collectSound != null)
            {
                GameObject tempAudio = new GameObject("CollectSound");
                AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
                audioSource.clip = collectSound;
                audioSource.Play();
                Destroy(tempAudio, 1f);
            }

            Destroy(gameObject);
        }
    }
}
