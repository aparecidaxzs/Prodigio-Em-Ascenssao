using UnityEngine;

public class TriggerDano : MonoBehaviour
{
    public int dano = 1; // quanto de dano causa

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no trigger é o Player
        if (collision.CompareTag("Player"))
        {
            // Tenta pegar o componente Player
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.TomarDano(dano); // chama o método de dano
            }
        }
    }
}
