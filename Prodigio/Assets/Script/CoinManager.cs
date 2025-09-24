using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // singleton

    [Header("Estado")]
    public int coinCount = 0;

    [Header("UI")]
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        // garante que sempre exista um "instance" válido
        instance = this;
    }

    private void Start()
    {
        ResetCoins(); // toda vez que uma fase inicia, começa do zero
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString();
    }

    public void ResetCoins()
    {
        coinCount = 0;
        UpdateUI();
    }
}
