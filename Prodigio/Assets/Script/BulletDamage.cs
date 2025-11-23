using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    private float speed;
    private int damage;
    private LayerMask collisionLayers;

    [Header("Tempo para desaparecer")]
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // evita acumular balas infinitas
    }

    // Inicializa os valores da bala (chamado pelo PlayerShoot)
    public void Init(float bulletSpeed, int bulletDamage, LayerMask layers)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        collisionLayers = layers;
    }

    void Update()
    {
        // Move a bala continuamente
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se colidiu com uma layer válida
        if (((1 << collision.gameObject.layer) & collisionLayers) != 0)
        {
            // Se for inimigo, aplicar dano
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // destrói a bala após atingir algo
            Destroy(gameObject);
        }
    }
}
