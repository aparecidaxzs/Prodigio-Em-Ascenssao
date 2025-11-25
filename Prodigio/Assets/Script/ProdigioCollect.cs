using UnityEngine;
using System;

public class ProdigioCollect : MonoBehaviour
{
    // Evento público para notificar o VictoryCondition
    public static event Action OnCollected;

    [Tooltip("Tag do jogador")]
    public string playerTag = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;
        Debug.Log($"[ProdigioCollect] Coletável '{gameObject.name}' coletado pelo Player.");
        OnCollected?.Invoke(); // notifica o VictoryCondition
        Destroy(gameObject);
    }
}
