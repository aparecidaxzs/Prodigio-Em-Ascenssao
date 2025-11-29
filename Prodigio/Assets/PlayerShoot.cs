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


    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.time >= nextShotTime && Input.GetKeyDown(KeyCode.R))
        {
            Shoot();
            nextShotTime = Time.time + shotCooldown;
        }
    }

    void Shoot()
    {
        // Ativa animação (se existir)
        if (anim != null)
            anim.SetBool("AtaqueCyberLuva", true);
        // Instancia a bala
        GameObject bullet = Instantiate(bulletPrefab, handPoint.position, Quaternion.identity);

        BulletDamage bd = bullet.GetComponent<BulletDamage>();

        if (bd != null)
        {
            
            // Direção baseada no flip do sprite
            float direction = spriteRenderer.flipX ? -1f : 1f;

            // Envia configuração da bala
            bd.Init(direction * bulletSpeed, bulletDamage, bulletCollisionLayers);
        }
        else
        {
            Debug.LogWarning("O prefab da bala NÃO tem o script BulletDamage!");
        }
        anim.SetBool("AtaqueCyberLuva", false);
    }
}
