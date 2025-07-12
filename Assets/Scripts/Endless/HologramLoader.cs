using UnityEngine;

public class HologramLoader : MonoBehaviour
{
    [Header("Sprite GameObjects")]
    public Transform outerRing;      // GameObject với SpriteRenderer
    public Transform middleRing;     // GameObject với SpriteRenderer
    public Transform innerRing;      // GameObject với SpriteRenderer
    public Transform coreTriangle;   // GameObject với SpriteRenderer

    [Header("Rotation Speeds")]
    public float outerSpeed = 30f;
    public float middleSpeed = -45f;
    public float innerSpeed = 60f;

    void Update()
    {
        // Xoay GameObjects
        outerRing.Rotate(0, 0, outerSpeed * Time.deltaTime);
        middleRing.Rotate(0, 0, middleSpeed * Time.deltaTime);
        innerRing.Rotate(0, 0, innerSpeed * Time.deltaTime);

        // Pulse effect cho core
        float pulse = 1f + Mathf.Sin(Time.time * 2f) * 0.2f;
        coreTriangle.localScale = Vector3.one * pulse;
    }
}
