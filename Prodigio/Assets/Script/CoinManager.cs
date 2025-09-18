using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    // Acesso global (singleton simples)
    public static CoinManager instance;

    [Header("Estado")]
    public int coinCount = 0;

    [Header("UI")]
    public TextMeshProUGUI coinText; // arraste o TextMeshPro do Canvas para aqui

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // opcional: persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateUI();
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = coinCount.ToString(); // ou $"Moedas: {coinCount}"
    }

    // Métodos auxiliares (opcionais)
    public void SetCoins(int value) { coinCount = value; UpdateUI(); }
    public void ResetCoins() { coinCount = 0; UpdateUI(); }

    // Salvar/carregar rápido (opcional)
    public void SaveCoins() { PlayerPrefs.SetInt("coins", coinCount); PlayerPrefs.Save(); }
    public void LoadCoins() { coinCount = PlayerPrefs.GetInt("coins", 0); UpdateUI(); }
}
