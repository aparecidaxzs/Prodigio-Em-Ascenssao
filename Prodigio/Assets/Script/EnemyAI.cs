using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float patrolDistance = 4f;
    public int maxHealth = 3;
    public int damageToPlayer = 1;

    [Header("Detecção do Player")]
    public float detectionRange = 6f;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;

    [Header("Ataque")]
    public float attackCooldown = 1.5f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRadius = 0.8f;

    [Header("Referências")]
    private Animator anim;
    private Rigidbody2D rig;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;
    private bool movingRight = true;
    private int currentHealth;
    private bool isDead = false;
    private bool isTakingDamage = false;
    private bool isAttacking = false;

    // Nova variável para garantir patrulha inicial
    private bool initialPatrol = true;
    public float initialPatrolTime = 2f; // Tempo em segundos para patrulhar no início do jogo

    void Start()
    {
        startPos = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Inicia a patrulha inicial e agenda o fim dela
        Invoke("EndInitialPatrol", initialPatrolTime);
    }

    void EndInitialPatrol()
    {
        initialPatrol = false;
    }

    void Update()
    {
        if (isDead || player == null || isTakingDamage) return;

        if (initialPatrol)
        {
            Patrol();
            return; // Força patrulha no início, ignorando detecção
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    // Resto do código permanece igual...
    void Patrol()
    {
        if (isAttacking) return;

        anim.SetBool("run", true);
        float move = movingRight ? 1 : -1;
        rig.linearVelocity = new Vector2(move * patrolSpeed, rig.linearVelocity.y);

        if (movingRight && transform.position.x >= startPos.x + patrolDistance)
            Flip();
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance)
            Flip();
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (isAttacking) return;

        if (distanceToPlayer > attackRange)
        {
            anim.SetBool("run", true);
            anim.SetBool("ataque", false);

            Vector2 direction = (player.position - transform.position).normalized;
            rig.linearVelocity = new Vector2(direction.x * chaseSpeed, rig.linearVelocity.y);

            // vira na direção do player
            if ((direction.x > 0 && transform.localScale.x < 0) ||
                (direction.x < 0 && transform.localScale.x > 0))
                Flip();
        }
        else
        {
            rig.linearVelocity = Vector2.zero;
            anim.SetBool("run", false);
            AttackPlayer();
        }
    }

    void AttackPlayer()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        isAttacking = true;
        anim.SetTrigger("ataque");
        nextAttackTime = Time.time + attackCooldown;

        // Dano aplicado no meio da animação
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.4f); // tempo do golpe
        DealDamageToPlayer();
        yield return new WaitForSeconds(0.5f); // espera fim da animação
        isAttacking = false;
    }

    void DealDamageToPlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider2D playerCol in hitPlayers)
        {
            Player playerScript = playerCol.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.BarradeVida(-damageToPlayer);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            StartCoroutine(BlinkEffect(0.1f, 3)); // pisca ao levar dano
        }
        else
        {
            StartCoroutine(DieBlink());
        }
    }

    IEnumerator BlinkEffect(float blinkSpeed, int times)
    {
        isTakingDamage = true;
        for (int i = 0; i < times; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkSpeed);
        }
        isTakingDamage = false;
    }

    IEnumerator DieBlink()
    {
        isDead = true;
        rig.linearVelocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic; // impede queda
        rig.gravityScale = 0f;                    // remove gravidade
        GetComponent<Collider2D>().enabled = false;

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.BarradeVida(-damageToPlayer);
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            currentHealth = 0; // Garante que está morto
            StartCoroutine(DieBlink());
        }
    }
}