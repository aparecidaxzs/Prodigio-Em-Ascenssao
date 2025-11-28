using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryCondition : MonoBehaviour
{
    [Header("Configurações")]
    public string nextLevelName = "";
    public string menuP = "";

    public string prodigioTag = "Prodigios";
    public string playerTag = "Player";

    private int totalProdigios = 0;
    private int prodigiosColetados = 0;

    [Header("Objetos de Vitória")]
    public GameObject vitoria0;

    public GameObject botaoCasa;
    public GameObject botaoRein;
    public GameObject botaoSeguir;
    public GameObject botaoPause;
    public GameObject score;
    public GameObject panel;

    [Header("Novos Objetos")]
    public GameObject objetoQueSobe;  // objeto que vai subir
    public GameObject diamanteObj;    // diamante que aparece por 3s

    public GameObject voltarProd;

    private bool jaAtivou = false; // evita chamar duas vezes

    void Awake()
    {
        totalProdigios = GameObject.FindGameObjectsWithTag(prodigioTag).Length;
    }

    void OnEnable()
    {
        ProdigioCollect.OnCollected += HandleCollected;
    }

    void OnDisable()
    {
        ProdigioCollect.OnCollected -= HandleCollected;
    }

    private void HandleCollected()
    {
        prodigiosColetados++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        if (jaAtivou) return; // impede chamar 2x
        jaAtivou = true;

        if (prodigiosColetados >= 3) // verifica os 3 pródigios
        {
            StartCoroutine(SequenciaVitoria());
        }
        else
        {
            voltarProd.SetActive(true);
            
        }
    }

    IEnumerator SequenciaVitoria()
    {
        //AudioManager.instance.PlayVictory();
        // 1) faz o objeto subir
        yield return StartCoroutine(FazerObjetoSubir());

        // 2) ativa o diamante
        diamanteObj.SetActive(true);

        // 3) espera 3 segundos
        yield return new WaitForSeconds(3f);

        // 4) desativa o diamante
        diamanteObj.SetActive(false);

        // 5) ativa o painel e tudo mais exatamente como você queria
        panel.SetActive(true);
        botaoPause.SetActive(false);
        score.SetActive(false);

        vitoria0.SetActive(true);

        botaoCasa.SetActive(true);
        botaoRein.SetActive(true);
        botaoSeguir.SetActive(true);
    }

    IEnumerator FazerObjetoSubir()
    {
        float duracao = 1.2f;
        float velocidade = 2f;

        float tempo = 0f;
        Vector3 posInicial = objetoQueSobe.transform.position;

        while (tempo < duracao)
        {
            objetoQueSobe.transform.position += Vector3.up * velocidade * Time.deltaTime;
            tempo += Time.deltaTime;
            yield return null;
        }
    }

    public void Segrui()
    {
        SceneManager.LoadScene(nextLevelName);
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Casa()
    {
        SceneManager.LoadScene(menuP);
    }
}
