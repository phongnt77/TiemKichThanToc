using UnityEngine;

public class RotatingMovement : MonoBehaviour
{
    [SerializeField]
    private Transform earth;
    [SerializeField]
    private float orbitSpeed = 20f;

    void Update()
    {
        // Quay quanh Trái đất theo trục Z (Vector3.forward)
        transform.RotateAround(earth.position, Vector3.forward, orbitSpeed * Time.deltaTime);
        earth.Rotate(Vector3.forward, orbitSpeed * Time.deltaTime);
    }
}
