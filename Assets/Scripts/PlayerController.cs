using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float missileLifeTime = 5f;

    [Header("Muzzle Effect")]
    public Transform muzzleSpawnPoint;

    void Update()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, y, 0) * speed * Time.deltaTime;
        transform.Translate(move);

        // Giới hạn vùng chơi
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.5f, 2.5f);
        pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);
        transform.position = pos;
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (missilePrefab != null && missileSpawnPoint != null)
            {
                GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, Quaternion.identity);
                Destroy(missile, missileLifeTime);
            }

            if (GameManager.instance != null && GameManager.instance.muzzleFlash != null && muzzleSpawnPoint != null)
            {
                GameObject muzzle = Instantiate(GameManager.instance.muzzleFlash, muzzleSpawnPoint.position, Quaternion.identity);
                Destroy(muzzle, missileLifeTime);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            if (GameManager.instance != null)
                GameManager.instance.GameOver();
        }
    }
}
