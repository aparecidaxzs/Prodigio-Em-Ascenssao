using UnityEngine;

public class Inimigo : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;

    public int dano = -1; // dano que o inimigo causa (valor negativo porque o SetVida do player espera isso)
    public float velocidade = 2f; // velocidade de movimento do inimigo
    public Transform[] pontosPatrulha; // lista de pontos que o inimigo vai patrulhar
    private int indiceAtual = 0; // índice do ponto de patrulha atual

    public float distanciaDeteccao = 5f; // distância para detectar o player
    public float tempoEntreAtaques = 2f; // tempo entre ataques (se necessário para animações futuras)
    private float ultimoAtaque = 0f;

    private Transform player; // referência ao player

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; // assume que o player tem a tag "Player"
    }

    void Update()
    {
        if (player == null) return;

        float distanciaParaPlayer = Vector2.Distance(transform.position, player.position);

        if (distanciaParaPlayer <= distanciaDeteccao)
        {
            // Player está perto: parar patrulha e perseguir
            PerseguirPlayer();
            // Aqui você pode adicionar lógica para animação de ataque com espada, se desejar
        }
        else
        {
            // Player longe: voltar à patrulha
            Patrulhar();
        }
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

    void PerseguirPlayer()
    {
        // Movimentar em direção ao player
        Vector2 direcao = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, velocidade * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo tomou dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player playerScript = collision.gameObject.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.BarradeVida(dano); // aplica dano no player (ataque com espada)
        }
    }
}
