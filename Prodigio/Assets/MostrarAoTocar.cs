using UnityEngine;
using System.Collections;

public class MostrarAoTocar : MonoBehaviour
{
    [Header("Objeto que vai aparecer")]
    public GameObject objetoParaMostrar;

    [Header("Tempo que ficará visível")]
    public float tempoVisivel = 10f;

    private bool jáAtivou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !jáAtivou)
        {
            jáAtivou = true;
            StartCoroutine(Mostrar());
        }
    }

    IEnumerator Mostrar()
    {
        objetoParaMostrar.SetActive(true);   // Ativa o objeto
        yield return new WaitForSeconds(tempoVisivel);
        objetoParaMostrar.SetActive(false);  // Desativa
    }
}
