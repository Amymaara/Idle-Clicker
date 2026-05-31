using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ApprenticeTrainingType
{
    Speed,
    Profit,
    Batch
}

public class ApprenticeTrainingUpgradeRow : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text currentBonusText;

    private ApprenticeCard apprentice;
    private ApprenticeTrainingType trainingType;

    private void Start()
    {
        buyButton.onClick.AddListener(BuyUpgrade);

        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    public void Setup(ApprenticeCard apprenticeCard, ApprenticeTrainingType type)
    {
        apprentice = apprenticeCard;
        trainingType = type;

        UpdateUI();
    }

    private void BuyUpgrade()
    {
        if (apprentice == null) return;

        apprentice.BuyTrainingUpgrade(trainingType);
        UpdateUI();

        if (ApprenticeTrainingPopup.Instance != null)
            ApprenticeTrainingPopup.Instance.UpdateMasteryPointText();
    }

    private void UpdateUI()
    {
        if (apprentice == null) return;

        int level = apprentice.GetTrainingLevel(trainingType);
        int cost = apprentice.GetTrainingCost(trainingType);

        upgradeNameText.text = trainingType switch
        {
            ApprenticeTrainingType.Speed => "Faster Brewing",
            ApprenticeTrainingType.Profit => "Better Potions",
            ApprenticeTrainingType.Batch => "Double Batch",
            _ => "Training"
        };

        descriptionText.text = trainingType switch
        {
            ApprenticeTrainingType.Speed => "+10% apprentice speed per level.",
            ApprenticeTrainingType.Profit => "+10% automated profit per level.",
            ApprenticeTrainingType.Batch => "+1 potion made per round.",
            _ => ""
        };

        currentBonusText.text = trainingType switch
        {
            ApprenticeTrainingType.Speed => "Current Bonus: +" + (level * 10) + "% Speed",
            ApprenticeTrainingType.Profit => "Current Bonus: +" + (level * 10) + "% Profit",
            ApprenticeTrainingType.Batch => "Current Bonus: +" + level + " Batch",
            _ => ""
        };

        levelText.text = "Level " + level;
        costText.text = cost + " MP";

        bool canAfford = apprentice.AvailableMasteryPoints >= cost;

        buyButton.interactable = canAfford;

        buyButton.image.color = canAfford
            ? new Color(0.6f, 1f, 0.6f)
            : new Color(0.6f, 0.6f, 0.6f);
    }
}