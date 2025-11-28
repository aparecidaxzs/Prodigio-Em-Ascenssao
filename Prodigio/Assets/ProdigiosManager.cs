using UnityEngine;

public class ProdigiosManager : MonoBehaviour
{
    [Header("Configurações")]
    public int totalProdigios = 3;
    private int coletados = 0;

    [Header("UI de aviso")]
    public GameObject avisoUI;
    public float tempoAviso = 0.2f;

    private Collider2D colliderFisico;   // bloqueia
    private Collider2D triggerDetector;  // detecta player


    private void Awake()
    {
        // Pega automaticamente os dois colliders
        Collider2D[] colls = GetComponents<Collider2D>();
        foreach (var c in colls)
        {
            if (c.isTrigger) triggerDetector = c;
            else colliderFisico = c;
        }
    }

    private void OnEnable()
    {
        ProdigioCollect.OnCollected += RegistrarColeta;
    }

    private void OnDisable()
    {
        ProdigioCollect.OnCollected -= RegistrarColeta;
    }

    private void Start()
    {
        if (avisoUI != null)
            avisoUI.SetActive(false);
    }

    private void RegistrarColeta()
    {
        coletados++;

        // Se coletou tudo → libera a passagem
        if (coletados >= totalProdigios)
            LiberarPassagem();
    }


    private void LiberarPassagem()
    {
        // Desativa o collider físico → player pode passar
        if (colliderFisico != null)
            colliderFisico.enabled = false;

        // Não mostra aviso nesse caso
    }


    // Detecta quando o player encosta no TRIGGER da barreira
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Se o player JÁ tem todos → NÃO mostra aviso
        if (coletados >= totalProdigios) return;

        // Caso não tenha os 3 → mostra aviso
        MostrarAviso();
    }


    private void MostrarAviso()
    {
        if (avisoUI == null) return;

        avisoUI.SetActive(true);
        CancelInvoke(nameof(EsconderAviso));
        Invoke(nameof(EsconderAviso), tempoAviso);
    }

    private void EsconderAviso()
    {
        avisoUI.SetActive(false);
    }
}
