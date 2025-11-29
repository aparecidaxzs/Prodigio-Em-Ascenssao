using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    public float lifeTime = 3f;

    private float direction = 1f;
    private Rigidbody2D rig;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        rig.linearVelocity = new Vector2(direction * speed, rig.linearVelocity.y);
    }

    // Chamado pelo inimigo
    public void SetDirection(float dir)
    {
        direction = Mathf.Sign(dir);

        // vira o sprite corretamente
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(dir);
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // DANO NO PLAYER
        Player p = collision.GetComponent<Player>();
        if (p != null)
        {
            p.TomarDano(damage);   
            Destroy(gameObject);
            return;
        }

        // PAREDES / CHÃO
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.CompareTag("Chão"))
        {
            Destroy(gameObject);
        }
    }
}
