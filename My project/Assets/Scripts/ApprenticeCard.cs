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
    [SerializeField] private TMP_Text upgradePreviewText;


    [Header("Tutorial Target")]
    [SerializeField] private RectTransform apprenticeTabTutorialTarget;
    [SerializeField] private RectTransform apprenticeBuyButtonTutorialTarget;

    [Header("Locked Overlay")]
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TMP_Text lockedReasonText;
    [SerializeField] private PulseButtons pulseButton;

    private int apprenticeLevel = 0;
    private bool hasShownApprenticeTutorial = false;
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
    private float GetSpeedAtLevel(int level)
    {
        if (level <= 0) return 1f;
        return 1f + ((level - 1) * speedIncreasePerLevel);
    }
    public void BuyOrUpgradeApprentice()
    {
        if (targetPotion == null) return;
        if (!targetPotion.IsUnlocked) return;

        int upgradeAmount = GetUpgradeAmount();

        if (upgradeAmount <= 0) return;

        double totalCost = GetBulkUpgradeCost(upgradeAmount);

        if (!CurrencyManager.Instance.CanAfford(totalCost)) return;

        TutorialManager.Instance.ShowTutorial(
         "Buy an apprentice to automate Basic Brew.",
         apprenticeBuyButtonTutorialTarget,
         TutorialAction.BuyApprentice
        );

        CurrencyManager.Instance.SpendCoins(totalCost);

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.TryCompleteTutorial(TutorialAction.BuyApprentice);
        }

        SoundManager.Instance.PlaySound(SoundType.Upgrade);

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
        bool potionUnlocked = targetPotion != null && targetPotion.IsUnlocked;

        Debug.Log(
            gameObject.name +
            " target potion: " +
            (targetPotion != null ? targetPotion.GetPotionName() : "NULL") +
            " | unlocked: " + potionUnlocked
        );

        if (lockedOverlay != null)
        {
            lockedOverlay.SetActive(!potionUnlocked);
        }

        nameText.text = apprenticeName;
        descriptionText.text = description;

        levelText.text = apprenticeLevel == 0
            ? "Not Owned"
            : "Level " + apprenticeLevel;

        stat1Text.text = "Automation";
        stat1IncreaseText.text = apprenticeLevel == 0 ? "Off" : "On";

        stat2Text.text = "Speed";
        stat2IncreaseText.text = "x" + SpeedMultiplier.ToString("F1");

        if (!potionUnlocked)
        {
            buyButtonText.text = "Potion\nLocked";
            buyButton.interactable = false;
            levelText.text = "Locked";
            stat1IncreaseText.text = "Off";
            stat2IncreaseText.text = "Locked";
            return;
        }

        int upgradeAmount = GetUpgradeAmount();
        double bulkCost = GetBulkUpgradeCost(upgradeAmount);

        if (upgradePreviewText != null)
        {
            if (targetPotion == null || !targetPotion.IsUnlocked || upgradeAmount <= 0)
            {
                upgradePreviewText.text = "";
            }
            else
            {
                float currentSpeed = GetSpeedAtLevel(apprenticeLevel);
                float previewSpeed = GetSpeedAtLevel(apprenticeLevel + upgradeAmount);

                upgradePreviewText.text =
                    "> x" + previewSpeed.ToString("F1");
            }
        }

        buyButtonText.text = apprenticeLevel == 0
            ? "Buy\n" + NumberFormatter.FormatMoney(bulkCost)
            : "Upgrade x" + upgradeAmount + "\n" + NumberFormatter.FormatMoney(bulkCost);

        bool canAfford = CurrencyManager.Instance.CanAfford(bulkCost);
        bool canUpgrade = upgradeAmount > 0;

                buyButton.interactable = canUpgrade && canAfford;

        buyButton.image.color = (canUpgrade && canAfford)
            ? new Color(0.6f, 1f, 0.6f)   // green
            : new Color(0.6f, 0.6f, 0.6f); // grey

        // pulseButton.SetPulse(canUpgrade && canAfford);

        if (!hasShownApprenticeTutorial &&
    apprenticeLevel == 0 &&
    targetPotion != null &&
    targetPotion.IsUnlocked &&
    CurrencyManager.Instance.CanAfford(GetBulkUpgradeCost(1)))
        {
            hasShownApprenticeTutorial = true;

            TutorialManager.Instance.ShowTutorial(
             "Open the Apprentice tab to hire someone to automate brewing.",
             apprenticeTabTutorialTarget,
             TutorialAction.OpenApprenticeTab
            );
        }
    }

    public bool IsAvailableToBuy()
    {
        if (targetPotion == null) return false;
        if (!targetPotion.IsUnlocked) return false;

        int upgradeAmount = GetUpgradeAmount();
        double bulkCost = GetBulkUpgradeCost(upgradeAmount);

        return upgradeAmount > 0 && CurrencyManager.Instance.CanAfford(bulkCost);
    }
}