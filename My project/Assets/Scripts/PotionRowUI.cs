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
    [SerializeField] private TMP_Text timeNeededText;
    [SerializeField] private TMP_Text amountMadeText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text upgradeCostText;
    [SerializeField] private TMP_Text apprenticeText;

    [SerializeField] private Slider timerSlider;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button apprenticeButton;

    private int potionLevel = 1;
    private bool isProducing = false;
    private bool hasApprentice = false;

    private double CurrentProfit => baseProfit * potionLevel;
    private double CurrentUpgradeCost => baseUpgradeCost * Mathf.Pow(1.15f, potionLevel);

    private void Start()
    {
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

    public void StartProduction()
    {
        if (!isProducing)
        {
            StartCoroutine(ProducePotion());
        }
    }

    private IEnumerator ProducePotion()
    {
        isProducing = true;

        float timer = 0f;

        while (timer < productionTime)
        {
            timer += Time.deltaTime;

            timerSlider.value = timer / productionTime;
            timerText.text = timer.ToString("F1") + " / " + productionTime.ToString("F1") + "s";

            yield return null;
        }

        CurrencyManager.Instance.AddCoins(CurrentProfit);

        timerSlider.value = 0;
        timerText.text = productionTime.ToString("F1") + "s";

        isProducing = false;

        UpdateUI();
    }

    private void UpgradePotion()
    {
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
        timeNeededText.text = "Time Needed: " + productionTime.ToString("F1") + "s";
        amountMadeText.text = "Amount Made: " + amountMade;
        levelText.text = "Level " + potionLevel;
        upgradeCostText.text = "Upgrade\n$" + CurrentUpgradeCost.ToString("F0");
        apprenticeText.text = hasApprentice ? "Assigned" : "No Apprentice";
    }
}
