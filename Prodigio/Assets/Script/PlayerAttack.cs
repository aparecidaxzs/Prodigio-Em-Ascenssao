using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração do Ataque Normal (W)")]
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Cooldown do Ataque Normal")]
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    [Header("Configuração do Ataque Especial (Q - Luva)")]
    public Transform handPoint; // ponto de onde o tiro sai (mão do player)
    public GameObject projectilePrefab; // prefab do projétil
    public float projectileSpeed = 10f; // velocidade do projétil
    public int projectileDamage = 1; // dano do projétil
    public LayerMask projectileCollisionLayers; // layers para colisão (inimigos e plataformas)
    public float specialAttackCooldown = 0.5f; // cooldown entre tiros Q
    private float nextSpecialAttackTime = 0f;

    private Animator anim;
    private bool isAttacking = false;
    private SpriteRenderer spriteRenderer; // para verificar direção do player

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Ataque Normal com W
        if (!isAttacking && Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + 1f / attackRate;
        }

        // Ataque Especial com Q (um tiro por vez, se disponível)
        if (Time.time >= nextSpecialAttackTime && Input.GetKeyDown(KeyCode.Q) && CoinManager.instance.UseShot())
        {
            StartCoroutine(ShootProjectile());
            nextSpecialAttackTime = Time.time + specialAttackCooldown;
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetBool("AtaqueCorpo", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
                enemyScript.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("AtaqueCorpo", false);
        isAttacking = false;
    }

    IEnumerator ShootProjectile()
    {
        anim.SetBool("AtaqueCyberLuva", true); // ativa animação da luva

        // Dispara o tiro imediatamente
        GameObject projectile = Instantiate(projectilePrefab, handPoint.position, Quaternion.identity);
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            float direction = spriteRenderer.flipX ? -1f : 1f; // direção baseada no flip
            projScript.Initialize(direction * projectileSpeed, projectileDamage, projectileCollisionLayers);
        }

        // Espera a duração completa da animação antes de desativar
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("AtaqueCyberLuva", false);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}