using UnityEngine;

public class ChestShot : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float speed = 6f;         // Velocidade inicial
    public float lifetime = 3f;      // Tempo até sumir sozinho
    public int damage = 1;           // Dano causado ao player

    private Rigidbody2D rig;
    private float direction = 1f;    // Direção do tiro

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        rig.linearVelocity = new Vector2(direction * speed, rig.linearVelocity.y);
    }

    // Chamado pelo inimigo para definir direção do projétil
    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir);

        // Ajusta visualmente o sprite
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Acertou o player
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.BarradeVida(-damage);
            Destroy(gameObject);
        }

        // Pegou no chão (ou qualquer outra layer que você escolher)
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
