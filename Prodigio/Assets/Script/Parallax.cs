using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos; // posição inicial
    private Transform cam;  // câmera

    public float parallaxEffect; // intensidade do efeito

    void Start()
    {
        startPos = transform.position.x;
        cam = Camera.main.transform;
    }

    void Update()
    {
        float distance = cam.position.x * parallaxEffect;

        transform.position = new Vector3(
            startPos + distance,
            transform.position.y,
            transform.position.z
        );
    }
}
