using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ApprenticeCard : MonoBehaviour
{
    [Header("Apprentice Data")]
    [SerializeField] private string apprenticeName = "Potion Apprentice";
    [SerializeField] private string description = "Automatically brews this potion.";
    [SerializeField] private double baseCost = 250;
    [SerializeField] private float costScaling = 1.25f;
    [SerializeField] private float speedIncreasePerLevel = 0.1f;

    [Header("Target Potion")]
    [SerializeField] private PotionRowUI targetPotion;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text stat1Text;
    [SerializeField] private TMP_Text stat1IncreaseText;
    [SerializeField] private TMP_Text stat2Text;
    [SerializeField] private TMP_Text stat2IncreaseText;
    [SerializeField] private TMP_Text buyButtonText;
    [SerializeField] private Button buyButton;

    private int apprenticeLevel = 0;

    private double CurrentCost => baseCost * Mathf.Pow(costScaling, apprenticeLevel);

    private float SpeedMultiplier
    {
        get
        {
            if (apprenticeLevel <= 0)
                return 1f;

            return 1f + ((apprenticeLevel - 1) * speedIncreasePerLevel);
        }
    }

    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
        ApprenticeBuyModeManager.Instance.OnBuyModeChanged += UpdateUI;

        buyButton.onClick.AddListener(BuyOrUpgradeApprentice);

        UpdateUI();
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;

        if (ApprenticeBuyModeManager.Instance != null)
            ApprenticeBuyModeManager.Instance.OnBuyModeChanged -= UpdateUI;
    }

    public void BuyOrUpgradeApprentice()
    {
        if (targetPotion == null) return;

        int upgradeAmount = GetUpgradeAmount();

        if (upgradeAmount <= 0) return;

        double totalCost = GetBulkUpgradeCost(upgradeAmount);

        if (!CurrencyManager.Instance.CanAfford(totalCost)) return;

        CurrencyManager.Instance.SpendCoins(totalCost);

        // First purchase unlocks apprentice
        if (apprenticeLevel == 0)
        {
            apprenticeLevel = 1;
            targetPotion.SetApprenticeAssigned(true);
            upgradeAmount--; // first purchase used
        }

        apprenticeLevel += upgradeAmount;

        targetPotion.SetApprenticeSpeedMultiplier(SpeedMultiplier);

        UpdateUI();
    }

    private int GetUpgradeAmount()
    {
        if (ApprenticeBuyModeManager.Instance.CurrentMode == ApprenticeBuyMode.One)
            return 1;

        if (ApprenticeBuyModeManager.Instance.CurrentMode == ApprenticeBuyMode.Ten)
            return 10;

        return GetMaxAffordableUpgrades();
    }

    private int GetMaxAffordableUpgrades()
    {
        int affordableAmount = 0;
        double totalCost = 0;
        int simulatedLevel = apprenticeLevel;

        while (true)
        {
            double nextCost = baseCost * Mathf.Pow(costScaling, simulatedLevel);

            if (!CurrencyManager.Instance.CanAfford(totalCost + nextCost))
                break;

            totalCost += nextCost;
            simulatedLevel++;
            affordableAmount++;
        }

        return affordableAmount;
    }

    private double GetBulkUpgradeCost(int amount)
    {
        double totalCost = 0;
        int simulatedLevel = apprenticeLevel;

        for (int i = 0; i < amount; i++)
        {
            totalCost += baseCost * Mathf.Pow(costScaling, simulatedLevel);
            simulatedLevel++;
        }

        return totalCost;
    }

    public void UpdateUI()
    {
        nameText.text = apprenticeName;
        descriptionText.text = description;

        levelText.text = apprenticeLevel == 0
            ? "Not Owned"
            : "Level " + apprenticeLevel;

        stat1Text.text = "Automation";
        stat1IncreaseText.text = apprenticeLevel == 0 ? "Off" : "On";

        stat2Text.text = "Speed";
        stat2IncreaseText.text = "x" + SpeedMultiplier.ToString("F1");

        int upgradeAmount = GetUpgradeAmount();
        double bulkCost = GetBulkUpgradeCost(upgradeAmount);

        buyButtonText.text = apprenticeLevel == 0
            ? "Buy\n" + NumberFormatter.FormatMoney(bulkCost)
            : "Upgrade x" + upgradeAmount + "\n" + NumberFormatter.FormatMoney(bulkCost);

        buyButton.interactable =
            upgradeAmount > 0 &&
            CurrencyManager.Instance.CanAfford(bulkCost);
    }
}