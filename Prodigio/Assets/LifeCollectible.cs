using UnityEngine;

public class LifeCollectible : MonoBehaviour
{
    public int vidaParaDar = 1; // quanto de vida o item restaura

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                // só adiciona vida se o player não estiver com vida cheia
                if (player.vidaAtual < player.maxVida)
                {
                    player.AddVida(vidaParaDar);
                }
            }

            Destroy(gameObject); // destrói o coletável depois de pegar
        }
    }
}
