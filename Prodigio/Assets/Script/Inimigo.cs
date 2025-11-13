using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Atributos de Vida")]
    public int maxHealth = 2;
    private int currentHealth;

    [Header("Ataque")]
    public int dano = -1; // dano que o inimigo causa
    public float tempoEntreAtaques = 2f;
    private float ultimoAtaque = 0f;

    [Header("Movimentação")]
    public float velocidade = 2f;
    public Transform[] pontosPatrulha;
    private int indiceAtual = 0;

    [Header("Detecção do Player")]
    public float distanciaDeteccao = 5f;
    private Transform player;

    private Animator anim;
    private bool estaAtacando = false; // controla se a animação de ataque está ativa

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distanciaParaPlayer = Vector2.Distance(transform.position, player.position);

        if (distanciaParaPlayer <= distanciaDeteccao)
        {
            // Detectou o player → começa a perseguição e o ataque
            PerseguirPlayer();
        }
        else
        {
            // Player longe → patrulha normal
            anim.SetBool("ataque", false);
            anim.SetBool("run", true);
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
            indiceAtual = (indiceAtual + 1) % pontosPatrulha.Length;
        }
    }

    void PerseguirPlayer()
    {
        anim.SetBool("run", false);
        anim.SetBool("ataque", true);

        // movimenta em direção ao player
        Vector2 direcao = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, velocidade * Time.deltaTime);
    }

    // CHAMADO PELO EVENTO DE ANIMAÇÃO
    public void CausarDano()
    {
        if (player == null) return;

        float distanciaParaPlayer = Vector2.Distance(transform.position, player.position);
        if (distanciaParaPlayer < 1.5f) // só aplica dano se estiver próximo o suficiente
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.BarradeVida(dano);
                Debug.Log("Inimigo causou dano ao player!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo tomou dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Destroy(gameObject);
    }
}
