using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // ================================
    // CONFIGURAÇÕES GERAIS DO INIMIGO
    // ================================

    [Header("Configurações Gerais")]
    public float patrolSpeed = 2f;           // Velocidade enquanto patrulha
    public float patrolDistance = 4f;        // Distância que ele anda para cada lado
    public int maxHealth = 3;                // Vida máxima
    public int damageToPlayer = 1;           // Dano ao encostar no player

    // ================================
    // DETECÇÃO E ATAQUE
    // ================================

    [Header("Detecção do Player")]
    public float detectionRange = 3f;        // Distância para ativar o ataque
    public float attackRange = 1.2f;         // Distância para causar dano
    public LayerMask playerLayer;            // Layer do player para detectar no ataque

    [Header("Ataque")]
    public float attackCooldown = 1.2f;      // Tempo entre ataques
    private float nextAttackTime = 0f;       // Controle interno do cooldown
    public Transform attackPoint;            // Ponto de onde o golpe sai
    public float attackRadius = 0.7f;        // Tamanho da área de dano

    // ================================
    // COMPONENTES
    // ================================

    private Animator anim;                   // Controla animações
    private Rigidbody2D rig;                 // Controla movimento físico
    private SpriteRenderer spriteRenderer;   // Para piscar ao tomar dano
    private Transform player;                // Referência ao player

    // ================================
    // CONTROLE INTERNO
    // ================================

    private Vector3 startPos;                // Ponto inicial para definir patrulha
    private bool movingRight = true;         // Direção da patrulha
    private int currentHealth;               // Vida atual
    private bool isDead = false;             // Controle de morte
    private bool isTakingDamage = false;     // Impede ações durante dano
    private bool isAttacking = false;        // Evita múltiplos ataques sobrepostos

    void Start()
    {
        startPos = transform.position;                      // Salva posição inicial
        rig = GetComponent<Rigidbody2D>();                  // Pega Rigidbody2D
        anim = GetComponent<Animator>();                    // Pega Animator
        spriteRenderer = GetComponent<SpriteRenderer>();    // Pega SpriteRenderer
        currentHealth = maxHealth;                          // Define vida inicial

        // Procura o player na cena (tag "Player")
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null || isTakingDamage) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Se o player estiver dentro do alcance → entra no modo de ataque
        if (distanceToPlayer <= detectionRange)
        {
            AttackBehavior(distanceToPlayer);
        }
        else
        {
            // Caso contrário, permanece patrulhando
            Patrol();
        }
    }

    // ===========================================================
    // SISTEMA DE PATRULHA (ANDANDO PARA OS DOIS LADOS SEM PARAR)
    // ===========================================================

    void Patrol()
    {
        if (isAttacking) return;

        anim.SetBool("run", true);

        float moveDirection = movingRight ? 1 : -1;
        rig.linearVelocity = new Vector2(moveDirection * patrolSpeed, rig.linearVelocity.y);

        // Verifica se atingiu limite da patrulha
        if (movingRight && transform.position.x >= startPos.x + patrolDistance)
            Flip();
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance)
            Flip();
    }

    // Inverte a direção do inimigo
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ===========================================================
    // ATAQUE — o inimigo NÃO persegue o player, apenas ataca se perto
    // ===========================================================

    void AttackBehavior(float distanceToPlayer)
    {
        // Para de correr
        anim.SetBool("run", false);

        // Apenas ataca se estiver perto o suficiente
        if (distanceToPlayer <= attackRange)
        {
            rig.linearVelocity = Vector2.zero;
            TryAttack();
        }
        else
        {
            // Se detectar o player mas estiver longe, não faz nada
            rig.linearVelocity = Vector2.zero;
        }
    }

    void TryAttack()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        isAttacking = true;
        anim.SetTrigger("ataque");
        nextAttackTime = Time.time + attackCooldown;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.4f);  // Momento exato do golpe
        DealDamageToPlayer();
        yield return new WaitForSeconds(0.5f);  // Espera a animação terminar
        isAttacking = false;
    }

    void DealDamageToPlayer()
    {
        // Detecta o player na área de ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D col in hitPlayers)
        {
            Player p = col.GetComponent<Player>();
            if (p != null)
            {
                p.BarradeVida(-damageToPlayer);
            }
        }
    }

    public void Die()
{
    if (!isDead)
    {
        StartCoroutine(DeathEffect());
    }
}



    // ===========================================================
    // SISTEMA DE DANO E MORTE
    // ===========================================================

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        // Ainda está vivo → pisca
        if (currentHealth > 0)
        {
            StartCoroutine(BlinkEffect(0.1f, 3));
        }
        else
        {
            StartCoroutine(DeathEffect());
        }
    }

    IEnumerator BlinkEffect(float speed, int count)
    {
        isTakingDamage = true;

        for (int i = 0; i < count; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(speed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(speed);
        }

        isTakingDamage = false;
    }

    IEnumerator DeathEffect()
    {
        isDead = true;
        rig.linearVelocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.gravityScale = 0;
        GetComponent<Collider2D>().enabled = false;

        // Piscando antes de sumir
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }

    // Gizmos para visualizar alcance de ataque no editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
