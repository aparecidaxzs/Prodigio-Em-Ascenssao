using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 4f;
    public int maxHealth = 3;
    public int damageToPlayer = 1;

    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;

    private Vector3 startPos;
    private bool movingRight = true;
    private int currentHealth;
    private bool isDead = false;
    private bool isTakingDamage = false;

    void Start()
    {
        startPos = transform.position;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isDead || isTakingDamage) return;

        Patrol();
    }

    // ===========================================================
    // SISTEMA DE PATRULHA
    // ===========================================================

    void Patrol()
    {
        anim.SetBool("run", true);

        float moveDir = movingRight ? 1 : -1;
        rig.linearVelocity = new Vector2(moveDir * patrolSpeed, rig.linearVelocity.y);

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

    // ===========================================================
    // PLAYER TOMA DANO AO TOCAR NO INIMIGO
    // ===========================================================

    private void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p != null)
        {
            p.BarradeVida(-damageToPlayer);
        }
    }

    // ===========================================================
    // SISTEMA DE DANO E MORTE DO INIMIGO
    // ===========================================================

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
        anim.SetBool("run", false);

        rig.linearVelocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        rig.gravityScale = 0;

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
}
