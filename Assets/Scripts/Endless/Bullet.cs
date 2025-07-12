using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 25f;
    [HideInInspector] 
    public ObjectPoolManager bulletPool;
  
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);


        // Nếu ra khỏi màn hình thì trả về pool
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);


        ///Theo định nghĩa thì viewport có giá trị từ 0 đến 1 (trục X từ 0 đến 1, trục Y cũng từ 0 đến 1).
        if (viewPos.y > 1.1f || viewPos.y < -0.1f || viewPos.x > 1.1f || viewPos.x < -0.1f)
        {
            //ObjectPoolManager.Instance.ReturnObject(gameObject);
            bulletPool.ReturnObject(gameObject);
        }
    }

    // Nếu viên đạn va chạm với vật thể nào đó
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Missile hit: " + other.name);

        if (other.gameObject == gameObject)
        {
            return;
        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit Asteroid");
            if (GameManagerEndless.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManagerEndless.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }

            if (GameManagerEndless.instance != null)
            {
                GameManagerEndless.instance.AddScore(10);
            }

            bulletPool.ReturnObject(gameObject);
        }
        else if (other.gameObject.CompareTag("BulletEnemy"))
        {
            if (GameManagerEndless.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManagerEndless.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
                if (GameManagerEndless.instance != null)
                {
                    GameManagerEndless.instance.AddScore(5);
                }
            }
            bulletPool.ReturnObject(gameObject);
        }
        else if (other.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Bullet hit Asteroid");
            if (GameManagerEndless.instance?.explosionEffect != null)
            {
                GameObject explosion = Instantiate(GameManagerEndless.instance.explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }
            if (GameManagerEndless.instance != null)
            {
                GameManagerEndless.instance.AddScore(10);
            }
            bulletPool.ReturnObject(gameObject);
        }

        
    }

}
