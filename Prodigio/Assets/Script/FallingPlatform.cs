using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float delay = 2f;        // Tempo antes de cair
    public float destroyTime = 3f;  // Tempo para destruir a plataforma após cair (opcional)

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Começa parada
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Invoke("DropPlatform", delay);
        }
    }

    void DropPlatform()
    {
        rb.isKinematic = false; // Ativa a queda
        Destroy(gameObject, destroyTime); // Remove da cena depois (opcional)
    }
}
