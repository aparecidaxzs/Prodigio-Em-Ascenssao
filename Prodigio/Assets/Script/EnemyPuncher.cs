using UnityEngine;
using System.Collections;

public class EnemyPuncher : MonoBehaviour
{
    [Header("Movimentação e Patrulha")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 4f;

    [Header("Vida")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Detecção e Ataque")]
    public float detectionRange = 4f;  
    public float punchRange = 0.8f;
    public float shootRange = 3f;      
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    [Header("SOCOS")]
    public Transform punchPoint;
    public float punchRadius = 0.5f;
    public LayerMask playerLayer;

    [Header("TIRO")]
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;

    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spr;
    private Transform player;

    private bool movingRight = true;
    private Vector3 startPos;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isTakingDamage = false;

    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || isTakingDamage) return;
        if (player == null) { Patrol(); return; }

        float dist = Vector2.Distance(transform.position, player.position);

        // Decide qual ataque usar
        if (dist <= punchRange)
            PunchAttack();
        else if (dist <= shootRange)
            ShootAttack();
        else if (dist <= detectionRange)
            rig.linearVelocity = Vector2.zero;
        else
            Patrol();
    }

    // ============================================================
    // PATRULHA
    // ============================================================
    void Patrol()
    {
        if (isAttacking) return;

        anim.SetBool("run", true);

        float dir = movingRight ? 1 : -1;
        rig.linearVelocity = new Vector2(dir * patrolSpeed, rig.linearVelocity.y);

        if (movingRight && transform.position.x >= startPos.x + patrolDistance)
            Flip();
        else if (!movingRight && transform.position.x <= startPos.x - patrolDistance)
            Flip();
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    // ============================================================
    // ATAQUE – SOCOS
    // ============================================================
    void PunchAttack()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetTrigger("punch");

        StartCoroutine(PunchRoutine());
    }

    IEnumerator PunchRoutine()
    {
        rig.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.2f);
        PunchHit();

        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    void PunchHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(punchPoint.position, punchRadius, playerLayer);

        if (hit != null)
        {
            Player p = hit.GetComponent<Player>();
            if (p != null)
                p.BarradeVida(-1);
        }
    }

    // ============================================================
    // ATAQUE – TIRO
    // ============================================================
    void ShootAttack()
    {
        if (Time.time < nextAttackTime || isAttacking) return;

        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        anim.SetTrigger("shoot");

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        rig.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.25f); // momento do disparo

        ShootProjectile();

        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    void ShootProjectile()
    {
        GameObject b = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Direção do tiro baseada na escala
        float direction = transform.localScale.x > 0 ? 1 : -1;

        b.GetComponent<Bullet>().Setup(direction, bulletSpeed);
    }

    // ============================================================
    // DANO E MORTE
    // ============================================================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth > 0)
            StartCoroutine(Blink());
        else
            StartCoroutine(Die());
    }

    IEnumerator Blink()
    {
        isTakingDamage = true;

        for (int i = 0; i < 3; i++)
        {
            spr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        isTakingDamage = false;
    }

    IEnumerator Die()
    {
        isDead = true;
        rig.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = false;

        for (int i = 0; i < 3; i++)
        {
            spr.enabled = false;
            yield return new WaitForSeconds(0.15f);
            spr.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (punchPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
        }

        if (shootPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(shootPoint.position, 0.15f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
