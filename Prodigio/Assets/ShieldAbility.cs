using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    [Header("Configuração do Escudo (E)")]
    public GameObject shieldObject;          // Objeto visual do escudo (ativar/desativar)
    public float shieldDuration = 10f;       // Tempo de escudo ativo
    public float shieldCooldown = 30f;       // Tempo até poder usar de novo

    private bool isShieldActive = false;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        // Garante que o escudo comece desligado
        if (shieldObject != null)
            shieldObject.SetActive(false);
    }

    void Update()
    {
        // Ativar escudo no E
        if (Input.GetKeyDown(KeyCode.E) && !isShieldActive && !isOnCooldown)
        {
            ActivateShield();
        }

        // Contagem do cooldown
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    void ActivateShield()
    {
        isShieldActive = true;

        if (shieldObject != null)
            shieldObject.SetActive(true);

        // Desliga depois de X segundos
        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    void DeactivateShield()
    {
        isShieldActive = false;

        if (shieldObject != null)
            shieldObject.SetActive(false);

        // inicia cooldown
        isOnCooldown = true;
        cooldownTimer = shieldCooldown;
    }

    // Função pública que pode ser checada pelo Player, inimigos, espinhos, etc.
    public bool GetShieldState()
    {
        return isShieldActive;
    }
}
