using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração do Ataque")]
    public float attackRange = 0.5f;     // raio da área de ataque
    public int attackDamage = 1;        // dano que o ataque causa
    public Transform attackPoint;        // posição do ataque (um empty no frente do player)
    public LayerMask enemyLayers;        // camada dos inimigos

    [Header("Cooldown do Ataque")]
    public float attackRate = 2f;        // quantos ataques por segundo
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // tecla do ataque
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Aqui você pode chamar a animação no futuro
        // animator.SetTrigger("Attack");

        // Detecta inimigos na área do ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Aplica dano em cada inimigo atingido
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Acertou: " + enemy.name);
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

    // Gizmos para visualizar a área de ataque no editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
