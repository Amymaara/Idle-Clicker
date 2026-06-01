using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeUpgradeCard : MonoBehaviour
{
    [Header("Upgrade")]
    [SerializeField] private PrestigeUpgradeType upgradeType;

    [Header("UI")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text bonusText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button upgradeButton;

    private void Start()
    {
        upgradeButton.onClick.AddListener(BuyUpgrade);
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void BuyUpgrade()
    {
        PrestigeManager.Instance.BuyUpgrade(upgradeType);
    }

    private void UpdateUI()
    {
        if (PrestigeManager.Instance == null) return;

        int level = PrestigeManager.Instance.GetUpgradeLevel(upgradeType);
        int cost = PrestigeManager.Instance.GetUpgradeCost(upgradeType);

        levelText.text = "Level " + level;
        costText.text = "Cost: " + cost + " Essence";

        switch (upgradeType)
        {
            case PrestigeUpgradeType.Wealth:
                titleText.text = "Arcane Wealth";
                bonusText.text = "+" + (level * 5) + "% Profit Forever";
                break;

            case PrestigeUpgradeType.Efficiency:
                titleText.text = "Arcane Efficiency";
                bonusText.text = "+" + (level * 5) + "% Brew Speed Forever";
                break;

            case PrestigeUpgradeType.Apprenticeship:
                titleText.text = "Arcane Apprenticeship";
                bonusText.text = "+" + (level * 5) + "% Apprentice Speed Forever";
                break;

            case PrestigeUpgradeType.Duplication:
                titleText.text = "Arcane Duplication";
                bonusText.text = "+" + level + " Global Batch";
                break;
        }

        upgradeButton.interactable =
            PrestigeManager.Instance.ArcaneEssence >= cost;
    }
}