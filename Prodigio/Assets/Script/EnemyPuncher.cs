using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float moveSpeed = 2f;           
    public int maxHealth = 3;              
    public int contactDamage = 1;          // dano ao encostar
    private int currentHealth;             

    [Header("Detecção do Player")]
    public float detectionRange = 6f;
    public float meleeRange = 1.2f;      
    public LayerMask playerLayer;

    [Header("Ataque de Tiro")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    private float nextShootTime = 0f;

    [Header("Ataque Corpo a Corpo")]
    public float meleeCooldown = 1f;
    private float nextMeleeTime = 0f;

    [Header("Componentes")]
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    private bool isDead = false;
    private float lastDamageTime;
    public float contactDamageCooldown = 1f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Seguir o player
        FollowPlayer();

        // Virar para a direção do player
        FlipToPlayer();

        if (distance <= meleeRange)
        {
            TryMeleeAttack();
        }
        else if (distance <= detectionRange)
        {
            TryShoot();
        }
    }

    // ======================
    // PERSEGUIR O PLAYER
    // ======================
    void FollowPlayer()
    {
        float direction = player.position.x - transform.position.x;

        rig.linearVelocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rig.linearVelocity.y);

        // animação de corrida
        anim.SetBool("run", true);
    }

    // ======================
    // VIRAR PARA O PLAYER
    // ======================
    void FlipToPlayer()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // ======================
    // ATAQUE CORPO A CORPO
    // ======================
    void TryMeleeAttack()
    {
        if (Time.time < nextMeleeTime) return;

        nextMeleeTime = Time.time + meleeCooldown;

        rig.linearVelocity = Vector2.zero;

        anim.SetTrigger("punch");   // <-- ANIMAÇÃO DE SOCO

        StartCoroutine(MeleeDamageRoutine());
    }

    IEnumerator MeleeDamageRoutine()
    {
        yield return new WaitForSeconds(0.25f); // momento do impacto

        Collider2D hit = Physics2D.OverlapCircle(transform.position, meleeRange, playerLayer);

        if (hit != null)
        {
            Player.instance.BarradeVida(-contactDamage);
        }
    }

    // ======================
    // ATAQUE DE TIRO
    // ======================
    void TryShoot()
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + shootCooldown;

        anim.SetTrigger("shoot"); // <-- ANIMAÇÃO DE TIRO

        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(0.20f); // momento do disparo

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // virar projétil para a direção do player
        float direction = player.position.x - transform.position.x;
        bullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(direction));
    }

    // ======================
    // DANO POR CONTATO
    // ======================
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + contactDamageCooldown)
            {
                Player.instance.BarradeVida(-contactDamage);
                lastDamageTime = Time.time;

                anim.SetTrigger("punch");   // <-- ANIMAÇÃO OPCIONAL QUANDO ENCOSTA
            }
        }
    }

    // ======================
    // SISTEMA DE DANO
    // ======================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth > 0)
        {
            StartCoroutine(BlinkEffect());
        }
        else
        {
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator BlinkEffect()
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.enabled = true;
    }

    IEnumerator DeathRoutine()
    {
        isDead = true;
        rig.linearVelocity = Vector2.zero;

        anim.SetTrigger("die"); // <-- ANIMAÇÃO DE MORTE (se tiver)

        yield return new WaitForSeconds(0.4f);

        Destroy(gameObject);
    }

    // Gizmo do ataque melee
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
