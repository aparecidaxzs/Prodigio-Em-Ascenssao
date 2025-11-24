using UnityEngine;
using System.Collections;

public class MostrarAlgoPorTempo : MonoBehaviour
{
    [Header("Objeto que será ativado e desativado")]
    public GameObject objeto;

    [Header("Tempo que ficará ativo")]
    public float tempoNaTela = 3f;

    void Start()
    {
        StartCoroutine(Mostrar());
    }

    IEnumerator Mostrar()
    {
        objeto.SetActive(true);         // Ativa o objeto
        yield return new WaitForSeconds(tempoNaTela); 
        objeto.SetActive(false);        // Desativa o objeto
    }
}
