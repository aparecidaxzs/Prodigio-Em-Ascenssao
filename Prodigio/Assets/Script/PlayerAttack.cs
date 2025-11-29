using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque Corpo-a-Corpo (W)")]
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public float meleeCooldown = 0.6f;

    [Header("Ataque Especial (Q - Projétil)")]
    public Transform handPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int projectileDamage = 1;
    public LayerMask projectileCollisionLayers;
    public float specialAttackCooldown = 0.5f;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private float nextMeleeTime = 0f;
    private float nextSpecialAttackTime = 0f;
    private bool isMeleeing = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //ataque corpo a corpo
        if (Input.GetKeyDown(KeyCode.W) && Time.time >= nextMeleeTime && !isMeleeing)
        {
            StartCoroutine(DoMeleeAttack());
            nextMeleeTime = Time.time + meleeCooldown;
        }

        
        if (Input.GetKeyDown(KeyCode.Q) && Time.time >= nextSpecialAttackTime)
        {
            bool canShoot = true;
            if (CoinManager.instance != null)
                canShoot = CoinManager.instance.UseShot();

            if (canShoot)
            {
                StartCoroutine(DoSpecialAttack());
                nextSpecialAttackTime = Time.time + specialAttackCooldown;
            }
        }
    }

    
    IEnumerator DoMeleeAttack()
    {
        isMeleeing = true;

        // LIGA ANIMAÇÃO DE ATAQUE
        anim.SetBool("AtaqueCorpo", true);

        // DANO IMEDIATO (pode trocar por evento na animação)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI e = enemy.GetComponent<EnemyAI>();
            if (e != null)
                e.TakeDamage(attackDamage);
            
            // Inimigo grande
        InimigoGrande grande = enemy.GetComponent<InimigoGrande>();
        if (grande != null)
        grande.TakeDamage(attackDamage);
        }

        // Espera 1 frame para pegar a animação certa
        //yield return null;

        // PEGA DURAÇÃO DO CLIP
        float animDuration = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(animDuration);

        // DESLIGA ANIMAÇÃO
        anim.SetBool("AtaqueCorpo", false);

        isMeleeing = false;
    }

    
    IEnumerator DoSpecialAttack()
    {
        // LIGA ANIMAÇÃO
        anim.SetBool("AtaqueCyberLuva", true);

        // INSTANCIA PROJÉTIL
        GameObject projectile = Instantiate(projectilePrefab, handPoint.position, Quaternion.identity);

        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            float dir = spriteRenderer.flipX ? -1f : 1f;
            projScript.Initialize(dir * projectileSpeed, projectileDamage, projectileCollisionLayers);
        }

        // Espera 1 frame para pegar animação certa
        yield return null;

        float animDuration = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

        yield return new WaitForSeconds(animDuration);

        // DESLIGA ANIMAÇÃO
        anim.SetBool("AtaqueCyberLuva", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
