using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 2f; // Tempo em segundos antes de cair
    private Rigidbody2D rb;
    private bool isFalling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Mantém kinematic até ativar
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isFalling)
        {
            // Verifica se o jogador está em cima (colisão de cima)
            if (collision.contacts[0].normal.y < -0.5f) // Normal aponta para baixo, indicando que o jogador está em cima
            {
                Invoke("StartFalling", fallDelay); // Inicia timer
                isFalling = true;
            }
        }
    }

    void StartFalling()
    {
        rb.isKinematic = false; // Torna dinâmico para cair com gravidade
        // Opcional: Adicione efeitos, como som ou partícula
    }

    // Opcional: Reset para reutilizar (ex.: se quiser que ela volte após cair)
    void OnBecameInvisible()
    {
        // Reseta a plataforma quando sai da tela
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        isFalling = false;
        // Reposicione se necessário: transform.position = startPosition;
    }
}