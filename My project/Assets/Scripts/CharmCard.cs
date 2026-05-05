using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CharmType
{
    Profit,
    Speed,
    Growth,
    CostReduction,
    ApprenticeSpeed,
    Chaos
}

public class CharmCard : MonoBehaviour
{
    [Header("Charm Data")]
    [SerializeField] private string charmName;
    [SerializeField] private string description;
    [SerializeField] private double cost;
    [SerializeField] private CharmType charmType;
    [SerializeField] private float effectValue = 1.25f;

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
    public string CharmName => charmName;
    public CharmType CharmType => charmType;
    public float EffectValue => effectValue;

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

    public void UpdateUI()
    {
        nameText.text = charmName;
        descriptionText.text = description;
        statText.text = GetStatText();
        costText.text = isBought ? "Owned" : NumberFormatter.FormatMoney(cost);

        bool canAfford = CurrencyManager.Instance.CanAfford(cost);

        buyButton.interactable = !isBought;
        assignButton.interactable = isBought;

        buyButton.image.color = !isBought && canAfford
            ? new Color(0.6f, 1f, 0.6f)
            : new Color(0.6f, 0.6f, 0.6f);

        if (!isBought)
            assignButtonText.text = "Locked";
        else if (CharmManager.Instance.IsCharmActive(this))
            assignButtonText.text = "Remove";
        else
            assignButtonText.text = "Assign";
    }

    private string GetStatText()
    {
        return charmType switch
        {
            CharmType.Profit => "Profit x" + effectValue,
            CharmType.Speed => "Speed x" + effectValue,
            CharmType.Growth => "+1 Potion/Round",
            CharmType.CostReduction => "Costs -" + Mathf.RoundToInt(effectValue * 100) + "%",
            CharmType.ApprenticeSpeed => "Apprentice Speed x" + effectValue,
            CharmType.Chaos => "Random Bonus",
            _ => "Modifier"
        };
    }

    public bool IsAvailableToBuy()
    {
        return !isBought && CurrencyManager.Instance.CanAfford(cost);
    }
}