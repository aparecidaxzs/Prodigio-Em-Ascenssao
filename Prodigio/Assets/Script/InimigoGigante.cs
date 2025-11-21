using UnityEngine;
using System.Collections;

public class InimigoGigante : MonoBehaviour
{
    // ================================
    // CONFIGURAÇÕES GERAIS DO INIMIGO
    // ================================

    [Header("Configurações Gerais")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.2f;              
    public float patrolDistance = 4f;
    public int maxHealth = 25;                   
    public float jumpForce = 6f;                 
    public float randomJumpInterval = 3f;        

    [Header("Modo Fúria (<10 HP)")]
    public float speedMultiplier = 1.5f;         
    private bool enraged = false;

    // ================================
    // DANO AO PLAYER
    // ================================

    public int baseDamage = 1;            
    private float damageModifier = 0.25f; // Agora causa só 25% do dano

    // ================================
    // DETECÇÃO E ATAQUE
    // ================================

    [Header("Detecção")]
    public float detectionRange = 8f;
    public float attackRange = 1.2f;
    public LayerMask playerLayer;

    [Header("Ataque Corpo a Corpo (Soco)")]
    public Transform attackPoint;
    public float attackRadius = 0.7f;
    public float attackCooldown = 1.2f;
    private float nextAttackTime = 0f;

    [Header("Ataque de Projetil")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileCooldown = 2f;
    private float nextProjectileTime = 0f;

    // ================================
    // COMPONENTES
    // ================================

    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    // ================================
    // CONTROLE INTERNO
    // ================================

    private Vector3 startPos;
    private bool movingRight = true;

    private int currentHealth;
    private bool isDead = false;
    private bool isTakingDamage = false;
    private bool isAttacking = false;

    private float nextJumpTime = 0f;

    void Start()
    {
        startPos = transform.position;

        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null || isTakingDamage) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (!enraged && currentHealth <= 10)
        {
            enraged = true;
            patrolSpeed *= speedMultiplier;
            chaseSpeed *= speedMultiplier;
            attackCooldown /= 1.3f;
            projectileCooldown /= 1.3f;
        }

        if (distanceToPlayer <= attackRange)
        {
            rig.linearVelocity = Vector2.zero;
            TryAttack();
            return;
        }

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
            TryRandomJump();
            TryProjectileAttack();
            return;
        }

        Patrol();
        TryRandomJump();
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        anim.SetBool("run", true);

        float dir = player.position.x > transform.position.x ? 1 : -1;
        rig.linearVelocity = new Vector2(dir * chaseSpeed, rig.linearVelocity.y);

        if ((dir > 0 && transform.localScale.x < 0) || (dir < 0 && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void TryAttack()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        isAttacking = true;

        // ANIMAÇÃO DO SOCO AQUI:
        // anim.SetTrigger("Punch");

        nextAttackTime = Time.time + attackCooldown;

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0.35f); 
        DealDamageToPlayer();
        yield return new WaitForSeconds(0.4f);
        isAttacking = false;
    }

    void DealDamageToPlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRadius, playerLayer);

        foreach (Collider2D col in hitPlayers)
        {
            Player p = col.GetComponent<Player>();
            if (p != null)
            {
                int finalDamage = Mathf.CeilToInt(baseDamage * damageModifier);
                p.BarradeVida(-finalDamage);
            }
        }
    }

    void TryProjectileAttack()
    {
        if (Time.time < nextProjectileTime) return;

        nextProjectileTime = Time.time + projectileCooldown;

        // ANIMAÇÃO DE PROJÉTIL AQUI:
        // anim.SetTrigger("Shoot");

        GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        proj.GetComponent<ChestShot>().SetDirection(transform.localScale.x);
    }

    void TryRandomJump()
    {
        if (Time.time < nextJumpTime) return;

        nextJumpTime = Time.time + Random.Range(randomJumpInterval - 1, randomJumpInterval + 1);

        rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);

        // ANIMAÇÃO DE PULO AQUI:
        // anim.SetTrigger("Jump");
    }

    void Patrol()
    {
        if (isAttacking) return;

        anim.SetBool("run", true);

        float moveDirection = movingRight ? 1 : -1;
        rig.linearVelocity = new Vector2(moveDirection * patrolSpeed, rig.linearVelocity.y);

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

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

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

        // ANIMAÇÃO DE MORTE AQUI:
        // anim.SetTrigger("Death");

        yield return new WaitForSeconds(1.2f);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
