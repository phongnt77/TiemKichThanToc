using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 90f; // độ/giây

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
