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
        rb.linearVelocity = new Vector2(speed, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug das colisões
        Debug.Log("Projétil colidiu com: " + collision.gameObject.name);

        // Verifica layer válida
        if (((1 << collision.gameObject.layer) & collisionLayers) == 0)
        {
            Debug.Log("Layer não permitida — ignorando");
            return;
        }

        // ===============================
        // 1️⃣ Tenta achar EnemyAI
        // ===============================
        EnemyAI ai = collision.GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.TakeDamage(damage);
            Debug.Log("Dano aplicado ao EnemyAI: " + damage);
            Destroy(gameObject);
            return;
        }

        // ===============================
        // 2️⃣ Tenta achar InimigoGrande
        // ===============================
        InimigoGrande grande = collision.GetComponent<InimigoGrande>();
        if (grande != null)
        {
            grande.TakeDamage(damage);
            Debug.Log("Dano aplicado ao InimigoGrande: " + damage);
            Destroy(gameObject);
            return;
        }

        // ===============================
        // 3️⃣ Se não for nenhum dos dois
        // ===============================
        Debug.Log("Colidiu com algo sem script de inimigo");
        Destroy(gameObject);
    }
}
