using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Configuração do Tiro (R)")]
    public Transform handPoint;                  // Local de onde o tiro sai
    public GameObject bulletPrefab;              // Prefab da bala
    public float bulletSpeed = 10f;              // Velocidade da bala
    public int bulletDamage = 3;                 // Dano da bala
    public LayerMask bulletCollisionLayers;      // Layers que a bala atinge
    public float shotCooldown = 0.2f;            // Tempo entre tiros

    private float nextShotTime = 0f;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Sons")]
    public AudioClip somTiro;                    // Som do tiro do player

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Aperta R e dispara
        if (Time.time >= nextShotTime && Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
            nextShotTime = Time.time + shotCooldown;
        }
    }

    void Shoot()
    {
        // Dispara animação (opcional)
        if (anim != null)
            anim.SetTrigger("Shoot");

        // Toca som do tiro
        AudioManager.instance.PlaySFX(somTiro);

        // Cria a bala
        GameObject bullet = Instantiate(bulletPrefab, handPoint.position, Quaternion.identity);

        // Envia configuração para o script da bala
        BulletDamage bd = bullet.GetComponent<BulletDamage>();
        if (bd != null)
        {
            float direction = spriteRenderer.flipX ? -1f : 1f;
            bd.Init(direction * bulletSpeed, bulletDamage, bulletCollisionLayers);
        }
    }
}
