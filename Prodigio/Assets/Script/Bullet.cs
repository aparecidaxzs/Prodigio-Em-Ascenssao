using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 1; 
    public float lifetime = 4f;

    private float direction = 1f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    public void SetDirection(float dir)
    {
        direction = dir;

        // vira o projétil
        if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.instance.BarradeVida(-damage); // dano ao player
            Destroy(gameObject);
        }

        if (collision.CompareTag("Chão"))
        {
            Destroy(gameObject);
        }
    }
}
