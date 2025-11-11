using UnityEngine;
using System.Collections; // Adicionado para usar Coroutine

public class EnemyAI : MonoBehaviour
{
    [Header("Movimentação e Patrulha")]
    public float patrolSpeed = 2f;          // Velocidade de patrulha
    public float patrolDistance = 5f;       // Distância máxima de patrulha (ida e volta)
    private Vector3 startPosition;           // Posição inicial
    private bool movingRight = true;        // Direção inicial

    [Header("Detecção e Perseguição")]
    public float detectionRange = 3f;       // Alcance para detectar o Player
    public float yThreshold = 0.5f;         // Diferença máxima no eixo Y para perseguir (foca no eixo X)
    public float chaseSpeed = 3f;           // Velocidade ao perseguir
    private Transform player;               // Referência ao Player
    private bool isChasing = false;         // Se está perseguindo

    [Header("Ataque e Dano")]
    public int dano = -1;                   // Dano causado ao Player (valor negativo, como no seu script)
    public float attackAnimationDuration = 0.5f; // Duração da animação de ataque
    private bool isAttacking = false;       // Flag para controlar se está atacando (evita conflitos)

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

        // Verifica se o Player está no alcance de detecção e alinhado no eixo Y
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float yDifference = Mathf.Abs(player.position.y - transform.position.y);
        isChasing = distanceToPlayer <= detectionRange && yDifference <= yThreshold; // Só persegue se Y estiver próximo

        if (isChasing && !isAttacking) // Não persegue se estiver atacando
        {
            // Persegue o Player (foco no eixo X)
            ChasePlayer();
        }
        else if (!isAttacking) // Só patrulha se não estiver atacando
        {
            // Patrulla
            Patrol();
        }

        // Atualiza animações (sempre, para evitar travamentos)
        UpdateAnimations();
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
        // Move em direção ao Player (apenas no eixo X, já que Y é verificado)
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y); // Mantém Y inalterado
    }

    // Dano via colisão (baseado no seu script)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player playerScript = collision.gameObject.GetComponent<Player>();
        if (playerScript != null && !isAttacking) // Só ataca se não estiver já atacando
        {
            // Vira o sprite para o lado do Player durante o ataque
            FlipTowardsPlayer();

            playerScript.BarradeVida(dano); // Aplica dano no Player
            Debug.Log("Inimigo deu dano ao Player!");

            // Ativa animação de ataque (só quando detectado via colisão)
            if (anim != null)
            {
                isAttacking = true; // Flag para bloquear outras ações
                anim.SetBool("ataque", true);
                Debug.Log("Animação 'ataque' ativada!");
                StartCoroutine(ResetAttackAnimation());
            }
        }
    }

    // Coroutine para resetar a animação de ataque
    IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(attackAnimationDuration);
        if (anim != null)
        {
            anim.SetBool("ataque", false);
            Debug.Log("Animação 'ataque' resetada!");
        }
        isAttacking = false; // Libera para outras ações
    }

    // Método para atualizar animações (centralizado para evitar conflitos)
    void UpdateAnimations()
    {
        if (anim == null) return;

        bool isMoving = rb.linearVelocity.magnitude > 0.1f;

        // "run": Ativado sempre que estiver se movendo (patrulha OU perseguição), mas não durante ataque
        bool runState = isMoving && !isAttacking;
        anim.SetBool("run", runState);

        // Logs para debug (remova em produção)
        if (runState) Debug.Log("Estado: run (ativado na patrulha ou perseguição)");
        if (isAttacking) Debug.Log("Estado: ataque");
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

    void FlipTowardsPlayer()
    {
        // Vira o sprite baseado na posição do Player
        if (player.position.x > transform.position.x)
        {
            // Player à direita: vira para a direita (escala X positiva)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            // Player à esquerda: vira para a esquerda (escala X negativa)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Gizmos para visualizar áreas no Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Área de detecção
    }
}