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

    [Header("Potion To Automate")]
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

        buyButton.onClick.AddListener(BuyOrUpgradeApprentice);

        UpdateUI();
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
        }
    }

    public void BuyOrUpgradeApprentice()
    {
        if (targetPotion == null)
        {
            Debug.LogWarning(apprenticeName + " has no target potion assigned.");
            return;
        }

        if (!CurrencyManager.Instance.CanAfford(CurrentCost))
        {
            return;
        }

        CurrencyManager.Instance.SpendCoins(CurrentCost);

        if (apprenticeLevel == 0)
        {
            apprenticeLevel = 1;
            targetPotion.SetApprenticeAssigned(true);
        }
        else
        {
            apprenticeLevel++;
        }

        targetPotion.SetApprenticeSpeedMultiplier(SpeedMultiplier);

        UpdateUI();
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

        buyButtonText.text = apprenticeLevel == 0
            ? "Buy\n$" + CurrentCost.ToString("F0")
            : "Upgrade\n$" + CurrentCost.ToString("F0");

        buyButton.interactable = CurrencyManager.Instance.CanAfford(CurrentCost);
    }
}
