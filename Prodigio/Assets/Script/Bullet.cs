using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    private float direction;

    public void Setup(float dir, float bulletSpeed)
    {
        direction = dir;
        speed = bulletSpeed;

        // vira o sprite se precisar
        if (dir < 0)
        {
            Vector3 s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
        }

        Destroy(gameObject, 4f);
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player p = col.GetComponent<Player>();
            if (p != null)
                p.BarradeVida(-damage);

            Destroy(gameObject);
        }

        if (col.CompareTag("Ground"))
            Destroy(gameObject);
    }
}
