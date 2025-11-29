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
        Destroy(gameObject, lifeTime);
    }

    public void Init(float bulletSpeed, int bulletDamage, LayerMask layers)
    {
        speed = bulletSpeed;
        damage = bulletDamage;
        collisionLayers = layers;

        // Ajusta orientação automática da bala
        if (speed < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void Update()
    {
        // Move para frente REAL
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Layer válida?
        if (((1 << collision.gameObject.layer) & collisionLayers) == 0)
            return;

        // Caso seja inimigo
        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        Destroy(gameObject);

        // Inimigo grande
        InimigoGrande grande = collision.GetComponent<InimigoGrande>();
        if (grande != null)
        grande.TakeDamage(damage);

        Destroy(gameObject);
    }
}
