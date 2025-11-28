using UnityEngine;
using System.Collections;

public class LaserAtivavel : MonoBehaviour
{
    [Header("Configurações do Laser")]
    //public int dano = 1;              
    public float tempoLigado = 2f;    
    public float tempoDesligado = 2f; 
    public bool comecaLigado = true;  

    private Collider2D col;
    private SpriteRenderer sr;
    private bool ligado;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        ligado = comecaLigado;
        AtualizarLaser();

        StartCoroutine(CicloLaser());
    }

    IEnumerator CicloLaser()
    {
        while (true)
        {
            if (ligado)
                yield return new WaitForSeconds(tempoLigado);
            else
                yield return new WaitForSeconds(tempoDesligado);

            ligado = !ligado;
            AtualizarLaser();
        }
    }

    void AtualizarLaser()
    {
        if (col != null) col.enabled = ligado;
        if (sr != null) sr.enabled = ligado;
    }
}
