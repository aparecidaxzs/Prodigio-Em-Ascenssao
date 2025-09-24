using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.instance != null)
                CoinManager.instance.AddCoin(value);
            else
                Debug.LogWarning("CoinManager.instance é null — verifique se CoinManager está na cena.");

            //DontDestroyOnLoad(gameObject);
        }
    }
}
