using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Configurações do Parallax")]
    [Tooltip("0 = acompanha totalmente a câmera, 1 = imóvel")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    private Transform cam;
    private float startPosX;

    void Start()
    {
        cam = Camera.main.transform;
        startPosX = transform.position.x;
    }

    void Update()
    {
        // movimento no sentido contrário da câmera
        float distance = cam.position.x * -parallaxEffect;

        // move apenas no eixo X, mantendo Y e Z fixos
        transform.position = new Vector3(startPosX + distance, transform.position.y, transform.position.z);
    }
}
