using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmCard : MonoBehaviour
{
    [Header("Charm Data")]
    public string charmName;
    public string description;
    public double cost;

    public double profitMultiplier = 1;
    public float speedMultiplier = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyButton;

    private bool isBought = false;

    private void Start()
    {
        buyButton.onClick.AddListener(BuyCharm);
        UpdateUI();
    }

    private void BuyCharm()
    {
        if (isBought) return;

        if (CurrencyManager.Instance.CanAfford(cost))
        {
            CurrencyManager.Instance.SpendCoins(cost);

            CharmManager.Instance.ApplyCharm(profitMultiplier, speedMultiplier);

            isBought = true;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        nameText.text = charmName;
        descriptionText.text = description;
        statText.text = GetStatText();
        costText.text = "$" + cost;

        buyButton.interactable = !isBought;
    }

    private string GetStatText()
    {
        if (profitMultiplier > 1)
            return "Profit x" + profitMultiplier;

        if (speedMultiplier > 1)
            return "Speed x" + speedMultiplier;

        return "";
    }
}
