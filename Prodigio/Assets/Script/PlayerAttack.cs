using UnityEngine;
using System.Collections; // necessário para usar Coroutine

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração do Ataque")]
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public Transform attackPoint;
    public LayerMask enemyLayers;

    [Header("Cooldown do Ataque")]
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    private Animator anim;
    private bool isAttacking = false; // evita repetir ataque durante a animação

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // só pode atacar se não estiver atacando e o cooldown permitir
        if (!isAttacking && Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        // ativa a animação
        anim.SetBool("AtaqueCorpo", true);

        // aplica o dano imediatamente (ou após um pequeno delay se quiser sincronizar com o golpe)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
                enemyScript.TakeDamage(attackDamage);
        }

        // espera o tempo de duração da animação antes de desligar
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // desativa a animação e permite novo ataque
        anim.SetBool("AtaqueCorpo", false);
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
