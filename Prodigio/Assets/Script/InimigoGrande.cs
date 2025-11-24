using UnityEngine;
using System.Collections;

public class InimigoGrande : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float moveSpeed = 2f;
    public int maxHealth = 3;
    public int contactDamage = 1;
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

    [Header("PULO ALEATÓRIO")]
    public float jumpForce = 6f;
    public float randomJumpInterval = 3f;
    private float nextJumpTime = 0f;

    [Header("PULO DE ATAQUE")]
    public float attackJumpForce = 8f;
    public float attackJumpCooldown = 4f;
    private float nextAttackJump = 0f;

    [Header("Componentes")]
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    [Header("Sons do Inimigo")]
    public AudioClip somTiroInimigo;

    private bool isDead = false;
    private float lastDamageTime;
    public float contactDamageCooldown = 1f;

    // =============================
    // TELAS APÓS A MORTE
    // =============================
    [Header("TELAS APÓS MORRER")]
    public GameObject telaInicial;   // aparece ao morrer
    public GameObject telaDepois;    // aparece após 30s
    public float tempoDeEspera = 30f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        nextJumpTime = Time.time + Random.Range(2f, randomJumpInterval);
        nextAttackJump = Time.time + attackJumpCooldown;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        FollowPlayer();
        FlipToPlayer();

        TryRandomJump();
        TryAttackJump(distance);

        if (distance <= meleeRange)
            TryMeleeAttack();
        else if (distance <= detectionRange)
            TryShoot();
    }

    // ======================================================
    // MOVIMENTO
    // ======================================================
    void FollowPlayer()
    {
        float direction = player.position.x - transform.position.x;

        rig.linearVelocity = new Vector2(Mathf.Sign(direction) * moveSpeed, rig.linearVelocity.y);
        anim.SetBool("Run", true);
    }

    void FlipToPlayer()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // ======================================================
    // ATAQUE CORPO A CORPO
    // ======================================================
    void TryMeleeAttack()
    {
        if (Time.time < nextMeleeTime) return;

        nextMeleeTime = Time.time + meleeCooldown;
        rig.linearVelocity = Vector2.zero;

        anim.SetTrigger("Soco");

        StartCoroutine(MeleeDamageRoutine());
    }

    IEnumerator MeleeDamageRoutine()
    {
        yield return new WaitForSeconds(0.25f);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, meleeRange, playerLayer);

        if (hit != null)
        {
            Player.instance.BarradeVida(-contactDamage);
        }
    }

    // ======================================================
    // ATAQUE DE TIRO
    // ======================================================
    void TryShoot()
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + shootCooldown;
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        float direction = player.position.x - transform.position.x;
        bullet.GetComponent<EnemyBullet>().SetDirection(Mathf.Sign(direction));
        AudioManager.instance.PlaySFX(somTiroInimigo);
    }

    // ======================================================
    // PULO ALEATÓRIO
    // ======================================================
    void TryRandomJump()
    {
        if (Time.time < nextJumpTime) return;

        nextJumpTime = Time.time + Random.Range(1f, randomJumpInterval);

        rig.linearVelocity = new Vector2(rig.linearVelocity.x, jumpForce);
        anim.SetTrigger("Jump");
    }

    // ======================================================
    // PULO DE ATAQUE
    // ======================================================
    void TryAttackJump(float distance)
    {
        if (distance > detectionRange) return;
        if (Time.time < nextAttackJump) return;

        nextAttackJump = Time.time + attackJumpCooldown;

        StartCoroutine(AttackJumpRoutine());
    }

    IEnumerator AttackJumpRoutine()
    {
        anim.SetTrigger("Jump");

        yield return new WaitForSeconds(0.15f);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rig.linearVelocity = new Vector2(direction * moveSpeed * 2f, attackJumpForce);
    }

    // ======================================================
    // DANO POR CONTATO
    // ======================================================
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + contactDamageCooldown)
            {
                Player.instance.BarradeVida(-contactDamage);
                lastDamageTime = Time.time;
            }
        }
    }

    // ======================================================
    // TOMAR DANO + PISCAR
    // ======================================================
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        StartCoroutine(BlinkEffect());

        if (currentHealth <= 0)
            StartCoroutine(DeathRoutine());
    }

    IEnumerator BlinkEffect()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.08f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.08f);
        }
    }

    // ======================================================
    // MORTE + TELAS
    // ======================================================
    IEnumerator DeathRoutine()
    {
        isDead = true;
        rig.linearVelocity = Vector2.zero;

        anim.SetTrigger("die");

        // Espera animação da morte
        yield return new WaitForSeconds(0.4f);

        // Desativa o inimigo visualmente
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Mostra a primeira tela
        if (telaInicial != null)
            telaInicial.SetActive(true);

        // Espera 30s ou tempo configurado
        yield return new WaitForSeconds(tempoDeEspera);

        // Some a primeira
        if (telaInicial != null)
            telaInicial.SetActive(false);

        // Mostra a segunda
        if (telaDepois != null)
            telaDepois.SetActive(true);

        // Finalmente destrói o inimigo
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
