using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    else
    {
        Destroy(gameObject);
        return;
    }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
 // Procura o texto da UI sempre que a cena abrir
    if (coinText == null)
        coinText = GameObject.FindWithTag("CoinText")?.GetComponent<TextMeshProUGUI>();

    UpdateUI();
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