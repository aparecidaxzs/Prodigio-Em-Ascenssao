using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private int damage;
    private LayerMask collisionLayers;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float speed, int damage, LayerMask collisionLayers)
    {
        this.speed = speed;
        this.damage = damage;
        this.collisionLayers = collisionLayers;
        rb.linearVelocity = new Vector2(speed, 0); // move na direção horizontal
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projétil colidiu com: " + collision.gameObject.name + " na Layer: " + LayerMask.LayerToName(collision.gameObject.layer)); // Debug para ver colisões

        if (((1 << collision.gameObject.layer) & collisionLayers) != 0) // verifica se é inimigo ou plataforma
        {
            Debug.Log("Colisão válida com Layer permitida."); // Confirma se LayerMask está funcionando

            EnemyAI enemy = collision.GetComponent<EnemyAI>(); // corrigido para EnemyAI
            //EnemyShooter enemyPuncher = collision.GetComponent<EnemyShooter>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // dá dano no inimigo
                Debug.Log("Dano aplicado ao inimigo: " + damage); // Confirma dano
            }
            /*else if (enemyPuncher != null)
            {
                enemyPuncher.TakeDamage(damage);
            }*/
            else
            {
                Debug.Log("Objeto não tem script EnemyAI."); // Se não encontrar o script
            }
            Destroy(gameObject); // destrói o projétil
        }
        else
        {
            Debug.Log("Colisão ignorada (Layer não permitida)."); // Se LayerMask falhar
        }
    }
}