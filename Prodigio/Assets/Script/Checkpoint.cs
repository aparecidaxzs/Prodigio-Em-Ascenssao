using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint checkpointAtivo; // armazena o checkpoint único ativo

    public bool foiUsado = false; // revive somente uma vez

    [Header("Sprites")]
    public Sprite spriteDesativado;
    public Sprite spriteAtivado;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = spriteDesativado;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Desativa outro checkpoint se existir
            if (checkpointAtivo != null && checkpointAtivo != this)
            {
                checkpointAtivo.Desativar();
            }

            // Ativa este
            checkpointAtivo = this;
            sr.sprite = spriteAtivado;
        }
    }

    public void Desativar()
    {
        sr.sprite = spriteDesativado;
    }
}
