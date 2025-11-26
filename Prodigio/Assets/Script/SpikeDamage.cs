using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [Header("Dano causado ao tocar")]
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Player.instance != null)
            {
                Player.instance.TomarDano(damage);
            }
        }
    }
}
