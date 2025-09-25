using UnityEngine;

public class ZoomSprite : MonoBehaviour
{
    public Vector3 escalaFinal = new Vector3(2f, 2f, 2f); // tamanho final do zoom
    public float duracao = 5f; // tempo em segundos para completar o zoom

    private Vector3 escalaInicial;
    private float tempoPassado = 0f;

    void Start()
    {
        // Pega a escala inicial da sprite
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        // Atualiza o tempo decorrido
        tempoPassado += Time.deltaTime;

        // Calcula o percentual do tempo que já passou
        float t = Mathf.Clamp01(tempoPassado / duracao);

        // Faz a interpolação da escala
        transform.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
    }
}
