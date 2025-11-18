using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f; // Velocidade do movimento
    public float distance = 5f; // Distância total de movimento (ida e volta)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Salva a posição inicial
    }

    void Update()
    {
        // Movimento oscilante usando Mathf.PingPong para ir e voltar
        float movement = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPosition + new Vector3(movement, 0f, 0f);
    }
}