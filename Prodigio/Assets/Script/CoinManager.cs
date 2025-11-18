using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // singleton

    [Header("Estado")]
    public int coinCount = 0;
    public int shotsAvailable = 0; // tiros disponíveis, incrementados a cada 2 moedas

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
        int oldCoinCount = coinCount;
        coinCount += amount;

        // A cada 2 moedas coletadas, ganha 1 tiro
        int newCoins = coinCount - oldCoinCount;
        for (int i = 0; i < newCoins; i++)
        {
            if ((oldCoinCount + i + 1) % 2 == 0) // verifica se atingiu múltiplo de 2
            {
                shotsAvailable++;
            }
        }

        UpdateUI();
    }

    public bool UseShot()
    {
        if (shotsAvailable > 0)
        {
            shotsAvailable--;
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString(); // volta a mostrar apenas as moedas
    }

    public void ResetCoins()
    {
        coinCount = 0;
        shotsAvailable = 0;
        UpdateUI();
    }
}