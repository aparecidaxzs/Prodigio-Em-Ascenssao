using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;          // velocidade do projétil
    public int damage = 1;            // dano causado ao player
    public float lifeTime = 3f;       // tempo até desaparecer

    private float direction = 1f;     // direção horizontal (+1 direita, -1 esquerda)
    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime); // destrói depois de alguns segundos
    }

    void Update()
    {
        rig.linearVelocity = new Vector2(direction * speed, rig.linearVelocity.y);
    }

    // Função chamada pelo inimigo
    public void SetDirection(float dir)
    {
        direction = dir;

        // vira sprite de acordo com a direção
        if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    // Quando encosta em algo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.BarradeVida(-damage);
            Destroy(gameObject);
        }

        // evita atravessar paredes
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
