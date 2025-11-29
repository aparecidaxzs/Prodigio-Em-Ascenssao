using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Configurações Gerais")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;
    public int maxHealth = 3;
    public int damageToPlayer = 1;

    private int currentHealth;
    private Vector3 startPos;
    private bool movingRight = true;
    private bool isDead = false;
    private bool isTakingDamage = false;

    private Rigidbody2D rig;
    private SpriteRenderer sr;
    private Animator anim;

    void Start()
    {
        startPos = transform.position;
        currentHealth = maxHealth;

        rig = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead && !isTakingDamage)
            Patrol();
    }

    void Patrol()
    {
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        Player p = col.gameObject.GetComponent<Player>();

        if (p != null)
        {
            ShieldAbility shield = col.gameObject.GetComponent<ShieldAbility>();
            if (shield != null && shield.GetShieldState())
                return;

            p.TomarDano(damageToPlayer);
        }
    }

    // ======================================================
    // TOMAR DANO — FUNCIONA COM QUALQUER PROJÉTIL
    // ======================================================
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
            Die();
        }
    }

    IEnumerator BlinkEffect(float speed, int count)
    {
        isTakingDamage = true;
        anim.SetBool("run", false);
        rig.linearVelocity = Vector2.zero;

        for (int i = 0; i < count; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(speed);
            sr.enabled = true;
            yield return new WaitForSeconds(speed);
        }

        isTakingDamage = false;
    }

    public void Die()
    {
        if (!isDead)
            StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        isDead = true;
        anim.SetBool("run", false);
        rig.linearVelocity = Vector2.zero;

        GetComponent<Collider2D>().enabled = false;
        rig.bodyType = RigidbodyType2D.Kinematic;

        for (int i = 0; i < 3; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        Destroy(gameObject);
    }
}
