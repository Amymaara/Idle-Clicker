using UnityEngine;
using TMPro;
using System;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private double startingCoins = 0;
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private TMP_Text coinText;
    private double currentCoins = 0;
    private double CurrentCoins => currentCoins;

    public event Action OnCurrencyChanged;

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
        currentCoins = startingCoins;
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

    public void UpdateCoinUI()
    {
        coinText.text = NumberFormatter.FormatMoney(currentCoins);
        OnCurrencyChanged?.Invoke();
    }
}

