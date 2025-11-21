using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class VictoryCondition : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Nome da próxima cena (se deixar vazio, a fase atual será apenas reiniciada caso falte pródigos).")]
    public string nextLevelName = "";

    public string menuP = "";

    [Tooltip("Tag usada pelos pródigios (os coletáveis devem usar esta mesma tag).")]
    public string prodigioTag = "Prodigios";

    [Tooltip("Tag usada para identificar o jogador.")]
    public string playerTag = "Player";

    // Quantidade total de pródigios na cena
    private int totalProdigios = 0;

    // Quantidade já coletada pelo jogador
    private int prodigiosColetados = 0;

    public GameObject vitoria0;
    public GameObject vitoria1;
    public GameObject vitoria2;
    public GameObject vitoria3;

    public GameObject botaoCasa;
    public GameObject botaoRein;
    public GameObject botaoSeguir;

    public GameObject botaoPause;

    public GameObject score;

    public GameObject panel;

    void Awake()
    {
        // Encontra todos os objetos na cena que têm a tag dos pródigios
        // Isso define quantos pródigios existem para serem coletados
        totalProdigios = GameObject.FindGameObjectsWithTag(prodigioTag).Length;

        // Debug opcional: mostra no console quantos pródigios foram encontrados
        // Debug.Log($"[VictoryCondition] Total pródigios na cena: {totalProdigios}");
    }

    void OnEnable()
    {
        // Inscreve a função HandleCollected no evento OnCollected do coletável
        // Assim, sempre que um pródigo for coletado, este script será notificado
        ProdigioCollect.OnCollected += HandleCollected;
    }

    void OnDisable()
    {
        // Remove a inscrição no evento quando o objeto for desativado
        // Isso evita erros e referências perdidas
        ProdigioCollect.OnCollected -= HandleCollected;
    }

    private void HandleCollected()
    {
        // Incrementa a contagem de pródigios coletados
        prodigiosColetados++;

        // Debug opcional para acompanhar no console a quantidade coletada
        // Debug.Log($"[VictoryCondition] Pródigios coletados: {prodigiosColetados}/{totalProdigios}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou na área é o Player
        if (!collision.CompareTag(playerTag)) return;

        // Debug opcional indicando que o player chegou na zona de vitória
        // Debug.Log("[VictoryCondition] Player entrou na zona de vitória. Verificando pródigios...");

        // Se já coletou todos os pródigios e existe pelo menos 1 pródigo na fase...
        if (prodigiosColetados >= totalProdigios && totalProdigios > 0)
        {
            /*vitoria3.SetActive(true);
            botaoCasa.SetActive(true);
            botaoRein.SetActive(true);
            botaoSeguir.SetActive(true);
            botaoPause.SetActive(false);
            score.SetActive(false);*/

            panel.SetActive(true);
            // Debug opcional
            // Debug.Log("[VictoryCondition] Todos os pródigios coletados! Carregando próxima fase...");

            // Carrega a próxima fase indicada
            //if (!string.IsNullOrEmpty(nextLevelName))
                //SceneManager.LoadScene(nextLevelName);
            // else
                // Se o nome estiver vazio, o código não faz nada (útil para testes)
                // Debug.Log("[VictoryCondition] nextLevelName está vazio — configure o nome da próxima cena.");
        }
        else
        {
            // Se faltou algum pródigo, reinicia a fase
            // Debug.Log("[VictoryCondition] Faltam pródigios. Reiniciando fase...");

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
