using UnityEngine;

public class LifeUpgrade : MonoBehaviour
{
    public int aumentoMaxVida = 1; // quanto o maxVida vai aumentar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.AddVidaToda(aumentoMaxVida);
            }

            Destroy(gameObject); // destrói o power-up
        }
    }
}
