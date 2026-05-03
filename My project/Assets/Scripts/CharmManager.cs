using UnityEngine;

public class CharmManager : MonoBehaviour
{
    public static CharmManager Instance { get; private set; }

    public double ProfitMultiplier { get; private set; } = 1;
    public float SpeedMultiplier { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void BuyProfitCharm(double cost, double multiplier)
    {
        if (CurrencyManager.Instance.CanAfford(cost))
        {
            CurrencyManager.Instance.SpendCoins(cost);
            ProfitMultiplier *= multiplier;
        }
    }

    public void BuySpeedCharm(double cost, float multiplier)
    {
        if (CurrencyManager.Instance.CanAfford(cost))
        {
            CurrencyManager.Instance.SpendCoins(cost);
            SpeedMultiplier *= multiplier;
        }
    }

    public void ApplyCharm(double profitMultiplier, float speedMultiplier)
    {
        ProfitMultiplier *= profitMultiplier;
        SpeedMultiplier *= speedMultiplier;
    }

}
