using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [Header("Configuração do Ataque (W)")]
    public Transform handPoint;                     // ponto de saída do tiro
    public GameObject projectilePrefab;             // prefab do projétil
    public float projectileSpeed = 10f;             // velocidade do tiro
    public int projectileDamage = 1;                // dano
    public LayerMask projectileCollisionLayers;     // Layers que o tiro deve colidir

    [Header("Cooldown do Ataque")]
    public float attackRate = 0.3f;                 // tempo entre disparos
    private float nextAttackTime = 0f;

    private Animator anim;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("AtaqueCyberLuva", true);
            TryAttack();
        }

        else
        {
            anim.SetBool("AtaqueCyberLuva", true);
        }
    }

    private void TryAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackRate;
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        // instanciar o projétil
        GameObject projectile = Instantiate(projectilePrefab, handPoint.position, Quaternion.identity);

        // direção depende do flip do personagem
        float direction = spriteRenderer.flipX ? -1f : 1f;

        // inicializar o projétil com velocidade, dano e layers
        projectile.GetComponent<Projectile>().Initialize(
            projectileSpeed * direction,
            projectileDamage,
            projectileCollisionLayers
        );
    }
}
