using UnityEngine;
using System;

public class ProdigioCollect : MonoBehaviour
{
    // Evento público para notificar o VictoryCondition
    public static event Action OnCollected;

    [Tooltip("Tag do jogador")]
    public string playerTag = "Player";

    public GameObject prod1;
    public GameObject prod2;
    public GameObject prod3;

    private static int coletados = 0; // contador global de coletáveis

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        coletados++;

        Debug.Log($"[ProdigioCollect] Coletável '{gameObject.name}' coletado. Total: {coletados}");

        // Ativa o objeto correspondente
        if (coletados == 1 && prod1 != null)
            prod1.SetActive(true);
        else if (coletados == 2 && prod2 != null)
            prod2.SetActive(true);
        else if (coletados == 3 && prod3 != null)
            prod3.SetActive(true);

        OnCollected?.Invoke(); // notifica o VictoryCondition
        Destroy(gameObject);
    }
}
