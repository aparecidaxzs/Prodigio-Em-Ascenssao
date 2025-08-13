using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int dano = -1; // Dano que o inimigo causa (valor negativo porque seu SetVida espera isso)
    public float velocidade = 2f;
    public Transform[] pontosPatrulha;
    private int indiceAtual = 0;

    void Update()
    {
        Patrulhar();
    }

    void Patrulhar()
    {
        if (pontosPatrulha.Length == 0) return;

        Transform alvo = pontosPatrulha[indiceAtual];
        transform.position = Vector2.MoveTowards(transform.position, alvo.position, velocidade * Time.deltaTime);

        if (Vector2.Distance(transform.position, alvo.position) < 0.1f)
        {
            indiceAtual++;
            if (indiceAtual >= pontosPatrulha.Length)
                indiceAtual = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.SetVida(dano);
        }
    }
}
