using UnityEngine;

public class ParallaxSimple : MonoBehaviour
{
    [Header("Câmera")]
    public Transform cam;

    [Header("Intensidade do Parallax")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float distX = cam.position.x * parallaxEffect;
        float distY = cam.position.y * parallaxEffect;

        transform.position = new Vector3(
            startPos.x + distX,
            startPos.y + distY,
            transform.position.z
        );
    }
}
