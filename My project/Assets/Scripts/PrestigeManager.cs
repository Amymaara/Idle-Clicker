using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeManager : MonoBehaviour
{
    public static PrestigeManager Instance { get; private set; }

    [Header("Currency")]
    [SerializeField] private int arcaneEssence = 0;
    [SerializeField] private int highestClaimedTier = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text lifetimeCoinsText;
    [SerializeField] private TMP_Text currentTierText;
    [SerializeField] private TMP_Text essenceGainText;
    [SerializeField] private TMP_Text totalEssenceText;
    [SerializeField] private Button ascendButton;

    [Header("Permanent Bonus UI")]
    [SerializeField] private TMP_Text profitBonusText;
    [SerializeField] private TMP_Text speedBonusText;
    [SerializeField] private TMP_Text apprenticeBonusText;
    [SerializeField] private TMP_Text batchBonusText;

    [SerializeField] private int wealthLevel = 0;
    [SerializeField] private int efficiencyLevel = 0;
    [SerializeField] private int apprenticeshipLevel = 0;
    [SerializeField] private int duplicationLevel = 0;
    public float PermanentProfitMultiplier => 1f + (wealthLevel * 0.05f);
    public float PermanentSpeedMultiplier => 1f + (efficiencyLevel * 0.05f);
    public float PermanentApprenticeSpeedMultiplier => 1f + (apprenticeshipLevel * 0.05f);
    public int PermanentBatchBonus => duplicationLevel;

    public int ArcaneEssence => arcaneEssence;

    private void Start()
    {
        if (ascendButton != null)
            ascendButton.onClick.AddListener(Ascend);
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateUI();
    }

    public int GetUpgradeLevel(PrestigeUpgradeType type)
    {
        return type switch
        {
            PrestigeUpgradeType.Wealth => wealthLevel,
            PrestigeUpgradeType.Efficiency => efficiencyLevel,
            PrestigeUpgradeType.Apprenticeship => apprenticeshipLevel,
            PrestigeUpgradeType.Duplication => duplicationLevel,
            _ => 0
        };
    }

    public int GetUpgradeCost(PrestigeUpgradeType type)
    {
        int level = GetUpgradeLevel(type);

        if (type == PrestigeUpgradeType.Duplication)
        {
            return 3 * Mathf.RoundToInt(Mathf.Pow(2, level));
        }

        return Mathf.RoundToInt(Mathf.Pow(2, level));
    }

    public void BuyUpgrade(PrestigeUpgradeType type)
    {
        int cost = GetUpgradeCost(type);

        if (arcaneEssence < cost) return;

        arcaneEssence -= cost;

        switch (type)
        {
            case PrestigeUpgradeType.Wealth:
                wealthLevel++;
                break;

            case PrestigeUpgradeType.Efficiency:
                efficiencyLevel++;
                break;

            case PrestigeUpgradeType.Apprenticeship:
                apprenticeshipLevel++;
                break;

            case PrestigeUpgradeType.Duplication:
                duplicationLevel++;
                break;
        }

        UpdateUI();
    }

    public void Ascend()
    {
        int essenceGain = GetEssenceGain();

        if (essenceGain <= 0)
        {
            Debug.Log("Need to reach a higher tier before ascending.");
            return;
        }

        arcaneEssence += essenceGain;
        highestClaimedTier = GetCurrentTier();
        ResetRun();

        Debug.Log(
            "Ascended! Gained " +
            essenceGain +
            " Arcane Essence."
        );

        UpdateUI();
    }

    public int GetCurrentTier()
    {
        double lifetimeCoins = CurrencyManager.Instance.LifetimeCoinsEarned;

        if (lifetimeCoins < 1_000_000)
            return 0;

        return Mathf.FloorToInt(
            Mathf.Log10((float)lifetimeCoins)
        ) - 5;
    }

    public int GetEssenceGain()
    {
        int currentTier = GetCurrentTier();

        return Mathf.Max(0, currentTier - highestClaimedTier);
    }

    private void UpdateUI()
    {
        if (CurrencyManager.Instance == null) return;

        lifetimeCoinsText.text =
            "Lifetime Earnings: " +
            NumberFormatter.FormatMoney(
                CurrencyManager.Instance.LifetimeCoinsEarned
            );

        currentTierText.text =
            "Current Tier: " +
            GetCurrentTier();

        essenceGainText.text =
            "Essence Gain: " +
            GetEssenceGain();

        totalEssenceText.text =
            "Arcane Essence: " +
            arcaneEssence;


        if (profitBonusText != null)
            profitBonusText.text = "Profit: +" + (wealthLevel * 5) + "%";

        if (speedBonusText != null)
            speedBonusText.text = "Brew Speed: +" + (efficiencyLevel * 5) + "%";

        if (apprenticeBonusText != null)
            apprenticeBonusText.text = "Apprentice Speed: +" + (apprenticeshipLevel * 5) + "%";

        if (batchBonusText != null)
            batchBonusText.text = "Global Batch: +" + duplicationLevel;
    }

    private void ResetRun()
    {
        CurrencyManager.Instance.ResetCurrentRunCurrency();

        PotionRowUI[] potions = FindObjectsByType<PotionRowUI>(FindObjectsSortMode.None);

        foreach (PotionRowUI potion in potions)
        {
            potion.ResetForPrestige();
        }

        ApprenticeCard[] apprentices =
    FindObjectsByType<ApprenticeCard>(FindObjectsSortMode.None);

        foreach (ApprenticeCard apprentice in apprentices)
        {
            apprentice.ResetForPrestige();
        }

        CharmCard[] charms =
    FindObjectsByType<CharmCard>(FindObjectsSortMode.None);

        foreach (CharmCard charm in charms)
        {
            charm.ResetForPrestige();
        }

        if (CharmManager.Instance != null)
        {
            CharmManager.Instance.ResetForPrestige();
        }

        PotionMilestonUpgrade[] milestones =
    FindObjectsByType<PotionMilestonUpgrade>(FindObjectsSortMode.None);

        foreach (PotionMilestonUpgrade milestone in milestones)
        {
            milestone.ResetForPrestige();
        }

        if (AchievementManager.Instance != null)
        {
            AchievementManager.Instance.ResetForPrestige();
        }

        Debug.Log("Run reset after ascension.");


    }
}