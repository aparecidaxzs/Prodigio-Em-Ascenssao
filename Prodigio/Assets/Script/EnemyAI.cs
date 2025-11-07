using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movimentação e Patrulha")]
    public float patrolSpeed = 2f;          // Velocidade de patrulha
    public float patrolDistance = 5f;       // Distância máxima de patrulha (ida e volta)
    private Vector3 startPosition;           // Posição inicial
    private bool movingRight = true;        // Direção inicial

    [Header("Detecção e Perseguição")]
    public float detectionRange = 3f;       // Alcance para detectar o Player
    public float chaseSpeed = 3f;           // Velocidade ao perseguir
    private Transform player;               // Referência ao Player
    private bool isChasing = false;         // Se está perseguindo

    [Header("Ataque")]
    public float attackRange = 0.5f;        // Raio da área de ataque
    public int attackDamage = 1;            // Dano causado ao Player
    public float attackRate = 1f;           // Ataques por segundo
    public Transform attackPoint;           // Ponto de ataque (empty child)
    public LayerMask playerLayer;           // Layer do Player
    private float nextAttackTime = 0f;

    [Header("Vida do Inimigo")]
    public int maxHealth = 3;               // Vida máxima
    private int currentHealth;

    private Rigidbody2D rb;
    private Animator anim;                  // Opcional, para animações

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();    // Se não tiver, pode remover
        startPosition = transform.position;
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Encontra o Player pela tag
    }

    void Update()
    {
        if (player == null) return; // Se não encontrou o Player, não faz nada

        // Verifica se o Player está no alcance de detecção
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= detectionRange;

        if (isChasing)
        {
            // Persegue o Player
            ChasePlayer();
            // Tenta atacar se estiver próximo
            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else
        {
            // Patrulla
            Patrol();
        }

        // Animações (opcional)
        if (anim != null)
        {
            anim.SetBool("isWalking", rb.linearVelocity.magnitude > 0.1f);
            anim.SetBool("isAttacking", isChasing && distanceToPlayer <= attackRange);
        }
    }

    void Patrol()
    {
        // Move para a direita ou esquerda
        if (movingRight)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                movingRight = false;
                Flip(); // Vira o sprite
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                movingRight = true;
                Flip(); // Vira o sprite
            }
        }
    }

    void ChasePlayer()
    {
        // Move em direção ao Player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);

        // Vira o sprite baseado na direção
        if (direction.x > 0 && transform.eulerAngles.y != 0)
            Flip();
        else if (direction.x < 0 && transform.eulerAngles.y == 0)
            Flip();
    }

    void Attack()
    {
        // Detecta o Player na área de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D playerHit in hitPlayers)
        {
            if (playerHit.CompareTag("Player"))
            {
                // Causa dano ao Player usando o método existente
                Player.instance.BarradeVida(-attackDamage);
                Debug.Log("Inimigo atacou o Player!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Inimigo recebeu dano! Vida restante: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aqui você pode adicionar efeitos, como animação de morte ou som
        Destroy(gameObject);
    }

    void Flip()
    {
        // Vira o sprite (inverte a escala X)
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Gizmos para visualizar áreas no Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Área de detecção

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange); // Área de ataque
        }
    }
}
