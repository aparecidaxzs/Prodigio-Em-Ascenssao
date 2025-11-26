using UnityEngine;

public class ChestShot : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float speed = 6f;
    public float lifetime = 3f;
    public int damage = 1;

    private Rigidbody2D rig;
    private float direction = 1f;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        rig.linearVelocity = new Vector2(direction * speed, rig.linearVelocity.y);
    }

    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir);

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
            p.TomarDano(damage); // <-- AGORA CORRETO
            Destroy(gameObject);
            return;
        }

        // Se bater no chão
        if (collision.CompareTag("Ground") || collision.CompareTag("Chão"))
        {
            Destroy(gameObject);
        }
    }
}
