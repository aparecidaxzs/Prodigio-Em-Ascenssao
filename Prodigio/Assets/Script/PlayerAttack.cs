using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Configurações básicas do ataque do jogador
    [Header("Configuração do Ataque")]
    public float attackRange = 0.5f;     // Raio da área de ataque (distância em que o ataque alcança inimigos)
    public int attackDamage = 1;        // Quantidade de dano que o ataque causa aos inimigos
    public Transform attackPoint;        // Posição do ataque (um objeto vazio filho do jogador, posicionado na frente)
    public LayerMask enemyLayers;        // Camada (Layer) dos inimigos que podem ser atingidos (configure no Inspector)

    // Configurações de cooldown para controlar a frequência dos ataques
    [Header("Cooldown do Ataque")]
    public float attackRate = 2f;        // Número de ataques por segundo (ex.: 2f = 1 ataque a cada 0.5 segundos)
    private float nextAttackTime = 0f;   // Tempo até o próximo ataque permitido (calculado internamente)

    // Método chamado a cada frame do jogo
    void Update()
    {
        // Verifica se o tempo atual permite um novo ataque (respeitando o cooldown)
        if (Time.time >= nextAttackTime)
        {
            // Detecta se a tecla Q foi pressionada (tecla de ataque)
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Executa o ataque
                Attack();
                // Define o próximo tempo permitido para ataque (baseado no attackRate)
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Método que executa a lógica do ataque
    void Attack()
    {
        // Verifica se o attackPoint foi definido no Inspector (evita erros)
        if (attackPoint == null)
        {
            // Log de erro se o attackPoint não estiver configurado
            Debug.LogError("AttackPoint não está definido no Inspector!");
            return; // Sai do método sem executar o ataque
        }

        // Aqui você pode chamar a animação de ataque no futuro (ex.: animator.SetTrigger("Attack");)

        // Detecta todos os inimigos na área circular do ataque (usando a Layer especificada)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Log para debug: mostra quantos inimigos foram detectados
        Debug.Log("Ataque executado! Inimigos detectados: " + hitEnemies.Length);

        // Loop para aplicar dano em cada inimigo detectado
        foreach (Collider2D enemy in hitEnemies)
        {
            // Log para debug: confirma qual inimigo foi acertado e sua Layer
            Debug.Log("Acertou: " + enemy.name + " (Layer: " + LayerMask.LayerToName(enemy.gameObject.layer) + ")");

            // Tenta obter o script EnemyAI do inimigo atingido
            EnemyAI enemyScript = enemy.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                // Aplica dano ao inimigo chamando o método TakeDamage
                enemyScript.TakeDamage(attackDamage);
                // Log para debug: confirma que o dano foi aplicado
                Debug.Log("Dano aplicado a " + enemy.name + "!");
            }
            else
            {
                // Log de aviso se o inimigo não tem o script EnemyAI
                Debug.LogWarning("Inimigo " + enemy.name + " não tem o script EnemyAI anexado!");
            }
        }

        // Log adicional se nenhum inimigo foi detectado (ajuda a debugar problemas)
        if (hitEnemies.Length == 0)
        {
            Debug.Log("Nenhum inimigo detectado na área de ataque. Verifique Layer, Collider ou posição do attackPoint.");
        }
    }

    // Método para desenhar gizmos no Editor (visualiza a área de ataque)
    void OnDrawGizmosSelected()
    {
        // Só desenha se o attackPoint estiver definido
        if (attackPoint == null) return;

        // Define a cor do gizmo (vermelho para área de ataque)
        Gizmos.color = Color.red;
        // Desenha uma esfera wireframe na posição do attackPoint com o raio definido
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
