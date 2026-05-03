using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    private double currentCoins = 0;
    private double CurrentCoins => currentCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateCoinUI();
    }
    public void AddCoins(double amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public bool CanAfford(double amount)
    {
        return currentCoins >= amount;
    }
    public void SpendCoins(double amount)
    {
        if (!CanAfford(amount))
        {
            return;
        }

        currentCoins -= amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = "$" + currentCoins.ToString("F0");
    }
}

