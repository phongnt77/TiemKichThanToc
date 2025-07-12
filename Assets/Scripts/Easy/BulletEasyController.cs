using UnityEngine;

public class BulletEasyController : MonoBehaviour
{
    public float bulletSpeed = 25f;

    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;

    [Header("Item Drop Settings")]
    [SerializeField] private GameObject[] dropItems;
    [SerializeField][Range(0f, 1f)] private float dropChance = 0.1f;

    void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (explosionEffect != null)
            {
                GameObject explosionEffectDes = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosionEffectDes, 0.5f);
            }

            if (explosionSound != null)
            {
                GameObject tempAudio = new GameObject("ExplosionSound");
                AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
                audioSource.clip = explosionSound;
                audioSource.Play();
                Destroy(tempAudio, 1f);
            }

            if (dropItems.Length > 0 && Random.value < dropChance)
            {
                int randomIndex = Random.Range(0, dropItems.Length);
                GameObject itemToDrop = dropItems[randomIndex];
                Instantiate(itemToDrop, transform.position, Quaternion.identity);
            }
            EasyManager.Instance?.AddScore(10);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
