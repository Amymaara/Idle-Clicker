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

    [SerializeField] private Slider timerSlider;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button apprenticeButton;

    [Header("Unlock Settings")]
    [SerializeField] private bool startsUnlocked = false;
    [SerializeField] private double unlockCost = 100;
    [SerializeField] private GameObject lockedOverlay;
    [SerializeField] private TMP_Text unlockCostText;
    [SerializeField] private Button unlockButton;

    private bool isUnlocked;
    private int potionLevel = 1;
    private bool isProducing = false;
    private bool hasApprentice = false;
    private float apprenticeSpeedMultiplier = 1f;

    private double CurrentProfit => baseProfit * potionLevel * CharmManager.Instance.ProfitMultiplier;
    private double CurrentUpgradeCost => baseUpgradeCost * Mathf.Pow(1.12f, potionLevel);

    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
        isUnlocked = startsUnlocked;

        upgradeButton.onClick.AddListener(UpgradePotion);
        apprenticeButton.onClick.AddListener(AssignApprentice);

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

        if (CurrencyManager.Instance.CanAfford(unlockCost))
        {
            CurrencyManager.Instance.SpendCoins(unlockCost);
            isUnlocked = true;

            Debug.Log(potionName + " unlocked!");

            UpdateUI();
        }
    }
    public void StartProduction()
    {
        if (!isUnlocked) return;

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
    (CharmManager.Instance.SpeedMultiplier * apprenticeSpeedMultiplier);

        while (timer < modifiedProductionTime)
        {
            timer += Time.deltaTime;

            timerSlider.value = timer / modifiedProductionTime;
            timerText.text = timer.ToString("F1") + " / " + modifiedProductionTime.ToString("F1") + "s";

            yield return null;
        }

        CurrencyManager.Instance.AddCoins(CurrentProfit);

        timerSlider.value = 0;
        timerText.text = productionTime.ToString("F1") + "s";

        isProducing = false;

        UpdateUI();
    }

    public void UpgradePotion()
    {
        if (!isUnlocked) return;

        double cost = CurrentUpgradeCost;

        if (CurrencyManager.Instance.CanAfford(cost))
        {
            CurrencyManager.Instance.SpendCoins(cost);
            potionLevel++;
            UpdateUI();
        }
    }

    public void AssignApprentice()
    {
        hasApprentice = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        potionNameText.text = potionName;
        profitText.text = "Makes: $" + CurrentProfit.ToString("F0");
        amountMadeText.text = "Amount Made: " + amountMade;
        levelText.text = "Level " + potionLevel;
        upgradeCostText.text = "Upgrade\n$" + CurrentUpgradeCost.ToString("F0");
        apprenticeText.text = hasApprentice ? "Apprentice\nAssigned" : "No\nApprentice";
        lockedOverlay.SetActive(!isUnlocked);
        unlockCostText.text = "Unlock\n$" + unlockCost.ToString("F0");
        upgradeButton.interactable = CurrencyManager.Instance.CanAfford(CurrentUpgradeCost);
    }
}
