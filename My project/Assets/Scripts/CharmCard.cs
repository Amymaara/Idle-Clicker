using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmCard : MonoBehaviour
{
    [Header("Charm Data")]
    [SerializeField] private string charmName;
    [SerializeField] private string description;
    [SerializeField] private double cost;
    [SerializeField] private double profitMultiplier = 1;
    [SerializeField] private float speedMultiplier = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text statText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button assignButton;
    [SerializeField] private TMP_Text assignButtonText;

    private bool isBought = false;

    public bool IsBought => isBought;
    public double ProfitMultiplier => profitMultiplier;
    public float SpeedMultiplier => speedMultiplier;
    public string CharmName => charmName;

    private void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateUI;
        buyButton.onClick.AddListener(BuyCharm);
        assignButton.onClick.AddListener(ToggleAssignCharm);
        UpdateUI();
    }

    private void OnDisable()
    {
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnCurrencyChanged -= UpdateUI;
    }

    public void BuyCharm()
    {
        if (isBought) return;

        if (CurrencyManager.Instance.CanAfford(cost))
        {
            CurrencyManager.Instance.SpendCoins(cost);
            isBought = true;
            UpdateUI();
        }
    }

    public void ToggleAssignCharm()
    {
        if (!isBought) return;

        if (CharmManager.Instance.IsCharmActive(this))
        {
            CharmManager.Instance.RemoveCharm(this);
        }
        else
        {
            CharmManager.Instance.AssignCharm(this);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        nameText.text = charmName;
        descriptionText.text = description;
        statText.text = GetStatText();
        costText.text = NumberFormatter.FormatMoney(cost);

        buyButton.interactable = !isBought;
        assignButton.interactable = isBought;

        if (!isBought)
        {
            assignButtonText.text = "Locked";
        }
        else if (CharmManager.Instance.IsCharmActive(this))
        {
            assignButtonText.text = "Remove";
        }
        else
        {
            assignButtonText.text = "Assign";
        }
    }

    private string GetStatText()
    {
        if (profitMultiplier > 1)
            return "Profit x" + profitMultiplier;

        if (speedMultiplier > 1)
            return "Speed x" + speedMultiplier;

        return "Modifier";
    }
}
