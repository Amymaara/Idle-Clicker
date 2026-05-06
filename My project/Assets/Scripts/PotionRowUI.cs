using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionRowUI : MonoBehaviour
{
    [Header("Potion Settings")]
    [SerializeField] private string potionName = "Basic Potion";
    [SerializeField] private double baseProfit = 5;
    [SerializeField] private double baseUpgradeCost = 10;
    [SerializeField] private float productionTime = 2f;
    [SerializeField] private int amountMade = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text potionNameText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text profitText;
    [SerializeField] private TMP_Text amountMadeText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text apprenticeText;
    [SerializeField] private TMP_Text upgradePreviewText;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button apprenticeButton;

    [SerializeField] private FloatingMoneyTextUI floatingMoneyPrefab;
    [SerializeField] private Transform floatingMoneySpawnPoint;

    [Header("Unlock Settings")]
    [SerializeField] private bool startsUnlocked = false;
    [SerializeField] private double unlockCost = 100;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Button unlockButton;

    [Header("Milestone Upgrade Settings")]
    [SerializeField] private int potionsMadePerRound = 1;

    [Header("Tutorial")]
    [SerializeField] private BuyModeTutorialController buyModeTutorialController;

    public int PotionLevel => potionLevel;
    public bool IsUnlocked => isUnlocked || startsUnlocked;
    public int PotionsMadePerRound => potionsMadePerRound;

    private bool isUnlocked;
    private int potionLevel = 1;
    private bool isProducing = false;
    private bool hasApprentice = false;
    private float apprenticeSpeedMultiplier = 1f;

    private bool hasShownFirstEarningsTutorial = false;
    private bool hasShownUnlockPotionTutorial = false;

    private double CurrentProfit =>
     baseProfit *
     potionLevel *
     Mathf.Pow(1.015f, potionLevel - 1) *
     (potionsMadePerRound + CharmManager.Instance.GlobalPotionBonus) *
     CharmManager.Instance.ProfitMultiplier;

    private void Awake()
    {
        isUnlocked = startsUnlocked;
    }

    private void Start()
    {
        if (UpgradeBuyModeManager.Instance != null)
            UpgradeBuyModeManager.Instance.OnBuyModeChanged += UpdateUI;

        CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;

        upgradeButton.onClick.AddListener(UpgradePotion);

        if (apprenticeButton != null)
            apprenticeButton.onClick.AddListener(AssignApprentice);

        if (unlockButton != null)
            unlockButton.onClick.AddListener(UnlockPotion);

        timerSlider.value = 0;
        UpdateUI();
    }

    private void Update()
    {
        if (hasApprentice && !isProducing)
        {
            StartProduction();
        }
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;

        if (UpgradeBuyModeManager.Instance != null)
            UpgradeBuyModeManager.Instance.OnBuyModeChanged -= UpdateUI;
    }

    public string GetPotionName()
    {
        return potionName;
    }

    private double GetProfitAtLevel(int level)
    {
        return baseProfit *
               level *
               Mathf.Pow(1.015f, level - 1) *
               (potionsMadePerRound + CharmManager.Instance.GlobalPotionBonus) *
               CharmManager.Instance.ProfitMultiplier;
    }
    private int GetUpgradeAmount()
    {
        if (UpgradeBuyModeManager.Instance.CurrentMode == UpgradeBuyMode.One)
            return 1;

        if (UpgradeBuyModeManager.Instance.CurrentMode == UpgradeBuyMode.Ten)
            return 10;

        return GetMaxAffordableUpgrades();
    }

    private int GetMaxAffordableUpgrades()
    {
        int affordableAmount = 0;
        double totalCost = 0;
        int simulatedLevel = potionLevel;

        while (true)
        {
            double nextCost =
                baseUpgradeCost *
                Mathf.Pow(1.12f, simulatedLevel) *
                CharmManager.Instance.CostReductionMultiplier;

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
        int simulatedLevel = potionLevel;

        for (int i = 0; i < amount; i++)
        {
            totalCost +=
                baseUpgradeCost *
                Mathf.Pow(1.12f, simulatedLevel) *
                CharmManager.Instance.CostReductionMultiplier;

            simulatedLevel++;
        }

        return totalCost;
    }

    public void IncreasePotionsMadePerRound()
    {
        potionsMadePerRound++;
        UpdateUI();
    }

    public void SetApprenticeAssigned(bool value)
    {
        hasApprentice = value;
        UpdateUI();
    }

    public void SetApprenticeSpeedMultiplier(float multiplier)
    {
        apprenticeSpeedMultiplier = multiplier;
    }

    public void UnlockPotion()
    {
        if (isUnlocked) return;
        if (!CurrencyManager.Instance.CanAfford(unlockCost)) return;

        CurrencyManager.Instance.SpendCoins(unlockCost);
        isUnlocked = true;

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.TryCompleteTutorial(TutorialAction.UnlockPotion);

        Debug.Log(potionName + " unlocked!");

        UpdateUI();
    }

    public void StartProduction()
    {
        if (!isUnlocked) return;

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.TryCompleteTutorial(TutorialAction.BrewPotion);

        if (!isProducing)
        {
            StartCoroutine(ProducePotion());
        }
    }

    private IEnumerator ProducePotion()
    {
        isProducing = true;

        float timer = 0f;

        float modifiedProductionTime = productionTime /
            (CharmManager.Instance.SpeedMultiplier *
             apprenticeSpeedMultiplier *
             CharmManager.Instance.ApprenticeSpeedMultiplier);

        while (timer < modifiedProductionTime)
        {
            timer += Time.deltaTime;

            timerSlider.value = timer / modifiedProductionTime;
            timerText.text = timer.ToString("F1") + " / " + modifiedProductionTime.ToString("F1") + "s";

            yield return null;
        }

        double earnedAmount = CharmManager.Instance.ApplyChaosBonus(CurrentProfit);

        CurrencyManager.Instance.AddCoins(earnedAmount);
        SpawnFloatingMoney(earnedAmount);

        if (!hasShownFirstEarningsTutorial && potionName == "Basic Brew")
        {
            hasShownFirstEarningsTutorial = true;

            TutorialManager.Instance.ShowTutorial(
                "Each brew earns coins. Use them to upgrade your potions.",
                upgradeButton.GetComponent<RectTransform>(),
                TutorialAction.UpgradePotion
            );
        }

        timerSlider.value = 0;
        timerText.text = productionTime.ToString("F1") + "s";

        isProducing = false;

        UpdateUI();
    }

    private void SpawnFloatingMoney(double amount)
    {
        if (floatingMoneyPrefab == null || floatingMoneySpawnPoint == null)
            return;

        Vector3 randomOffset = new Vector3(Random.Range(-20f, 20f), 0f, 0f);

        FloatingMoneyTextUI floatingText = Instantiate(
            floatingMoneyPrefab,
            floatingMoneySpawnPoint.position + randomOffset,
            Quaternion.identity,
            floatingMoneySpawnPoint.parent
        );

        floatingText.Setup(amount);
    }

    public void UpgradePotion()
    {
        if (!isUnlocked) return;

        int upgradeAmount = GetUpgradeAmount();

        if (upgradeAmount <= 0) return;

        double totalCost = GetBulkUpgradeCost(upgradeAmount);
        
        if (!CurrencyManager.Instance.CanAfford(totalCost)) return;

        SoundManager.Instance.PlaySound(SoundType.Upgrade);

        CurrencyManager.Instance.SpendCoins(totalCost);

        potionLevel += upgradeAmount;

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.TryCompleteTutorial(TutorialAction.UpgradePotion);

        UpdateUI();
    }

    public void AssignApprentice()
    {
        hasApprentice = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        potionNameText.text = potionName;
        profitText.text = "Makes: " + NumberFormatter.FormatMoney(CurrentProfit);

        amountMadeText.text = "Potions: " +
            (potionsMadePerRound + CharmManager.Instance.GlobalPotionBonus);

        levelText.text = "Level " + potionLevel;

        apprenticeText.text = hasApprentice
            ? "Apprentice\nAssigned"
            : "No\nApprentice";

        if (lockedOverlay != null)
            lockedOverlay.SetActive(!isUnlocked);

        if (unlockCostText != null)
            unlockCostText.text = "Locked\n" + NumberFormatter.FormatMoney(unlockCost);

        int upgradeAmount = GetUpgradeAmount();
        double bulkCost = GetBulkUpgradeCost(upgradeAmount);

        if (upgradePreviewText != null)
        {
            if (!isUnlocked || upgradeAmount <= 0)
            {
                upgradePreviewText.text = "";
            }
            else
            {
                double previewProfit = GetProfitAtLevel(potionLevel + upgradeAmount);
                upgradePreviewText.text = "> " + NumberFormatter.FormatMoney(previewProfit);
            }
        }

        bool canUpgrade = upgradeAmount > 0;
        bool canAfford = canUpgrade && CurrencyManager.Instance.CanAfford(bulkCost);

        upgradeCostText.text = !canUpgrade
            ? "Upgrade\nN/A"
            : "Upgrade x" + upgradeAmount + "\n" + NumberFormatter.FormatMoney(bulkCost);

        upgradeButton.interactable =
            isUnlocked &&
            canUpgrade &&
            canAfford;

        upgradeButton.image.color =
            isUnlocked && canUpgrade && canAfford
                ? new Color(0.6f, 1f, 0.6f)
                : new Color(0.6f, 0.6f, 0.6f);

        if (!hasShownUnlockPotionTutorial &&
            !isUnlocked &&
            potionName == "Healing Potion" &&
            CurrencyManager.Instance.CanAfford(unlockCost))
        {
            hasShownUnlockPotionTutorial = true;

            TutorialManager.Instance.ShowTutorial(
                "Unlock new potions for bigger profits.",
                unlockButton.GetComponent<RectTransform>(),
                TutorialAction.UnlockPotion
            );
        }
    }
}
