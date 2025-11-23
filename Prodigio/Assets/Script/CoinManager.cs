using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // singleton

    [Header("Estado")]
    public int coinCount = 0;
    public int shotsAvailable = 0; // agora 1 moeda = 1 tiro

    [Header("UI")]
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ResetCoins();
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        shotsAvailable += amount; // <<< cada moeda dá 1 tiro

        UpdateUI();
    }

    public bool UseShot()
    {
        if (shotsAvailable > 0)
        {
            shotsAvailable--;  // gasta 1 tiro
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString(); // segue mostrando apenas moedas
    }

    public void ResetCoins()
    {
        coinCount = 0;
        shotsAvailable = 0;
        UpdateUI();
    }
}
