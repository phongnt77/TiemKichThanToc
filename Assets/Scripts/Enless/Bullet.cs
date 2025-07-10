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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == gameObject || other.CompareTag("Bullet"))
            return;
        //Debug.Log("Bullet collided with: " + other.name); // Ở vị trí ko di chuyển thì nó tự động gom nên phải check 
        //ObjectPoolManager.Instance.ReturnObject(gameObject);
        bulletPool.ReturnObject(gameObject);
    }
}
