using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Configuração do Tiro (E)")]
    public Transform handPoint;                  // Local de onde o tiro sai
    public GameObject bulletPrefab;              // Prefab da bala
    public float bulletSpeed = 10f;              // Velocidade da bala
    public int bulletDamage = 3;                 // Dano da bala
    public LayerMask bulletCollisionLayers;      // Layers que a bala atinge
    public float shotCooldown = 0.3f;            // Tempo entre tiros

    private float nextShotTime = 0f;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Aperta E e dispara
        if (Time.time >= nextShotTime && Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
            nextShotTime = Time.time + shotCooldown;
        }
    }

    void Shoot()
    {
        // dispara animação (opcional)
        if (anim != null)
            anim.SetTrigger("Shoot");

        // cria a bala
        GameObject bullet = Instantiate(bulletPrefab, handPoint.position, Quaternion.identity);

        // envia configuração para o script da bala
        BulletDamage bd = bullet.GetComponent<BulletDamage>();
        if (bd != null)
        {
            float direction = spriteRenderer.flipX ? -1f : 1f;
            bd.Init(direction * bulletSpeed, bulletDamage, bulletCollisionLayers);
        }
    }
}
