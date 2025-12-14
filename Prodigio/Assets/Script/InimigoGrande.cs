using UnityEngine;
using System.Collections;

public class InimigoGrande : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public int maxHealth = 3;
    public int contactDamage = 1;
    private int currentHealth;

    [Header("Movimento Inicial")]
    public float walkDistance = 1.2f;
    public float walkSpeed = 1.5f;
    private Vector3 startPosition;
    private bool terminouMovimentoInicial = false;

    [Header("Detecção do Player")]
    public float detectionRange = 6f;
    public LayerMask playerLayer;

    [Header("Ataque de Tiro")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 2f;
    private float nextShootTime = 0f;

    [Header("Componentes")]
    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Transform player;

    private bool isDead = false;
    private float lastDamageTime;
    public float contactDamageCooldown = 1f;

    [Header("TELAS APÓS MORRER")]
    public GameObject telaInicial;
    public GameObject telaDepois;
    public float tempoDeEspera = 30f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        startPosition = transform.position;

        // Começa andando
        anim.SetBool("Andando", true);
    }

    void Update()
    {
        if (isDead) return;

        // Movimento inicial
        if (!terminouMovimentoInicial)
        {
            AndarInicial();
            return;
        }

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            VirarParaPlayer();
            TryShoot();
        }
    }

    void AndarInicial()
    {
        transform.Translate(Vector2.right * walkSpeed * Time.deltaTime);

        float distanceWalked = Vector2.Distance(startPosition, transform.position);

        if (distanceWalked >= walkDistance)
        {
            terminouMovimentoInicial = true;
            anim.SetBool("Andando", false);
            rig.linearVelocity = Vector2.zero;
        }
    }

    void VirarParaPlayer()
    {
        if (player.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    void TryShoot()
    {
        if (Time.time < nextShootTime) return;

        nextShootTime = Time.time + shootCooldown;
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        if (firePoint == null || bulletPrefab == null || player == null) yield break;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        float direction = player.position.x - transform.position.x;
        var enemyBullet = bullet.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
            enemyBullet.SetDirection(Mathf.Sign(direction));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.collider.CompareTag("Player"))
        {
            if (Player.instance != null &&
                Time.time >= lastDamageTime + contactDamageCooldown)
            {
                Player.instance.TomarDano(contactDamage);
                lastDamageTime = Time.time;
            }
        }
    }

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

    IEnumerator DeathRoutine()
    {
        isDead = true;
        rig.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.4f);

        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (telaInicial != null)
            telaInicial.SetActive(true);

        yield return new WaitForSeconds(tempoDeEspera);

        if (telaInicial != null)
            telaInicial.SetActive(false);

        if (telaDepois != null)
            telaDepois.SetActive(true);

        Destroy(gameObject);
    }
}
