using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // singleton

    [Header("Estado")]
    public int coinCount = 0;
    public int shotsAvailable = 0; // 1 moeda = 1 tiro

    [Header("UI")]
    public TextMeshProUGUI coinText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // mantém entre cenas
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        ResetCoins();
    }

    public void AddCoin(int amount = 1)
    {
        coinCount += amount;
        shotsAvailable += amount;
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
            coinText.text = coinCount.ToString();
    }

    public void ResetCoins()
    {
        coinCount = 0;
        shotsAvailable = 0;
        UpdateUI();
    }
}