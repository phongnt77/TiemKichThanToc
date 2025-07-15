using UnityEngine;

public class ShootMediumManager : MonoBehaviour
{
    public Transform shootingPoint;
    [SerializeField] private AudioClip shootSound;

    public GameObject bulletPrefab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (shootSound != null)
            {
                GameObject tempAudio = new GameObject("ExplosionSound");
                AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
                audioSource.clip = shootSound;
                audioSource.Play();
                Destroy(tempAudio, 0.8f);
            }
            GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            bullet.AddComponent<AutoBulletDestroyWhenOffscreen>();
        }
    }
    public class AutoBulletDestroyWhenOffscreen : MonoBehaviour
    {
        void Update()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.y > 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
