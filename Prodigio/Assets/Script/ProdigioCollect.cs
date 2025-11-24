using UnityEngine;
using System;

public class ProdigioCollect : MonoBehaviour
{
    // Evento público para notificar o VictoryCondition
    public static event Action OnCollected;

    [Tooltip("Tag do jogador")]
    public string playerTag = "Player";

    [Header("Som")]
    public AudioClip somColetavel; // som da moeda/coletável


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;
            AudioManager.instance.PlaySFX(somColetavel);

        Debug.Log($"[ProdigioCollect] Coletável '{gameObject.name}' coletado pelo Player.");
        OnCollected?.Invoke(); // notifica o VictoryCondition
        Destroy(gameObject);
    }
}
