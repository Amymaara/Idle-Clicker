using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionMilestonUpgrade : MonoBehaviour
{
    [Header("Upgrade Data")]
    [SerializeField] private string upgradeName = "Potion Batch Upgrade";
    [SerializeField] private string description = "Increases potions made per round.";
    [SerializeField] private int firstMilestoneLevel = 50;
    [SerializeField] private int milestoneIncrease = 50;
    [SerializeField] private double baseUpgradeCost = 1000;
    [SerializeField] private float costScaling = 2f;

    [Header("Target Potion")]
    [SerializeField] private PotionRowUI targetPotion;

    [Header("UI")]
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statIncreaseText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text upgradeButtonText;
    [SerializeField] private Image progressFill;
    [SerializeField] private Button upgradeButton;

    private int upgradesPurchased = 0;

    private int CurrentMilestoneLevel =>
        firstMilestoneLevel + (upgradesPurchased * milestoneIncrease);

    private double CurrentUpgradeCost =>
      baseUpgradeCost * Mathf.Pow(costScaling, upgradesPurchased + 1);

    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
        upgradeButton.onClick.AddListener(BuyUpgrade);
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
        }
    }

    public void BuyUpgrade()
    {
        if (targetPotion == null) return;
        if (targetPotion.PotionLevel < CurrentMilestoneLevel) return;
        if (!CurrencyManager.Instance.CanAfford(CurrentUpgradeCost)) return;

        CurrencyManager.Instance.SpendCoins(CurrentUpgradeCost);

        targetPotion.IncreasePotionsMadePerRound();
        upgradesPurchased++;

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (targetPotion == null) return;

        bool ready = targetPotion.PotionLevel >= CurrentMilestoneLevel;

        upgradeNameText.text = upgradeName;
        descriptionText.text = description;
        statIncreaseText.text = "+1 Potion Made";

        levelText.text = "Requires Level " + CurrentMilestoneLevel;

        progressText.text =
            "Current Level: " +
            targetPotion.PotionLevel +
            " / " +
            CurrentMilestoneLevel;

        float progress = (float)targetPotion.PotionLevel / CurrentMilestoneLevel;
        progressFill.fillAmount = Mathf.Clamp01(progress);

        upgradeButtonText.text = ready
            ? "Upgrade\n$" + CurrentUpgradeCost.ToString("F0")
            : "Locked";

        upgradeButton.interactable =
            ready &&
            CurrencyManager.Instance.CanAfford(CurrentUpgradeCost);

        bool canAfford = CurrencyManager.Instance.CanAfford(CurrentUpgradeCost);

        upgradeButton.image.color = ready && canAfford
            ? new Color(0.6f, 1f, 0.6f)
            : new Color(0.6f, 0.6f, 0.6f);
    }
}
