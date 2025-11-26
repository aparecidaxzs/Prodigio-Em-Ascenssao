using UnityEngine;

public class EmpurrarEsquerda : MonoBehaviour
{
    public float força = 5f; // ajuste no Inspector

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // força negativa no eixo X = esquerda
        rb.AddForce(new Vector2(-força, 0f), ForceMode2D.Force);
    }
}
