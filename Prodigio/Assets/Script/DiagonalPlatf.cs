using UnityEngine;

public class DiagonalPlatf : MonoBehaviour
{
    public float speed = 2f; // Velocidade geral
    public float horizontalDistance = 5f; // Distância horizontal (esquerda-direita)
    public float verticalAmplitude = 2f; // Amplitude vertical (altura do sobe/desce)

    private Vector3 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        // Movimento horizontal oscilante (ida e volta)
        float horizontalMovement = Mathf.PingPong(Time.time * speed, horizontalDistance);
        
        // Movimento vertical sinusoidal (sobe e desce sincronizado)
        float verticalMovement = Mathf.Sin(Time.time * speed * Mathf.PI / horizontalDistance) * verticalAmplitude;
        
        // Aplica o movimento diagonal
        Vector3 newPosition = startPosition + new Vector3(horizontalMovement, verticalMovement, 0f);
        rb.MovePosition(newPosition);
    }
}